﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    class ViTriKhoController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();
        public List<dynamic> GetDanhSachViTri()
        {
            var danhSachViTri = qlkh.ViTriKhos
                .Select(vt => new
                {
                    MaViTri = vt.ViTriID ?? null,
                    MaDanhMuc = vt.SanPham.DanhMucID ?? null, // Lấy mã danh mục từ sản phẩm liên kết
                    MaSanPham = vt.SanPhamID ?? null,
                    TenSanPham = vt.SanPham.TenSanPham ?? null,
                    SoLuong = vt.SoLuong ,
                    SoLuongToiDa = vt.SoLuongToiDa 
                })
           .OrderByDescending(vt => vt.SoLuong)
                .ToList<dynamic>(); // Chuyển danh sách thành List<dynamic> để hiển thị trên DataGridView

            return danhSachViTri;
        }
        public void AddViTri(ViTriKho viTriKho)
        {
            qlkh.ViTriKhos.InsertOnSubmit(viTriKho);
            qlkh.SubmitChanges();

        }

        public void DeleteViTriByDanhMuc(string DanhMucID)
        {
            var viTriList = qlkh.ViTriKhos.Where(vt => vt.DanhMucID == DanhMucID).ToList();
            if (viTriList.Any())
            {
                qlkh.ViTriKhos.DeleteAllOnSubmit(viTriList);
                qlkh.SubmitChanges();
            }
        }
        public List<ViTriKho> GetListViTriByDanhMuc(string danhMucID)
        {
            return (from vt in qlkh.ViTriKhos
                    where vt.DanhMucID == danhMucID
                    select vt).ToList();
        }


        public ViTriKho GetViTriKhoByDanhMucIDAndViTriID(string danhMucID, string viTriID)
        {
            return qlkh.ViTriKhos.FirstOrDefault(vt => vt.DanhMucID == danhMucID && vt.ViTriID == viTriID);
        }
        public bool UpdateViTriKho(String ViTriID, ViTriKho newViTriKho)
        {
            // Tìm vị trí kho cần cập nhật
            try
            {
                var oldViTriKho = qlkh.ViTriKhos.FirstOrDefault(vt => vt.ViTriID == ViTriID);
                if (oldViTriKho != null)
                {
                    oldViTriKho.SoLuong = newViTriKho.SoLuong;
                    oldViTriKho.SoLuongToiDa = newViTriKho.SoLuongToiDa;
                    oldViTriKho.SanPhamID = newViTriKho.SanPhamID;

                    qlkh.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
            return true;
        }
        public int DemViTriKhoDaXep(String DanhMucID)
        {
            int count = qlkh.ViTriKhos
                .Where(v => v.DanhMucID == DanhMucID && v.SoLuong > 0)
                .Count();
            return count;
        }

        public int DemTongViTriKho(String DanhMucID)
        {
            int count = qlkh.ViTriKhos
                .Where(v => v.DanhMucID == DanhMucID)
                .Count();
            return count;
        }

    }
    

    
}
