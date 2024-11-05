using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoHang
{
    public partial class frmDMvaSP : Form
    {
        public frmDMvaSP()
        {
            InitializeComponent();
        }

        private void lblDM_Click(object sender, EventArgs e)
        {
           
            MessageBox.Show("Click");
        }

        private void bunifuDataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblDM_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }
    }
}
