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

        void Display()
        {
            if (Authentication.getRoleID() == "QUANLY")
            {
                btnPhieuXuat.Visible = true;
                btnPhieuNhap.Visible = true;
               
                btnSanPham.Visible = true;
                btnStock.Visible = true;
   
                btnDashboard.Visible = true;
         
                frmDashboard f = new frmDashboard();
                openChildForm(f);
            }
            if (Authentication.getRoleID() == "NHAPKHO")
            {
                btnPhieuNhap.Visible = true;
                frmPhieuNhap f = new frmPhieuNhap();
              
            }
            if (Authentication.getRoleID() == "XUATKHO")
            {
             
                btnStock.Visible = true;
                btnPhieuXuat.Visible = true;
                frmPhieuXuat f = new frmPhieuXuat();
                openChildForm(f);
            }
          
        }
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            
            frmDashboard f = new frmDashboard();

            openChildForm(f);
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            lblID.Text = Authentication.getUserID();
            Display();
           
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

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            frmDMvaSP f = new frmDMvaSP();
            openChildForm(f);
        }
    }
}
