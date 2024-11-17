using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    public class KhachHangController
    {
        private QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        public List<KhachHang> GetAllKH()
        {
            return qlkh.KhachHangs.ToList();
        }

        public KhachHang GetKhachHangByID(string khachHangID)
        {
            return qlkh.KhachHangs.FirstOrDefault(kh => kh.KhachHangID == khachHangID);
        }

        public bool AddKhachHang(string khachHangID, string tenKH, string diaChi, string thanhPho, string sdt)
        {
            try
            {
                KhachHang newKhachHang = new KhachHang
                {
                    KhachHangID = khachHangID,
                    TenKH = tenKH,
                    DiaChi = diaChi,
                    ThanhPho = thanhPho,
                    SDT = sdt
                };

                qlkh.KhachHangs.InsertOnSubmit(newKhachHang); // Thêm khách hàng mới vào danh sách
                qlkh.SubmitChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }

        public bool DeleteKhachHang(string khachHangID)
        {
            try
            {
                var khToDelete = qlkh.KhachHangs.FirstOrDefault(kh => kh.KhachHangID == khachHangID);
                if (khToDelete == null)
                    return false; // Không tìm thấy khách hàng

                qlkh.KhachHangs.DeleteOnSubmit(khToDelete); // Xóa khách hàng
                qlkh.SubmitChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }

        public bool UpdateKhachHang(string khachHangID, string tenKH, string diaChi, string thanhPho, string sdt)
        {
            try
            {
                var khToUpdate = qlkh.KhachHangs.FirstOrDefault(kh => kh.KhachHangID == khachHangID);
                if (khToUpdate == null)
                    return false; // Không tìm thấy khách hàng

                // Cập nhật thông tin khách hàng
                khToUpdate.TenKH = tenKH;
                khToUpdate.DiaChi = diaChi;
                khToUpdate.ThanhPho = thanhPho;
                khToUpdate.SDT = sdt;

                qlkh.SubmitChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }
    }
}
