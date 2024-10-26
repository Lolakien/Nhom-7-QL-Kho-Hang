using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoHang
{
    public partial class BoxDetail : UserControl
    {
     
        public BoxDetail(ViTriKho vt)
        {
            InitializeComponent();
            if (vt != null)
            {
                lbPosition.Text = vt.ViTriID;
                lbNumber.Text = "Vị trí: " + vt.SoLuong + " / " + vt.SoLuongToiDa;
            }
        }
      
   
        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void BoxDetail_Load(object sender, EventArgs e)
        {
            
         
        }
    }
}
