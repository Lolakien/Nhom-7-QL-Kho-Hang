using System;
using System.Collections.Generic;
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
    }
}
