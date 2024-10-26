using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QL_KhoHang
{
    public partial class frmStocks : Form
    {
        public frmStocks()
        {
            InitializeComponent();
        }

        DBConnect db = new DBConnect();

        void loadCboDanhMuc()
        {
            var danhMucList = db.GetAllDanhMuc();

            comboBoxDanhMuc.Items.Clear();

            foreach (var danhMuc in danhMucList)
            {
                comboBoxDanhMuc.Items.Add(new ComboBoxItem { Text = danhMuc.TenDanhMuc, Value = danhMuc.DanhMucID });
            }

            comboBoxDanhMuc.DisplayMember = "Text";
            comboBoxDanhMuc.ValueMember = "Value";
        }
        void loadCboSanPham()
        {
            var selectedDanhMuc = comboBoxDanhMuc.SelectedItem as ComboBoxItem;
            string selectedDanhMucID = selectedDanhMuc.Value;
            var sanPhamList = db.GetSanPhamByDanhMuc(selectedDanhMucID);
            cboSP.DataSource = sanPhamList;
            cboSP.ValueMember = "SanPhamID";
            cboSP.DisplayMember = "TenSanPham";
            
        }

        private void frmStocks_Load(object sender, EventArgs e)
        {
            loadCboDanhMuc();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            int rows = int.Parse(txtRow.Text);
            int columns = int.Parse(txtCol.Text);

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            var selectedDanhMuc = comboBoxDanhMuc.SelectedItem as ComboBoxItem;
            string selectedDanhMucID = selectedDanhMuc.Value;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {

                
                    ViTriKho vt = new ViTriKho();
                  
                    vt.ViTriID = ((char)('A' + i )).ToString() + (j+1) ;
                    vt.DanhMucID = selectedDanhMucID;
           
                    db.AddViTri(vt);

                }
           
            }
            LoadTable(selectedDanhMucID);
            MessageBox.Show("Khởi tạo thành công");
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        void loadBox(object sender, EventArgs e)
        {
            if (cboSP.SelectedValue == null) return;
            int inbox = db.GetTotalQuantityInBoxBySanPhamID(cboSP.SelectedValue.ToString());
            int total = db.GetQuantityBySanPhamID(cboSP.SelectedValue.ToString());
            lbBoxed.Text = total - inbox + "";
        }
        private void comboBoxDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCboSanPham();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoSize = true;
            if (comboBoxDanhMuc.SelectedItem != null)
            {
                btnInit.Enabled = true; 
                var selectedDanhMuc = comboBoxDanhMuc.SelectedItem as ComboBoxItem;

                if (selectedDanhMuc != null)
                {
                    string selectedDanhMucID = selectedDanhMuc.Value;
                    if (db.KTDanhMucTrong(selectedDanhMucID))
                    {
                      
                        LoadTable(selectedDanhMucID);
                     
                    }
                    else
                    {
                      
                        MessageBox.Show("Kho hàng chưa được khởi tạo");
                       
                    }
                }
            }
            else btnInit.Enabled = false; 
        }

        private void LoadTable(string danhMucID)
        {
            var viTriList = db.GetViTriByDanhMuc(danhMucID);
            if (viTriList.Count > 0)
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.RowStyles.Clear();

                int  rows = viTriList.Max(v => v.ViTriID[0]) - 'A' +1 ;
               
                int columns = viTriList.Max(v => int.Parse(v.ViTriID.Substring(1))) ;
              
                int buttonSize = 50;

                tableLayoutPanel1.ColumnCount = columns + 1;
                tableLayoutPanel1.RowCount = rows + 1;

                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, buttonSize / 2));
                for (int i = 0; i < columns; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, buttonSize));
                }

                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonSize / 2));
                for (int i = 0; i < rows; i++)
                {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonSize));
                }

                for (int i = 0; i < columns; i++)
                {
                    Label lbl = new Label();
                    lbl.Text = ((char)('A' + i)).ToString();
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    tableLayoutPanel1.Controls.Add(lbl, i + 1, 0);
                }
                for (int i = 0; i < columns; i++)
                {
                    Label lbl = new Label();
                    lbl.Text = i+1 +"";
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    tableLayoutPanel1.Controls.Add(lbl, 0, i + 1);
                }

             

                foreach (var viTri in viTriList)
                {
                    int row = int.Parse(viTri.ViTriID.Substring(1));
                    int column =  viTri.ViTriID[0] - 'A' + 1; 
                    Box box = new Box();
                    box.Size = new Size(buttonSize, buttonSize);
                    box.BackColor = Color.DarkOrange;
                    box.ViTri = viTri;
                    box.Dock = DockStyle.Fill;
                   
                    box.MouseHover += new EventHandler(Box_MouseEnter);
                    box.MouseLeave += new EventHandler(Box_MouseLeave);
                    box.Click += new EventHandler(loadBox);
                    tableLayoutPanel1.Controls.Add(box, column , row);
                }
            }
        }




        private void ShowBoxDetail(BoxDetail boxDetail)
        {
         
           
            Controls.Add(boxDetail);
            boxDetail.Visible = true;
            boxDetail.BringToFront();
            boxDetail.Refresh();
        }

        private void HideBoxDetail(BoxDetail boxDetail)
        {
            boxDetail.Visible = false;
            Controls.Remove(boxDetail);
        }

    

        private void Box_MouseEnter(object sender, EventArgs e)
        {
            Box box = sender as Box;

             ViTriKho vt = new ViTriKho();
        
             vt = box.ViTri;
             BoxDetail boxDetail = new BoxDetail(vt);
             Point cursorPosition = Cursor.Position;

             Point formRelativePosition = this.PointToClient(cursorPosition);
             formRelativePosition.X += 50;

             boxDetail.Location = formRelativePosition;

             ShowBoxDetail(boxDetail);
        }

        private void Box_MouseLeave(object sender, EventArgs e)
        {
            foreach (Control ctrl in Controls.OfType<BoxDetail>().ToList())
            {
               
              Controls.Remove(ctrl);
                ctrl.Dispose(); 
            }
        }

        private void btn_Reset(object sender, EventArgs e)
        {
            tableLayoutPanel1.Controls.Clear();
     
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            var selectedDanhMuc = comboBoxDanhMuc.SelectedItem as ComboBoxItem;
            string selectedDanhMucID = selectedDanhMuc.Value;
            db.DeleteViTriByDanhMuc(selectedDanhMucID);
        }

        bool isBoxOpen = false;
        private void btnOpenBox_Click(object sender, EventArgs e)
        {
            pnBoxSetting.Visible = !pnBoxSetting.Visible;
            if (isBoxOpen) btnOpenBox.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Vertical;
            else btnOpenBox.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
        }

        private void cboSP_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }






      
    }
}
