using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    class PhieuNhapController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        public List<PhieuNhap> GetAllPhieuNhap()
        {
            return qlkh.PhieuNhaps.ToList();
        }

        public List<dynamic> GetChiTietPhieuNhapInfo(string phieuNhapID)
        {
            var result = from ctpn in qlkh.ChiTietPhieuNhaps
                         join sp in qlkh.SanPhams on ctpn.SanPhamID equals sp.SanPhamID
                         join dm in qlkh.DanhMucs on sp.DanhMucID equals dm.DanhMucID
                         where ctpn.PhieuNhapID == phieuNhapID
                         select new
                         {
                             MaSanPham = sp.SanPhamID,
                             TenSanPham = sp.TenSanPham,
                             SoLuong = ctpn.SoLuong,
                             DanhMucID = dm.DanhMucID,
                             TenDanhMuc = dm.TenDanhMuc,
                         };

            return result.ToList<dynamic>();
        }

    }
}
