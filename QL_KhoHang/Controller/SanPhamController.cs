using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    public class SanPhamController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();
        public List<SanPham> GetSanPhamByDanhMuc(string danhMucID)
        {
            return (from sp in qlkh.SanPhams
                    where sp.DanhMucID == danhMucID
                    select sp).ToList();
        }
        public SanPham GetSanPhamByID(string sanPhamID)
        {
            var sanPham = qlkh.SanPhams
                              .FirstOrDefault(s => s.SanPhamID == sanPhamID);

            return sanPham; 
        }
        public int GetTotalQuantityInBoxBySanPhamID(string sanPhamID)
        {
            if (!IsSanPhamIDExists(sanPhamID)) return 0;
            var totalQuantity = qlkh.ViTriKhos
                             .Where(v => v.SanPhamID != null && v.SanPhamID == sanPhamID)
                             .Sum(v => (v.SoLuong != null ? v.SoLuong : 0)); // Kiểm tra null và thay thế bằng 0

            return totalQuantity;
        }
        public bool IsSanPhamIDExists(string sanPhamID)
        {
            return qlkh.ViTriKhos.Any(v => v.SanPhamID == sanPhamID);
        }

        public int GetQuantityBySanPhamID(string sanPhamID)
        {
       
            var quantity = qlkh.SanPhams
                             .Where(sp => sp.SanPhamID == sanPhamID)
                             .Select(sp => sp.SoLuong)
                             .FirstOrDefault();
            return quantity;
        }
        public List<SanPham> SearchSanPhamByDanhMucID(string searchText, string danhMucID)
        {
            return qlkh.SanPhams
                     .Where(sp => sp.TenSanPham.Contains(searchText) && sp.DanhMucID == danhMucID)
                     .ToList();
        }

        public List<SanPham> ListSanPhamByPhieuNhapID( string phieuNhapID)
        {
             var sanPhams = (from ctpn in qlkh.ChiTietPhieuNhaps
                    join sp in qlkh.SanPhams on ctpn.SanPhamID equals sp.SanPhamID
                    where ctpn.PhieuNhapID == phieuNhapID
                    select sp).ToList();

             return sanPhams;
        }
    }
}
