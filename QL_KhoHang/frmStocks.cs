using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QL_KhoHang.UserControls;
using QL_KhoHang.Controller;
namespace QL_KhoHang
{
    public partial class frmStocks : Form
    {
        public frmStocks()
        {
            InitializeComponent();
        }

        DBConnect db = new DBConnect();
        List<Box> listBox = new List<Box>();

        void loadCboSanPham()
        {
            string selectedDanhMucID = selectedDanhMuc;
            var sanPhamList = db.sanPhamController.GetSanPhamByDanhMuc(selectedDanhMucID);
            cboSP.DataSource = sanPhamList;
            cboSP.ValueMember = "SanPhamID";
            cboSP.DisplayMember = "TenSanPham";  
        }

        void loadPanelDanhMuc()
        {
            var danhMucList = db.danhMucController.GetAllDanhMuc();
            foreach (var danhMuc in danhMucList)
            {
                Section s = new Section(danhMuc);
                s.Click += SectionDanhMuc_Click;
                s.Dock = DockStyle.Top;
                pnSection.Controls.Add(s);
            }
        }

        String selectedDanhMuc;
        void SectionDanhMuc_Click(object sender, EventArgs e)
        {
            ClearTable();
            Section s = sender as Section;
            s.BackColor = Color.Gainsboro;
            selectedDanhMuc = s.dm.DanhMucID;
            lbDanhMuc.Text = s.dm.TenDanhMuc;
            loadCboSanPham();
            if (selectedDanhMuc != null)
            {
                if (db.danhMucController.DanhMucIsNotEmpty(selectedDanhMuc))
                {
                    pnInit.Visible = false;
                    LoadTable(selectedDanhMuc);
                }
                else
                {
                    pnInit.Visible = true;
                
                }
            }
        }
        void loadPhieuNhap()
        {
            var listPhieuNhap = db.phieuNhapController.GetAllPhieuNhap();
            listboxPhieuNhap.DataSource = listPhieuNhap;
            listboxPhieuNhap.DisplayMember = "PhieuNhapID";
            listboxPhieuNhap.ValueMember = "PhieuNhapID";

        }
        public void loadChiTietPN(Bunifu.UI.WinForms.BunifuDataGridView bunifuDataGridView)
        {
            var data = db.phieuNhapController.GetChiTietPhieuNhapInfo(listboxPhieuNhap.SelectedValue.ToString());
            dgvSanPham.DataSource = data;
            bunifuDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            bunifuDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            bunifuDataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            bunifuDataGridView.BorderStyle = BorderStyle.None;
            bunifuDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }
        void loadProgressPanel()
        {
            int tong = db.viTriKhoController.DemTongViTriKho(selectedDanhMuc);
            int daXep = db.viTriKhoController.DemViTriKhoDaXep(selectedDanhMuc);
            int trong = tong-daXep;
            lbDaChua.Text = daXep.ToString();
            lbConTrong.Text = trong.ToString();
            if (tong > 0)
            {
                float phanTramDaXep = (float)daXep / tong * 100;
                bunifuCircleProgressbar1.Value = (int)phanTramDaXep;
                bunifuCircleProgressbar1.Text = (int)phanTramDaXep +  "%";
            }
            else
            {
                bunifuCircleProgressbar1.Value = 0; 
                bunifuCircleProgressbar1.Text = "0%";
            }
        }

        private void frmStocks_Load(object sender, EventArgs e)
        {

            loadPhieuNhap();
            loadPanelDanhMuc();
            txtTimKiemSetup();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            int rows = int.Parse(txtRow.Text);
            int columns = int.Parse(txtCol.Text);
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            string selectedDanhMucID = selectedDanhMuc;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {     
                    ViTriKho vt = new ViTriKho();         
                    vt.ViTriID = ((char)('A' + i )).ToString() + (j+1) ;
                    vt.DanhMucID = selectedDanhMucID;
                    db.viTriKhoController.AddViTri(vt);
                }
            }
            LoadTable(selectedDanhMucID);
            MessageBox.Show("Khởi tạo thành công");
        }

        ViTriKho selectedViTriKho;
        Box selectedBox;
        void Box_Click(object sender, EventArgs e)
        {
        
            Box box = sender as Box;
            selectedBox = box;
            selectedViTriKho = box.ViTri;
            txtSoLuong.Text = box.ViTri.SoLuong.ToString();
            txtSoLuongMax.Text = box.ViTri.SoLuongToiDa.ToString();
      
            box.ViTri = selectedViTriKho;
            lblBoxID.Text = box.ViTri.ViTriID;
         
           if(box.ViTri.SanPhamID !=null) cboSP.SelectedValue = box.ViTri.SanPhamID;
            if (cboSP.SelectedValue != null)
            {
                
                int daXep = db.sanPhamController.GetTotalQuantityInBoxBySanPhamID(cboSP.SelectedValue.ToString());
                int total = db.sanPhamController.GetQuantityBySanPhamID(cboSP.SelectedValue.ToString());
                lblChuaXep.Text = "Còn " + (total - daXep) + " sản phẩm chưa được xếp vào kho";
            }
         
            
        }

