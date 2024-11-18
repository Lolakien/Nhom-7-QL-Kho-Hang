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
       
        public static string UserID = String.Empty;
        public static string RoleID = String.Empty;

        public static string getUserID()
        {
            return UserID;
        }
        public static string getRoleID() { 
            return RoleID;
        }
        public static bool ValidateUser(string ID, string password)
        {
            QL_KhoHangDataContext QLKho = new QL_KhoHangDataContext();
            var user = (from nv in QLKho.NhanViens
                        where nv.NhanVienID == ID && nv.MatKhau == password
                        select nv).FirstOrDefault();
            if(user==null) return false;
            UserID = user.NhanVienID;
            RoleID=user.VaiTroID;

            return true;
        }
    }
}
