using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QL_KhoHang
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
        
       

            LoadChart();
            loadProfitChart();
            lblDate.Text = DateTime.Now.ToShortDateString();
        }

      
        private void LoadChart()
        {
            QL_KhoHangDataContext QLKho = new QL_KhoHangDataContext();
            DateTime startDate = DateTime.Now.AddYears(-1);



            // Truy vấn tổng tiền nhập từ bảng PhieuNhap
            var nhapTien = QLKho.PhieuNhaps
                .Where(pn => pn.NgayNhap >= startDate)
                .GroupBy(pn => pn.NgayNhap.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    TongTienNhap = g.Sum(pn => (decimal?)pn.TongTien) ?? 0 
                }).ToList();

            // Truy vấn tổng tiền xuất từ bảng PhieuXuat
            var xuatTien = QLKho.PhieuXuats
                .Where(px => px.NgayXuat >= startDate)
                .GroupBy(px => px.NgayXuat.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    TongTienXuat = g.Sum(px => (decimal?)px.TongTien) ?? 0 // Sử dụng nullable decimal
                }).ToList();

            // Tạo hai series cho biểu đồ cột
            var seriesNhap = new Series("Nhập kho");
            seriesNhap.ChartType = SeriesChartType.Column;

            var seriesXuat = new Series("Xuất kho");
            seriesXuat.ChartType = SeriesChartType.Column;

            // Đổ dữ liệu nhập vào series Nhập kho
            foreach (var nhap in nhapTien)
            {
              seriesNhap.Points.AddXY("Tháng " + nhap.Thang, nhap.TongTienNhap);
            }

            // Đổ dữ liệu xuất vào series Xuất kho
            foreach (var xuat in xuatTien)
            {
                seriesXuat.Points.AddXY("Tháng " + xuat.Thang, xuat.TongTienXuat);
            }

            
            chart1.Series.Clear(); 
            chart1.Series.Add(seriesNhap);
            chart1.Series.Add(seriesXuat);

          
            chart1.ChartAreas[0].AxisX.Title = "Tháng";
            chart1.ChartAreas[0].AxisY.Title = "Tổng Tiền";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0 VND";

         
            lblNhap.Text =   nhapTien.Where(n => n.Thang == DateTime.Now.Month).Sum(n => n.TongTienNhap).ToString("#,##0 VND");
            lblXuat.Text =   xuatTien.Where(x => x.Thang == DateTime.Now.Month).Sum(x => x.TongTienXuat).ToString("#,##0 VND");
        }
        private void loadProfitChart()
        {
            QL_KhoHangDataContext QLKho = new QL_KhoHangDataContext();
            DateTime startDate = new DateTime(DateTime.Now.Year, 1, 1); // Bắt đầu từ đầu năm
            DateTime endDate = DateTime.Now; // Đến thời điểm hiện tại

            // Truy vấn tổng tiền nhập từ bảng PhieuNhap theo tháng
            var nhapTien = QLKho.PhieuNhaps
                .Where(pn => pn.NgayNhap >= startDate && pn.NgayNhap <= endDate)
                .GroupBy(pn => pn.NgayNhap.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    TongTienNhap = g.Sum(pn => (decimal?)pn.TongTien) ?? 0
                }).ToList();

            // Truy vấn tổng tiền xuất từ bảng PhieuXuat theo tháng
            var xuatTien = QLKho.PhieuXuats
                .Where(px => px.NgayXuat >= startDate && px.NgayXuat <= endDate)
                .GroupBy(px => px.NgayXuat.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    TongTienXuat = g.Sum(px => (decimal?)px.TongTien) ?? 0
                }).ToList();

            // Tạo series cho biểu đồ lợi nhuận
            var profitSeries = new Series("Lợi nhuận")
            {
                ChartType = SeriesChartType.Line
            };

            // Tính lợi nhuận theo từng tháng
            for (int month = 1; month <= 12; month++)
            {
                var nhap = nhapTien.FirstOrDefault(n => n.Thang == month);
                var xuat = xuatTien.FirstOrDefault(x => x.Thang == month);

                decimal tongTienNhap = nhap != null ? nhap.TongTienNhap : 0;
                decimal tongTienXuat = xuat != null ? xuat.TongTienXuat : 0;

                decimal profit = tongTienXuat - tongTienNhap;

                profitSeries.Points.AddXY("Tháng " + month, profit);
            }

            // Thêm series lợi nhuận vào chart
            chartProfit.Series.Clear();
            chartProfit.Series.Add(profitSeries);

            // Cập nhật label lợi nhuận tháng hiện tại
            var nhapCurrentMonth = nhapTien.FirstOrDefault(n => n.Thang == DateTime.Now.Month);
            var xuatCurrentMonth = xuatTien.FirstOrDefault(x => x.Thang == DateTime.Now.Month);

            var currentMonthNhap = nhapCurrentMonth != null ? nhapCurrentMonth.TongTienNhap : 0;
            var currentMonthXuat = xuatCurrentMonth != null ? xuatCurrentMonth.TongTienXuat : 0;

            var currentMonthProfit = currentMonthXuat - currentMonthNhap;

            lblProfit.Text = currentMonthProfit.ToString("#,##0 VND");

            // Đặt tiêu đề cho các trục của biểu đồ
            chartProfit.ChartAreas[0].AxisX.Title = "Tháng";
            chartProfit.ChartAreas[0].AxisY.Title = "Lợi nhuận";
            chartProfit.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0 VND";
        }





    }
}
