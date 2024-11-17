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
using System.Configuration;
using System.Windows.Forms;
using System.Xml;
using System.Data.Sql;
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

     
        



        private void lblExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        
            string server = Environment.MachineName;
            lblServerName.Text = server;
            string newConnectionString = "Data Source="+ server+";Initial Catalog=QL_HangHoa;Integrated Security=True";
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["QL_KhoHang.Properties.Settings.QL_HangHoaConnectionString"].ConnectionString = newConnectionString;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");

        }


      
     
    }
}
