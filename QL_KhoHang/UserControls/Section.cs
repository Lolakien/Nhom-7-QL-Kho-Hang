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
    }
}
