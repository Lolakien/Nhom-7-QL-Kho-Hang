using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    class LichSuXuatKhoController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();
        public bool ThemLichSuXuatKho(string MaPX, List<dynamic> LichSuViTri)
        {
            foreach (var vt in LichSuViTri)
            {
                LichSuXuatKho lsxk = new LichSuXuatKho
                {
                    PhieuXuatID = MaPX,
                    DanhMucID = vt.DanhMucID,
                    SanPhamID = vt.SanPhamID,
                    SoLuong = vt.SoLuong,
                    ViTriID = vt.ViTriID  
                };

                qlkh.LichSuXuatKhos.InsertOnSubmit(lsxk);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            qlkh.SubmitChanges();
            return true;


        }
    }
}
