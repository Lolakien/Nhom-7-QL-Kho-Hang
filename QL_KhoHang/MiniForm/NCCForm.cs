using QL_KhoHang.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace QL_KhoHang.MiniForm
{
    public partial class NCCForm : Form
    {
        NCCController controller = new NCCController();
        private HttpClient _httpClient;
        public NCCForm()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private void NCCForm_Load(object sender, EventArgs e)
        {
            var lstNCC = controller.GetAllNCC();
            dgvNCC.DataSource = lstNCC;
            LoadCities();
        }
        void loadData()
        {
            var lstNCC = controller.GetAllNCC();
            dgvNCC.DataSource = lstNCC;
        }
        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng dữ liệu được click
                DataGridViewRow row = dgvNCC.Rows[e.RowIndex];

                // Hiển thị thông tin vào các TextBox tương ứng
                txtMaNCC.Text = row.Cells["NhaCungCapID"].Value.ToString();  // Mã NCC
                txtTenNCC.Text = row.Cells["TenNhaCungCap"].Value.ToString(); // Tên NCC
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();       // Địa chỉ
                txtSDT.Text = row.Cells["DienThoai"].Value.ToString();       // SDT
            }
        }

        private void bunifuDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

  
        private async Task LoadCities()
        {
            try
            {
                using (_httpClient)
                {
                    // Gọi API lấy dữ liệu thành phố
                    var response = await _httpClient.GetStringAsync("https://provinces.open-api.vn/api/p/");

                    // Deserialize JSON thành đối tượng dynamic
                    var cities = JsonConvert.DeserializeObject<List<dynamic>>(response);

                    // Cập nhật ComboBox với dữ liệu thành phố
                    cboThanhPho.DataSource = cities;
                    cboThanhPho.DisplayMember = "name";
                    cboThanhPho.ValueMember = "code";  // Mã thành phố (dùng làm mã NCC)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thành phố: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string tenNCC = txtTenNCC.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string thanhPho = cboThanhPho.Text; 
            if (string.IsNullOrEmpty(tenNCC) || string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(thanhPho))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }
            if (controller.AddNCC(txtMaNCC.Text, tenNCC, diaChi, sdt, cboThanhPho.Text))  
            {
                MessageBox.Show("Thêm NCC thành công!");
                loadData(); 
            }
            else
            {
                MessageBox.Show("Không thể thêm NCC!");
            }
        }

        private void UpdateMaNCC()
        {
            // Lấy mã thành phố từ ComboBox
            string maThanhPho = ((dynamic)cboThanhPho.SelectedItem)?.code; // Mã thành phố

            // Lấy 3 số cuối của số điện thoại
            string sdt = txtSDT.Text.Trim();
            string lastThreeDigits = sdt.Length >= 3 ? sdt.Substring(sdt.Length - 3) : "000"; // Lấy 3 số cuối

            // Nếu mã thành phố và số điện thoại hợp lệ thì tạo Mã NCC
            if (!string.IsNullOrEmpty(maThanhPho) && !string.IsNullOrEmpty(lastThreeDigits))
            {
                string maNCC = "NCC" + maThanhPho + lastThreeDigits;
                txtMaNCC.Text = maNCC; // Hiển thị Mã NCC lên textbox
            }
            else
            {
                txtMaNCC.Text = ""; // Nếu không có đủ thông tin, để trống
            }
        }

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            UpdateMaNCC();
        }

        private void cboThanhPho_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMaNCC();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maNCC = txtMaNCC.Text.Trim();

            if (string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để xóa!");
                return;
            }

            // Xóa nhà cung cấp
            if (controller.DeleteNCC(maNCC))
            {
                MessageBox.Show("Xóa NCC thành công!");
                loadData();
            }
            else
            {
                MessageBox.Show("Không thể xóa NCC!");
            }
        }
    }
}
