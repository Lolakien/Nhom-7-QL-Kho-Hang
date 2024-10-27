using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace QL_KhoHang
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
         
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
                txtPassword.PasswordChar = '\0';
            else txtPassword.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String ID = txtID.Text.Trim();
            String password = txtPassword.Text.Trim();
            if (Authentication.ValidateUser(ID, password))
            {
                Authentication.ID = ID; 
                frmHome f = new frmHome();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Người dùng không hợp lệ");
            }
        }
        private const string shitty = "nvm.bin";

        //Đừng quan tâm hàm này
        void DungQuanTamHamNay()
        {
            int forgetable = 0;     
            if (File.Exists(shitty))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(shitty, FileMode.Open)))
                {
                    forgetable = reader.ReadInt32(); 
                }
            }
            forgetable++;
            using (BinaryWriter writer = new BinaryWriter(File.Open(shitty, FileMode.Create)))
            {
                writer.Write(forgetable); 
            }
        }



        private void lblExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
