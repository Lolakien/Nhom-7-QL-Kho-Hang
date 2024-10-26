using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_KhoHang.Controller
{
     class DanhMucController
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        public List<DanhMuc> GetAllDanhMuc()
        {
            return qlkh.DanhMucs.ToList();
        }

        public bool DanhMucIsNotEmpty(string danhMucID)
        {
            var viTriCount = (from vt in qlkh.ViTriKhos
                              where vt.DanhMucID == danhMucID
                              select vt).Count();

            return viTriCount > 0;
        }
    }
}
