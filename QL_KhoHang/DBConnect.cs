using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang
{
    public  class DBConnect
    {
        // Tạo instance của QL_KhoHangDataContext
       private QL_KhoHangDataContext db = new QL_KhoHangDataContext();

        // Truy vấn lấy tất cả sản phẩm
        public List<SanPham> GetAllSanPham()
        {
            return db.SanPhams.ToList();
        }

        public void AddViTri(ViTriKho viTriKho)
        {
            db.ViTriKhos.InsertOnSubmit(viTriKho);
            db.SubmitChanges();
        }
        public void DeleteViTriByDanhMuc(string DanhMucID)
        {  
            var viTriList = db.ViTriKhos.Where(vt => vt.DanhMucID == DanhMucID).ToList();
            if (viTriList.Any())
            {
                db.ViTriKhos.DeleteAllOnSubmit(viTriList);
                db.SubmitChanges();
            }
        }
        public void AddSanPham(SanPham sanPham)
        {
            db.SanPhams.InsertOnSubmit(sanPham);
            db.SubmitChanges();
        }

   
        public void UpdateSanPham(SanPham sanPham)
        {
            var existingProduct = db.SanPhams.FirstOrDefault(sp => sp.SanPhamID == sanPham.SanPhamID);
            if (existingProduct != null)
            {
                existingProduct.TenSanPham = sanPham.TenSanPham;
                existingProduct.SoLuong = sanPham.SoLuong;
                existingProduct.GiaBan = sanPham.GiaBan;
                db.SubmitChanges();
            }
        }

        // Truy vấn xóa sản phẩm
        public void DeleteSanPham(string sanPhamID)
        {
            var product = db.SanPhams.FirstOrDefault(sp => sp.SanPhamID == sanPhamID);
            if (product != null)
            {
                db.SanPhams.DeleteOnSubmit(product);
                db.SubmitChanges();
            }
        }

        // Truy vấn lấy tất cả nhà cung cấp
        public List<NhaCungCap> GetAllNhaCungCap()
        {
            return db.NhaCungCaps.ToList();
        }

        // Truy vấn lấy tất cả phiếu nhập
        public List<PhieuNhap> GetAllPhieuNhap()
        {
            return db.PhieuNhaps.ToList();
        }

        // Truy vấn lấy tất cả phiếu xuất
        public List<PhieuXuat> GetAllPhieuXuat()
        {
            return db.PhieuXuats.ToList();
        }

        // Truy vấn lấy tất cả khách hàng
        public List<KhachHang> GetAllKhachHang()
        {
            return db.KhachHangs.ToList();
        }

        public List<DanhMuc> GetAllDanhMuc()
        {
            return db.DanhMucs.ToList();
        }

        public bool KTDanhMucTrong(string danhMucID)
        {
            var viTriCount = (from vt in db.ViTriKhos
                              where vt.DanhMucID == danhMucID
                              select vt).Count();

            return viTriCount > 0;
        }
        public List<ViTriKho> GetViTriByDanhMuc(string danhMucID)
        {
            return (from vt in db.ViTriKhos
                    where vt.DanhMucID == danhMucID
                    select vt).ToList();
        }
        public ViTriKho GetViTriByID(string ID)
        {
            return db.ViTriKhos.FirstOrDefault(vt=> vt.ViTriID == ID);
        }
        public List<SanPham> GetSanPhamByDanhMuc(string danhMucID)
        {
            return (from sp in db.SanPhams
                    where sp.DanhMucID == danhMucID
                    select sp).ToList();
        }

        public int GetTotalQuantityInBoxBySanPhamID(string sanPhamID)
        {
            
            var totalQuantity = db.ViTriKhos
                                  .Where(v => v.SanPhamID == sanPhamID)
                                  .Sum(v => v.SoLuong);
            return totalQuantity;
        }

        public int GetQuantityBySanPhamID(string sanPhamID)
        {
       
            var quantity = db.SanPhams
                             .Where(sp => sp.SanPhamID == sanPhamID)
                             .Select(sp => sp.SoLuong)
                             .FirstOrDefault();
            return quantity;
        }
    }

}
