using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    public class NhanVienController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();
        public List<NhanVien> GetAllNhanVien()
        {
            return qlkh.NhanViens.ToList();
        }
        public bool ThemNhanVien(NhanVien nhanVien)
        {
            try
            {
                using (var context = new QL_KhoHangDataContext())
                {
                    // Tạo đối tượng nhân viên mới
                    var newNhanVien = new NhanVien
                    {
                        NhanVienID = nhanVien.NhanVienID,
                        MatKhau = nhanVien.MatKhau,
                        HoTen = nhanVien.HoTen,
                        DienThoai = nhanVien.DienThoai,
                        VaiTroID = nhanVien.VaiTroID
                    };

                    // Thêm nhân viên vào bảng NhanVien
                    context.NhanViens.InsertOnSubmit(newNhanVien);
                    context.SubmitChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (có thể ghi log hoặc thông báo)
                return false;
            }
        }

        public int GetSoLuongNhanVien()
        {
            using (var context = new QL_KhoHangDataContext())
            {
                // Trả về số lượng nhân viên trong bảng NhanVien
                return context.NhanViens.Count();
            }
        }

        public bool SuaNhanVien(NhanVien nhanVien)
        {
            try
            {
                using (var context = new QL_KhoHangDataContext())
                {
                    // Tìm nhân viên theo ID
                    var existingNhanVien = context.NhanViens.SingleOrDefault(nv => nv.NhanVienID == nhanVien.NhanVienID);
                    if (existingNhanVien != null)
                    {
                        // Cập nhật thông tin
                        existingNhanVien.HoTen = nhanVien.HoTen;
                        existingNhanVien.DienThoai = nhanVien.DienThoai;
                        existingNhanVien.VaiTroID = nhanVien.VaiTroID;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        context.SubmitChanges();
                        return true;
                    }
                    return false; // Nhân viên không tồn tại
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (có thể ghi log hoặc thông báo)
                return false;
            }
        }

        public bool XoaNhanVien(string maNhanVien)
        {
            try
            {
                using (var context = new QL_KhoHangDataContext())
                {
                    // Tìm nhân viên theo ID
                    var existingNhanVien = context.NhanViens.SingleOrDefault(nv => nv.NhanVienID == maNhanVien);
                    if (existingNhanVien != null)
                    {
                        // Xóa nhân viên
                        context.NhanViens.DeleteOnSubmit(existingNhanVien);
                        context.SubmitChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                        return true;
                    }
                    return false; // Nhân viên không tồn tại
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (có thể ghi log hoặc thông báo)
                return false;
            }
        }

    }
}
