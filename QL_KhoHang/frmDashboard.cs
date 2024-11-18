using BunifuAnimatorNS;
using QL_KhoHang.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QL_KhoHang
{
    public partial class frmDashboard : Form
    {
        PhieuXuatController phieuXuatController = new PhieuXuatController();
        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            LoadThongKeData();
            loadKhoHang();
            loadViTri();
            loadNhanVien();

        }
        
        ThongKeController controller = new ThongKeController();
        private async void LoadThongKeData()
        {
         
            var tongSoLuongXuatTheoThang = controller.GetTongSoLuongXuatTheoThang();
            var tongSoLuongNhapTheoThang = controller.GetTongSoLuongNhapTheoThang();

            // Xóa các series cũ trên biểu đồ
            chartThongKe.Series.Clear();
            chartThongKe.ChartAreas[0].AxisX.Interval = 1; // Đảm bảo mỗi cột là một tháng

            // Thêm series cho sản phẩm xuất
            var seriesXuat = new Series("Sản phẩm xuất")
            {
                ChartType = SeriesChartType.Column,  // Biểu đồ cột
                BorderWidth = 3,  // Độ dày đường viền
                IsValueShownAsLabel = true  // Hiển thị giá trị trên mỗi cột
            };

            // Thêm series cho sản phẩm nhập
            var seriesNhap = new Series("Sản phẩm nhập")
            {
                ChartType = SeriesChartType.Column,  // Biểu đồ cột
                BorderWidth = 3,  // Độ dày đường viền
                IsValueShownAsLabel = true  // Hiển thị giá trị trên mỗi cột
            };

            // Thêm dữ liệu vào series sản phẩm xuất
            for (int i = 0; i < tongSoLuongXuatTheoThang.Count; i++)
            {
                seriesXuat.Points.AddXY((i + 1).ToString(), tongSoLuongXuatTheoThang[i]);  // X là tháng, Y là tổng số lượng
            }

            // Thêm dữ liệu vào series sản phẩm nhập
            for (int i = 0; i < tongSoLuongNhapTheoThang.Count; i++)
            {
                seriesNhap.Points.AddXY((i + 1).ToString(), tongSoLuongNhapTheoThang[i]);  // X là tháng, Y là tổng số lượng
            }

            // Thêm các series vào biểu đồ
           
            chartThongKe.Series.Add(seriesNhap);
            chartThongKe.Series.Add(seriesXuat);
        }

        void loadKhoHang()
        {
            lblTonKho.Text = controller.GetTongHangHoaTrongKho().ToString();
            lblHangXuat.Text = controller.GetHangXuatThangNay().ToString();
            lblHangNhap.Text = controller.GetHangNhapThangNay().ToString();
            lblSapHet.Text = controller.CountHangSapHet().ToString();
        }

        void loadViTri()
        {
            ViTriKhoController VTcontroller = new ViTriKhoController();
            var danhSachViTri = VTcontroller.GetDanhSachViTri();

            dgvViTri.DataSource = danhSachViTri; // Gán dữ liệu vào DataGri
        }

        void loadNhanVien()
        {
            NhanVienController nhanVienController = new NhanVienController();
            var lstNV = nhanVienController.GetAllNhanVien();
            dgvNhanVien.DataSource = lstNV; 

        }

        private void btn_nv_them_Click(object sender, EventArgs e)
        {
            NhanVienController nhanVienController = new NhanVienController();
            // Lấy thông tin từ các trường nhập liệu
            string hoTen = txt_hoten.Text.Trim();
            string soDienThoai = txt_sdt.Text.Trim();

            // Kiểm tra các trường có hợp lệ không
            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(soDienThoai))
            {
                MessageBox.Show("Họ tên và số điện thoại không được để trống.");
                return;
            }

            // Xác định vai trò
            string vaiTro = rda_nhapkho.Checked ? "NHAPKHO" : "XUATKHO";

            // Tạo mã nhân viên tự động
            int soNhanVien = nhanVienController.GetSoLuongNhanVien(); // Lấy số lượng nhân viên
            string maNhanVien = "NV" + (soNhanVien + 1).ToString("D2"); // Định dạng thành 2 chữ số

            // Mật khẩu mặc định
            string matKhau = "123456";

            // Tạo đối tượng nhân viên
            NhanVien newNhanVien = new NhanVien
            {
                NhanVienID = maNhanVien,
                MatKhau = matKhau,
                HoTen = hoTen,
                DienThoai = soDienThoai,
                VaiTroID = vaiTro
            };

            // Gọi phương thức thêm nhân viên vào cơ sở dữ liệu
            bool isSuccess = nhanVienController.ThemNhanVien(newNhanVien);

            if (isSuccess)
            {
                MessageBox.Show("Thêm nhân viên thành công.");
                loadNhanVien(); // Cập nhật danh sách nhân viên
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi thêm nhân viên.");
            }
        }

        private void btn_nv_sua_Click(object sender, EventArgs e)
        {
            NhanVienController nhanVienController = new NhanVienController();
            // Lấy mã nhân viên từ TextBox
            string maNhanVien = txt_manv.Text.Trim();

            // Kiểm tra xem mã nhân viên có hợp lệ không
            if (string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên.");
                return;
            }

            // Lấy thông tin từ các trường nhập liệu
            string hoTen = txt_hoten.Text.Trim();
            string soDienThoai = txt_sdt.Text.Trim();

            // Kiểm tra các trường có hợp lệ không
            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(soDienThoai))
            {
                MessageBox.Show("Họ tên và số điện thoại không được để trống.");
                return;
            }

            // Xác định vai trò
            string vaiTro = rda_nhapkho.Checked ? "NHAPKHO" : "XUATKHO";

            // Tạo đối tượng nhân viên để sửa
            NhanVien updatedNhanVien = new NhanVien
            {
                NhanVienID = maNhanVien,
                HoTen = hoTen,
                DienThoai = soDienThoai,
                VaiTroID = vaiTro
            };

            // Gọi phương thức sửa nhân viên vào cơ sở dữ liệu
            bool isSuccess = nhanVienController.SuaNhanVien(updatedNhanVien);

            if (isSuccess)
            {
                MessageBox.Show("Sửa thông tin nhân viên thành công.");
                loadNhanVien(); // Cập nhật danh sách nhân viên
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi sửa thông tin nhân viên. Vui lòng kiểm tra mã nhân viên.");
            }
        }

        private void btn_nv_xoa_Click(object sender, EventArgs e)
        {
            NhanVienController nhanVienController = new NhanVienController();
            // Lấy mã nhân viên từ TextBox
            string maNhanVien = txt_manv.Text.Trim();

            // Kiểm tra xem mã nhân viên có hợp lệ không
            if (string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên cần xóa.");
                return;
            }

            // Xác nhận trước khi xóa
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này không?",
                                                 "Xác nhận xóa",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
            if (confirmResult == DialogResult.No)
            {
                return; // Nếu người dùng chọn 'No', không thực hiện xóa
            }

            // Gọi phương thức xóa nhân viên từ cơ sở dữ liệu
            bool isSuccess = nhanVienController.XoaNhanVien(maNhanVien);

            if (isSuccess)
            {
                MessageBox.Show("Xóa nhân viên thành công.");
                loadNhanVien(); // Cập nhật danh sách nhân viên
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa nhân viên. Vui lòng kiểm tra mã nhân viên.");
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có nhấp vào một hàng hợp lệ không
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ hàng đã chọn
                DataGridViewRow selectedRow = dgvNhanVien.Rows[e.RowIndex];

                // Gán giá trị vào các TextBox
                txt_manv.Text = selectedRow.Cells[0].Value.ToString(); // Mã nhân viên
                txt_hoten.Text = selectedRow.Cells[2].Value.ToString(); // Họ tên
                txt_sdt.Text = selectedRow.Cells[3].Value.ToString(); // Số điện thoại

                // Xác định vai trò và gán vào RadioButton
                string vaiTro = selectedRow.Cells[4].Value.ToString(); // Vai trò
                if (vaiTro == "NHAPKHO")
                {
                    rda_nhapkho.Checked = true;
                    rda_xuatkho.Checked = false;
                }
                else if (vaiTro == "XUATKHO")
                {
                    rda_nhapkho.Checked = false;
                    rda_xuatkho.Checked = true;
                }
            }
        }
    }
}
