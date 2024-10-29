using System;

namespace QL_KhoHang
{
    class ChiTietPN : ChiTietPhieuNhap
    {
        public int STT { get; set; }
        public string MASANPHAM { get; set; }
        public string TenSP { get; set; }
        public int SOLUONG { get; set; }
        public double DONGIA { get; set; }
        public double THANHTIEN { get; set; }

        public ChiTietPN(ChiTietPhieuNhap ct, int stt)
        {
            this.STT = stt;
            this.MASANPHAM = ct.SanPhamID;
            this.TenSP = ct.SanPham.TenSanPham; // Giả định rằng ChiTietPhieuNhap cũng có thuộc tính SANPHAM
            this.SOLUONG = (int)ct.SoLuong;
            this.DONGIA = (double)ct.GiaNhap;
            this.THANHTIEN = (double)ct.TongTien;
        }
    }
}