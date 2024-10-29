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
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            
            frmDashboard f = new frmDashboard();

            openChildForm(f);
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            lblID.Text = Authentication.ID;

            openChildForm(new frmStocks());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmStocks f = new frmStocks();
            openChildForm(f);
            
        }

        private Form activeForm = null;

        private void openChildForm(Form childForm)
        {
            if (activeForm != null) activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnLoad.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
        }

        private void pnLoad_Paint(object sender, PaintEventArgs e)
        {

        }
        private void frmHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnPhieuNhap_Click(object sender, EventArgs e)
        {
            frmPhieuNhap f = new frmPhieuNhap();
            openChildForm(f);
        }

        private void btnPhieuXuat_Click(object sender, EventArgs e)
        {
            frmPhieuXuat f = new frmPhieuXuat();
            openChildForm(f);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
