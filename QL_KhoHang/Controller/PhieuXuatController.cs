using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace QL_KhoHang.Controller
{
    public class PhieuXuatController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        public List<PhieuXuat> GetAllPhieuXuat()
        {
            return qlkh.PhieuXuats.ToList();
        }

        public List<dynamic> GetChiTietPhieuXuatInfo(string phieuXuatID)
        {
            var result = from ctx in qlkh.ChiTietPhieuXuats
                         join sp in qlkh.SanPhams on ctx.SanPhamID equals sp.SanPhamID
                         join dm in qlkh.DanhMucs on sp.DanhMucID equals dm.DanhMucID
                         where ctx.PhieuXuatID == phieuXuatID
                         select new
                         {
                             MaSanPham = sp.SanPhamID,
                             TenSanPham = sp.TenSanPham,
                             SoLuong = ctx.SoLuong,
                             DanhMucID = dm.DanhMucID,
                             TenDanhMuc = dm.TenDanhMuc,
                         };

            return result.ToList<dynamic>();
        }

        public string GenerateMaPhieuXuat(string maKH, string NgayThangNam)
        {


            string baseMaPX = "PX" + NgayThangNam + maKH;


            var similarPhieuXuats = qlkh.PhieuXuats
                .Where(px => px.PhieuXuatID.StartsWith(baseMaPX))
                .Select(px => px.PhieuXuatID)
                .ToList();

            // Xác định số thứ tự tiếp theo
            int nextNumber = 1;
            if (similarPhieuXuats.Any())
            {
                // Lấy số thứ tự lớn nhất từ các mã tương tự và tăng lên 1
                var lastNumber = similarPhieuXuats
                    .Select(px => int.Parse(px.Substring(baseMaPX.Length)))
                    .OrderByDescending(n => n)
                    .FirstOrDefault();
                nextNumber = lastNumber + 1;
            }

            // Tạo mã phiếu xuất với số thứ tự được format thành 2 chữ số (ví dụ: "01", "02")
            string maPhieuXuat = String.Format("{0}{1:D2}", baseMaPX, nextNumber);


            return maPhieuXuat;
        }
        public bool ThemPhieuXuat(string maNV, string maKH, DateTime ngayXuat, string maPX, List<dynamic> chiTietPhieuXuats, String ghiChu)
        {

            // Tạo đối tượng PhieuXuat mới
            PhieuXuat phieuXuat = new PhieuXuat
            {
                PhieuXuatID = maPX,
                KhachHangID = maKH,
                NhanVienID = maNV, // Thay "NV01" bằng ID nhân viên hiện tại nếu có
                NgayXuat = ngayXuat,
                GhiChu = ghiChu,        // Ghi chú nếu có

            };

            // Thêm phiếu xuất vào cơ sở dữ liệu
            qlkh.PhieuXuats.InsertOnSubmit(phieuXuat);

            // Thêm chi tiết phiếu xuất
            foreach (var chiTiet in chiTietPhieuXuats)
            {
                ChiTietPhieuXuat ctPhieuXuat = new ChiTietPhieuXuat
                {
                    PhieuXuatID = maPX,
                    SanPhamID = chiTiet.MaSanPham,
                    SoLuong = chiTiet.SoLuong,
                    GiaXuat = (double?)chiTiet.GiaXuat
                };

                qlkh.ChiTietPhieuXuats.InsertOnSubmit(ctPhieuXuat);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            qlkh.SubmitChanges();
            return true;


        }
        public List<dynamic> GetThongKeSanPhamXuat()
        {
            // Lấy tháng hiện tại
            DateTime now = DateTime.Now;
            int currentMonth = now.Month;
            int currentYear = now.Year;

            // Danh sách để chứa kết quả
            var thongKeList = new List<dynamic>();

            // Lấy danh sách sản phẩm
            var sanPhams = qlkh.SanPhams.ToList();

            // Duyệt qua từng sản phẩm
            foreach (var sp in sanPhams)
            {
                // Tạo một đối tượng thống kê
                var thongKe = new
                {
                    SanPhamID = sp.SanPhamID,
                    TenSanPham = sp.TenSanPham,
                    SoLuongThang1 = GetTongSoLuong(sp.SanPhamID, currentMonth, currentYear),
                    SoLuongThang2 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 1 + 12) % 12, currentMonth == 1 ? currentYear - 1 : currentYear),
                    SoLuongThang3 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 2 + 12) % 12, currentMonth <= 2 ? currentYear - 1 : currentYear),
                    SoLuongThang4 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 3 + 12) % 12, currentMonth <= 3 ? currentYear - 1 : currentYear),
                    SoLuongThang5 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 4 + 12) % 12, currentMonth <= 4 ? currentYear - 1 : currentYear),
                    SoLuongThang6 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 5 + 12) % 12, currentMonth <= 5 ? currentYear - 1 : currentYear),
                    SoLuongThang7 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 6 + 12) % 12, currentMonth <= 6 ? currentYear - 1 : currentYear),
                    SoLuongThang8 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 7 + 12) % 12, currentMonth <= 7 ? currentYear - 1 : currentYear),
                    SoLuongThang9 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 8 + 12) % 12, currentMonth <= 8 ? currentYear - 1 : currentYear),
                    SoLuongThang10 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 9 + 12) % 12, currentMonth <= 9 ? currentYear - 1 : currentYear),
                    SoLuongThang11 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 10 + 12) % 12, currentMonth <= 10 ? currentYear - 1 : currentYear),
                    SoLuongThang12 = GetTongSoLuong(sp.SanPhamID, (currentMonth - 11 + 12) % 12, currentMonth <= 11 ? currentYear - 1 : currentYear)
                };

                thongKeList.Add(thongKe);
            }

            return thongKeList;
        }

        // Hàm phụ để tính tổng số lượng sản phẩm xuất ra trong một tháng và năm nhất định
        private int GetTongSoLuong(string sanPhamID, int month, int year)
        {
            var tongSoLuong = qlkh.ChiTietPhieuXuats
                .Where(ctx => ctx.SanPhamID == sanPhamID &&
                            ctx.PhieuXuat.NgayXuat.Month == month &&
                            ctx.PhieuXuat.NgayXuat.Year == year)
                .Sum(ctx => (int?)ctx.SoLuong) ?? 0;

            return tongSoLuong;
        }



    }
}
