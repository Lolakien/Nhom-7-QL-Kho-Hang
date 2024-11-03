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
    public partial class Box : UserControl
    {
        public Box()
        {
            InitializeComponent();
    
          
        }
      public  ViTriKho ViTri {get;set;}
      public String tenSP;
      public SanPhamController spController = new SanPhamController();
        
        public void Box_Load(object sender, EventArgs e)
        {
            setBox();
         
        }

        public void setBox()
        {
            if (ViTri != null)
            {
                if (ViTri.SanPhamID != null)
                {

                    SanPham sp = spController.GetSanPhamByID(ViTri.SanPhamID);
                    tenSP = sp.TenSanPham;
                }
                double daChua = ((float)ViTri.SoLuong / ViTri.SoLuongToiDa) * 100;

                if (daChua == 100) BackColor = Color.DarkOrange;
                else
                    if (daChua > 70) BackColor = Color.SandyBrown;

                    else if (daChua > 0) BackColor = Color.Khaki;
                    else BackColor = Color.LightGray;
            }
            else BackColor = Color.LightGray;
        }
        public void setBorder()
        {
            pnTop.BackColor = Color.LimeGreen;
            pnBottom.BackColor = Color.LimeGreen;
            pnLeft.BackColor = Color.LimeGreen;
            pnRight.BackColor = Color.LimeGreen;

        }

        public void setBorderDefault()
        {
            pnTop.BackColor = SystemColors.Control;
            pnBottom.BackColor = SystemColors.Control;
            pnLeft.BackColor = SystemColors.Control;
            pnRight.BackColor = SystemColors.Control;

        }

        public void pnChild_Click(object sender, EventArgs e)
        {

        }

        private void Box_Click(object sender, EventArgs e)
        {

        }


  
    }
}
