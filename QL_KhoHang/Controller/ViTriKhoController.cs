using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    class ViTriKhoController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();
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
        public void UpdateViTriKho(String ViTriID,ViTriKho newViTriKho)
        {
            // Tìm vị trí kho cần cập nhật
            var oldViTriKho = qlkh.ViTriKhos.FirstOrDefault(vt => vt.ViTriID == ViTriID);
            if (oldViTriKho != null)
            {
                oldViTriKho.SoLuong = newViTriKho.SoLuong;
                oldViTriKho.SoLuongToiDa = newViTriKho.SoLuongToiDa;
                oldViTriKho.SanPhamID = newViTriKho.SanPhamID;

                qlkh.SubmitChanges();
            }
        }
    }
}
