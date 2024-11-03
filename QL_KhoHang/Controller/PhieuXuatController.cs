using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    class PhieuXuatController
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
    }
    
}
