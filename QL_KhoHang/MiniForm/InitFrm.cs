using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoHang.MiniForm
{
    public partial class InitFrm : Form
    {
      
        public InitFrm()
        {
            InitializeComponent();
  
        
         }
        private void InitFrm_Load(object sender, EventArgs e)
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    var panel = new Panel
                    {
                        Size = new Size(10, 10),
                        Dock = DockStyle.Fill,
                        BackColor = Color.LightGray,
                        Tag = new Point(row, col),
                       
                     
                    };
                    panel.MouseEnter += Panel_MouseEnter;
                    panel.MouseClick += Panel_Click;
                    tableLayoutPanel1.Controls.Add(panel, col, row);
                }
            }
        }
        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            Panel button = sender as Panel; 
            if (button != null)
            {
                Point position = (Point)button.Tag;
                int row = position.X;
                int col = position.Y;

                lblSize.Text = (row +1) + " x " + (col +1);
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    Panel btn = control as Panel;
                    if (btn != null)
                    {
                        Point btnPosition = (Point)btn.Tag;    
                        if (btnPosition.X <= row && btnPosition.Y <= col)
                        {
                            btn.BackColor = Color.Orange; 
                        }
                        else
                        {
                            btn.BackColor = SystemColors.Control;
                        }
                    }
                }
               
               
            }

        }
        void Panel_Click(object sender, EventArgs e)
        {
            Panel button = sender as Panel; 
            if (button != null)
            {
                Point position = (Point)button.Tag;
                int row = position.X;
                int col = position.Y;
                frmStocks f = Application.OpenForms.OfType<frmStocks>().FirstOrDefault();

                if (f != null)
                {
                    this.Close();
                    f.initTable(row, col);
          
                }

            }
            
        }

        
    }
}
