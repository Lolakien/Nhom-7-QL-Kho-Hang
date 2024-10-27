using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_KhoHang.Controller;
namespace QL_KhoHang
{
    public partial class BoxDetail : UserControl
    {
        SanPhamController spController = new SanPhamController();
        ViTriKho vt;
        public BoxDetail(ViTriKho vt)
        {
            InitializeComponent();
            this.vt = vt;
        }
      
   
        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void BoxDetail_Load(object sender, EventArgs e)
        {
            if (vt != null)
            {
                SanPham sp = spController.GetSanPhamByID(vt.SanPhamID);
                if (sp != null)
                {
                    lbName.Text = sp.TenSanPham;
                }
                lbPosition.Text = vt.ViTriID;

                lbNumber.Text = vt.SoLuong + " / " + vt.SoLuongToiDa;

            }
         
        }
    }
}