        void ClearTable()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
        }
        private void LoadTable(string danhMucID)
        {
            loadProgressPanel();
            btnReset.Visible = false;
            pnInit.Visible = false;
            ClearTable();
            tableLayoutPanel1.AutoSize = true;

            var viTriList = db.viTriKhoController.GetListViTriByDanhMuc(danhMucID);
            if (viTriList.Count > 0)
            {
                lbDanhMuc.Visible = true;
                btnReset.Visible = true;
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.RowStyles.Clear();

                int rows = viTriList.Max(v => v.ViTriID[0]) - 'A' + 1;

                int columns = viTriList.Max(v => int.Parse(v.ViTriID.Substring(1)));

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
                //Add dòng chữ
                for (int i = 0; i < columns; i++)
                {
                    Label lbl = new Label();
                    lbl.Text = ((char)('A' + i)).ToString();
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    tableLayoutPanel1.Controls.Add(lbl, i + 1, 0);
                }
                //Add côt số
                for (int i = 0; i < columns; i++)
                {
                    Label lbl = new Label();
                    lbl.Text = i + 1 + "";
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    tableLayoutPanel1.Controls.Add(lbl, 0, i + 1);
                }

                foreach (var viTri in viTriList)
                {
                    int row = int.Parse(viTri.ViTriID.Substring(1));
                    int column = viTri.ViTriID[0] - 'A' + 1;
                    //Add box 
                    Box box = new Box();
                    listBox.Add(box);
                    box.Size = new Size(buttonSize, buttonSize);

                    box.ViTri = viTri;
                    box.Dock = DockStyle.Fill;

                    box.MouseHover += new EventHandler(Box_MouseEnter);
                    box.MouseLeave += new EventHandler(Box_MouseLeave);
                    box.Click += new EventHandler(Box_Click);
                    tableLayoutPanel1.Controls.Add(box, column, row);
                }
            }
            else
            {
                pnInit.Visible = true;
                MessageBox.Show("Kho hàng chưa được khởi tạo");
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
        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearTable();
            pnInit.Visible = true;
            btnReset.Visible = false;
            string selectedDanhMucID = selectedDanhMuc;
            db.viTriKhoController.DeleteViTriByDanhMuc(selectedDanhMucID);
        }
        private void btnOpenBox_Click(object sender, EventArgs e)
        {
            pnBoxSetting.Visible = !pnBoxSetting.Visible;
            if (pnBoxSetting.Visible) btnOpenBox.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Vertical;
            else btnOpenBox.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Horizontal; ;
        }

        private void cboSP_TextChanged(object sender, EventArgs e)
        {
            string searchText = cboSP.Text;
            var matchingProducts = db.sanPhamController.SearchSanPhamByDanhMucID(searchText, selectedDanhMuc);
            cboSP.DataSource = matchingProducts;
            cboSP.DisplayMember = "TenSanPham";
            cboSP.ValueMember = "SanPhamID";
            cboSP.Text = searchText;
            cboSP.SelectionStart = searchText.Length;
            cboSP.SelectionLength = 0;
            cboSP.DroppedDown = true;
        }

        private void listboxPhieuNhap_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadChiTietPN(dgvSanPham);
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
               if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var maSanPham = dgvSanPham.Rows[e.RowIndex].Cells["MaSanPham"].Value;
                    var maDanhMuc = dgvSanPham.Rows[e.RowIndex].Cells["DanhMucID"].Value;
                    var tenDanhMuc = dgvSanPham.Rows[e.RowIndex].Cells["TenDanhMuc"].Value;
                    selectedDanhMuc = maDanhMuc.ToString();
                    LoadTable(selectedDanhMuc);
                    var matchingProducts = db.sanPhamController.SearchSanPhamByDanhMucID("", selectedDanhMuc);
                    cboSP.DataSource = matchingProducts;
                    cboSP.DisplayMember = "TenSanPham";
                    cboSP.ValueMember = "SanPhamID";
                    cboSP.SelectedValue = maSanPham;
                    lbDanhMuc.Text = tenDanhMuc.ToString();
                }
        }
      

        private void btnBoxSave_Click(object sender, EventArgs e)
        {
            string viTriID = selectedViTriKho.ViTriID;
            ViTriKho vt = new ViTriKho();
           
            vt.SanPhamID =cboSP.SelectedValue.ToString();
            vt.SoLuong =int.Parse(txtSoLuong.Text);
            vt.SoLuongToiDa =int.Parse(txtSoLuongMax.Text);
            db.viTriKhoController.UpdateViTriKho(viTriID,vt);
            selectedBox.setBox();
            loadProgressPanel();
            MessageBox.Show("Lưu thành công");
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            bool found = false;
            if (e.KeyCode == Keys.Enter)
            {
                foreach (Box box in listBox)
                {
                    box.setBorderDefault();
                    if (box.tenSP == txtTimKiem.Text)
                    {
                        found = true;
                        box.setBorder();
                    }                
                }
                if (!found) MessageBox.Show("Không tìm thấy sản phẩm");
            }
        }

        void txtTimKiemSetup()
        {
            List<SanPham> listSP = db.sanPhamController.GetAllSanPham();
            List<string> listTenSP = new List<string>();          
            foreach(SanPham sp in listSP)
            {
                listTenSP.Add(sp.TenSanPham);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
            autoCompleteData.AddRange(listTenSP.ToArray());
            txtTimKiem.AutoCompleteCustomSource = autoCompleteData;
        }
    }
}
