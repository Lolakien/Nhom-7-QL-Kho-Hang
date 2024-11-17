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

    }
}
