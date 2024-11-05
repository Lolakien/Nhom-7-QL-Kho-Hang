using System;

namespace QL_KhoHang
{
    class ChiTietPX : ChiTietPhieuXuat
    {
        public int STT { get; set; }
        public string MASANPHAM { get; set; }
        public string TenSP { get; set; }
        public int SOLUONG { get; set; }
        public double DONGIA { get; set; }
        public double THANHTIEN { get; set; }

        public ChiTietPX(ChiTietPhieuXuat ct, int stt)
        {
            this.STT = stt;
            this.MASANPHAM = ct.SanPhamID;
            this.TenSP = ct.SanPham.TenSanPham;
            this.SOLUONG = (int)ct.SoLuong;
            this.DONGIA = (double)ct.GiaXuat;
            this.THANHTIEN = (double)ct.TongTien;
        }
    }
}