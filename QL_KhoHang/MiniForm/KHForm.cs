using QL_KhoHang.Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Forms;
using BunifuAnimatorNS;
using System.Threading.Tasks;

namespace QL_KhoHang.MiniForm
{
    public partial class KHForm : Form
    {
        KhachHangController controller = new KhachHangController();
        private HttpClient _httpClient;
        public KHForm()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private void KHForm_Load(object sender, EventArgs e)
        {
            var lstKH = controller.GetAllKH();
            dgvKH.DataSource = lstKH;
            LoadCities();
        }

        void loadData()
        {
            var lstKH = controller.GetAllKH();
            dgvKH.DataSource = lstKH;
        }

        private void dgvKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng dữ liệu được click
                DataGridViewRow row = dgvKH.Rows[e.RowIndex];

                // Hiển thị thông tin vào các TextBox tương ứng
                txtMaKH.Text = row.Cells["KhachHangID"].Value.ToString();  // Mã KH
                txtTenKH.Text = row.Cells["TenKhachHang"].Value.ToString(); // Tên KH
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();       // Địa chỉ
                txtSDT.Text = row.Cells["DienThoai"].Value.ToString();       // SDT
            }
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
                    cboThanhPho.ValueMember = "code";  // Mã thành phố (dùng làm mã KH)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thành phố: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string tenKH = txtTenKH.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string thanhPho = cboThanhPho.Text;
            if (string.IsNullOrEmpty(tenKH) || string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(thanhPho))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }
            if (controller.AddKhachHang(txtMaKH.Text, tenKH, diaChi, sdt, cboThanhPho.Text))
            {
                MessageBox.Show("Thêm KH thành công!");
                loadData();
            }
            else
            {
                MessageBox.Show("Không thể thêm KH!");
            }
        }

        private void UpdateMaKH()
        {
            // Lấy mã thành phố từ ComboBox
            string maThanhPho = ((dynamic)cboThanhPho.SelectedItem)?.code; // Mã thành phố

            // Lấy 3 số cuối của số điện thoại
            string sdt = txtSDT.Text.Trim();
            string lastThreeDigits = sdt.Length >= 3 ? sdt.Substring(sdt.Length - 3) : "000"; // Lấy 3 số cuối

            // Nếu mã thành phố và số điện thoại hợp lệ thì tạo Mã KH
            if (!string.IsNullOrEmpty(maThanhPho) && !string.IsNullOrEmpty(lastThreeDigits))
            {
                string maKH = "KH" + maThanhPho + lastThreeDigits;
                txtMaKH.Text = maKH; // Hiển thị Mã KH lên textbox
            }
            else
            {
                txtMaKH.Text = ""; // Nếu không có đủ thông tin, để trống
            }
        }

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            UpdateMaKH();
        }

        private void cboThanhPho_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMaKH();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maKH = txtMaKH.Text.Trim();

            if (string.IsNullOrEmpty(maKH))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa!");
                return;
            }

            // Xóa khách hàng
            if (controller.DeleteKhachHang(maKH))
            {
                MessageBox.Show("Xóa KH thành công!");
                loadData();
            }
            else
            {
                MessageBox.Show("Không thể xóa KH!");
            }
        }

    
    }

}
