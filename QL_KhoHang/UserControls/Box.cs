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
    public partial class Box : UserControl
    {
        public Box()
        {
            InitializeComponent();
    
          
        }
      public  ViTriKho ViTri {get;set;}
        private void Box_Load(object sender, EventArgs e)
        {
            if (ViTri != null)
            {
                double daChua = ((float)ViTri.SoLuong / ViTri.SoLuongToiDa) * 100;
                if (daChua == 100) BackColor = Color.DarkOrange;
                if (daChua > 70) BackColor = Color.SandyBrown;
                else BackColor = Color.LightYellow;
            }
            else BackColor = Color.Khaki;
        }


  
    }
}
