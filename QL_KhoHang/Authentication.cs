using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang
{
    public static class Authentication
    {
       
        public static string ID ="NULL";
       
        public static bool ValidateUser(string ID, string password)
        {
            QL_KhoHangDataContext QLKho = new QL_KhoHangDataContext();
            var user = (from nv in QLKho.NhanViens
                        where nv.NhanVienID == ID && nv.MatKhau == password
                        select nv).FirstOrDefault();

            
            return user != null;
        }
    }
}
