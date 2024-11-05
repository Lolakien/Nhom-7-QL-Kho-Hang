using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
    class KhachHangController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();
        public KhachHang GetKhachHangByID(string khachHangID)
        {
            var kh = qlkh.KhachHangs.FirstOrDefault(s => s.KhachHangID == khachHangID);

            return kh;
        }
    }
}
