using System;
using System.Collections.Generic;
using System.Linq;


namespace QL_KhoHang.Controller
{
    public class ThongKeController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();
        public List<int> GetTongSoLuongXuatTheoThang()
        {
            DateTime now = DateTime.Now;
            int currentYear = now.Year;

            var tongSoLuongThang = new List<int>();

            // Duyệt qua 12 tháng trong năm hiện tại
            for (int month = 1; month <= 12; month++)
            {
                int totalQuantity = qlkh.ChiTietPhieuXuats
                    .Where(ctx => ctx.PhieuXuat.NgayXuat.Month == month && ctx.PhieuXuat.NgayXuat.Year == currentYear)
                    .Sum(ctx => (int?)ctx.SoLuong) ?? 0;

                tongSoLuongThang.Add(totalQuantity);
            }

            return tongSoLuongThang;
        }

        public List<int> GetTongSoLuongNhapTheoThang()
        {
            DateTime now = DateTime.Now;
            int currentYear = now.Year;

            var tongSoLuongThang = new List<int>();

            // Duyệt qua 12 tháng trong năm hiện tại
            for (int month = 1; month <= 12; month++)
            {
                int totalQuantity = qlkh.ChiTietPhieuNhaps
                    .Where(ctx => ctx.PhieuNhap.NgayNhap.Month == month && ctx.PhieuNhap.NgayNhap.Year == currentYear)
                    .Sum(ctx => (int?)ctx.SoLuong) ?? 0;

                tongSoLuongThang.Add(totalQuantity);
            }

            return tongSoLuongThang;
        }
        public int GetTongHangHoaTrongKho()
        {
            return qlkh.SanPhams.Sum(sp => (int?)sp.SoLuong) ?? 0;
        }

        // Lấy tổng số lượng sản phẩm nhập trong tháng này
        public int GetHangNhapThangNay()
        {
            DateTime now = DateTime.Now;
            int currentMonth = now.Month;
            int currentYear = now.Year;

            return qlkh.ChiTietPhieuNhaps
                .Where(ctx => ctx.PhieuNhap.NgayNhap.Month == currentMonth && ctx.PhieuNhap.NgayNhap.Year == currentYear)
                .Sum(ctx => (int?)ctx.SoLuong) ?? 0;
        }

        // Lấy tổng số lượng sản phẩm xuất trong tháng này
        public int GetHangXuatThangNay()
        {
            DateTime now = DateTime.Now;
            int currentMonth = now.Month;
            int currentYear = now.Year;

            return qlkh.ChiTietPhieuXuats
                .Where(ctx => ctx.PhieuXuat.NgayXuat.Month == currentMonth && ctx.PhieuXuat.NgayXuat.Year == currentYear)
                .Sum(ctx => (int?)ctx.SoLuong) ?? 0;
        }

        // Lấy danh sách sản phẩm sắp hết hàng
        public List<SanPham> GetHangSapHet()
        {
           
            return qlkh.SanPhams
                .Where(sp => sp.SoLuong < sp.SanPhamToiThieu)
                .ToList();
        }

        // Trả về các thông tin cần hiển thị trên các label
        public (int tongHangHoa, int hangNhapThangNay, int hangXuatThangNay, List<SanPham> hangSapHet) GetThongKeThongTin()
        {
            int tongHangHoa = GetTongHangHoaTrongKho();
            int hangNhapThangNay = GetHangNhapThangNay();
            int hangXuatThangNay = GetHangXuatThangNay();
            List<SanPham> hangSapHet = GetHangSapHet();

            return (tongHangHoa, hangNhapThangNay, hangXuatThangNay, hangSapHet);
        }
        public int CountHangSapHet()
        {
           
            return qlkh.SanPhams
                .Count(sp => sp.SoLuong < sp.SanPhamToiThieu);
        }
    }
}
