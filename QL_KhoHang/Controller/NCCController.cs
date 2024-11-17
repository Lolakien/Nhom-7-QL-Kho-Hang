using System;
using System.Collections.Generic;
using System.Linq;

namespace QL_KhoHang.Controller
{
    public class NCCController
    {
        // Tạo instance của QL_KhoHangDataContext để tương tác với cơ sở dữ liệu
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        // Lấy danh sách tất cả nhà cung cấp
        public List<NhaCungCap> GetAllNCC()
        {
            return qlkh.NhaCungCaps.ToList();
        }

        // Thêm nhà cung cấp mới
        public bool AddNCC(string maNCC, string tenNCC, string diaChi, string sdt, string thanhPho)
        {
            try
            {
                // Tạo đối tượng NCC mới
                NhaCungCap newNCC = new NhaCungCap
                {
                    NhaCungCapID = maNCC,
                    TenNhaCungCap = tenNCC,
                    DiaChi = diaChi,
                    DienThoai = sdt,
                    ThanhPho = thanhPho // Lưu tên thành phố vào cơ sở dữ liệu
                };

                // Thêm vào cơ sở dữ liệu
                qlkh.NhaCungCaps.InsertOnSubmit(newNCC);
                qlkh.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi

                return false;
            }
        }


        // Xóa nhà cung cấp theo ID
        public bool DeleteNCC(string nhaCungCapID)
        {
            try
            {
                // Tìm nhà cung cấp theo ID
                var nccToDelete = qlkh.NhaCungCaps.FirstOrDefault(ncc => ncc.NhaCungCapID == nhaCungCapID);
                if (nccToDelete == null)
                    return false; // Không tìm thấy nhà cung cấp

                qlkh.NhaCungCaps.DeleteOnSubmit(nccToDelete); // Xóa nhà cung cấp
                qlkh.SubmitChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ghi log hoặc hiển thị thông báo)
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }

        // Sửa thông tin nhà cung cấp
        public bool UpdateNCC(NhaCungCap updatedNCC)
        {
            try
            {
                // Tìm nhà cung cấp cần sửa theo ID
                var nccToUpdate = qlkh.NhaCungCaps.FirstOrDefault(ncc => ncc.NhaCungCapID == updatedNCC.NhaCungCapID);
                if (nccToUpdate == null)
                    return false; // Không tìm thấy nhà cung cấp

                // Cập nhật thông tin nhà cung cấp
                nccToUpdate.TenNhaCungCap = updatedNCC.TenNhaCungCap;
                nccToUpdate.DiaChi = updatedNCC.DiaChi;
                nccToUpdate.ThanhPho = updatedNCC.ThanhPho;
                nccToUpdate.DienThoai = updatedNCC.DienThoai;

                qlkh.SubmitChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ghi log hoặc hiển thị thông báo)
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }
    }
}
