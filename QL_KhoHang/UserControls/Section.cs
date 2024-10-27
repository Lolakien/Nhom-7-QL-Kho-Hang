using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoHang.UserControls
{
    public partial class Section : UserControl
    {
        public DanhMuc dm { get; set; }
        public DBConnect db = new DBConnect();
        public Section(DanhMuc dm)
        {
            InitializeComponent();
            this.dm = dm;
            lbTenSP.Text = dm.TenDanhMuc;
        }

        private void Section_Leave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void Section_Load(object sender, EventArgs e)
        {
            int tong = db.viTriKhoController.DemTongViTriKho(dm.DanhMucID);
            int daXep = db.viTriKhoController.DemViTriKhoDaXep(dm.DanhMucID);
            int trong = tong - daXep;
            if (tong > 0) 
            {
                float phanTramDaXep = (float)daXep / tong * 100;

                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke(new Action(() => progressBar.Value = (int)phanTramDaXep));
                }
                else
                {
                    progressBar.Value = (int)phanTramDaXep;
                }          
            }
            else
            {
                progressBar.Value = 0; 

            }

        }

      

     
    }
}
