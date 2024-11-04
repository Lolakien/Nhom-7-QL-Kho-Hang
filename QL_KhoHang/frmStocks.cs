using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QL_KhoHang.UserControls;
using QL_KhoHang.Controller;
using QL_KhoHang.MiniForm;
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
                    btnInit.Visible = false;
                    LoadTable(selectedDanhMuc);
                }
                else
                {
                    btnInit.Visible = true;
                
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
            InitFrm f = new InitFrm();
            f.ShowDialog();
        }

      public void initTable(int rows, int columns)
        {
         
         
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            string selectedDanhMucID = selectedDanhMuc;
            for (int i = 0; i <= rows; i++)
            {
                for (int j = 0; j <= columns; j++)
                {
                    ViTriKho vt = new ViTriKho();
                    vt.ViTriID = ((char)('A' + i)).ToString() + (j + 1);
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
            //Hien thi tren thong tin vi tri
            selectedBox = box;
            selectedViTriKho = box.ViTri;
            txtSoLuong.Text = box.ViTri.SoLuong.ToString();
            txtSoLuongMax.Text = box.ViTri.SoLuongToiDa.ToString();
            box.ViTri = selectedViTriKho;
            lblBoxID.Text = box.ViTri.ViTriID;
         
           if(box.ViTri.SanPhamID !=null) cboSP.SelectedValue = box.ViTri.SanPhamID;
            if (cboSP.SelectedValue != null)
            {
                
               int chuaXepViTri = db.sanPhamController.GetTotalQuantityNotInBoxBySanPhamID(cboSP.SelectedValue.ToString());
               lblChuaXep.Text = chuaXepViTri.ToString();
            }
            //Hien thi tren phieu xuat kho
            lbXKTenSP.Text = cboSP.Text;
            txtXKSoLuong.Text = box.ViTri.SoLuong.ToString();
            lbXKViTri.Text = box.ViTri.ViTriID;


            foreach (Box b in listBox)
            {
                b.setBorderDefault();
            }
            box.setBorder();
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
            btnInit.Visible = false;
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
                    btnInit.Visible = true;
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
            btnInit.Visible = true;
            btnReset.Visible = false;
            string selectedDanhMucID = selectedDanhMuc;
            db.viTriKhoController.DeleteViTriByDanhMuc(selectedDanhMucID);
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
            int soLuong = int.Parse(txtSoLuong.Text);
            int chuaXepViTri = db.sanPhamController.GetTotalQuantityNotInBoxBySanPhamID(cboSP.SelectedValue.ToString());
            int soLuongToiDa = int.Parse(txtSoLuongMax.Text);
           
            if (soLuong > soLuongToiDa || soLuong > chuaXepViTri) MessageBox.Show("Số lượng không hợp lệ ");
            else
            {
                ViTriKho vt = new ViTriKho();


                vt.SanPhamID = cboSP.SelectedValue.ToString();
                vt.SoLuong = soLuong;

                vt.SoLuongToiDa = soLuongToiDa;
                if (db.viTriKhoController.UpdateViTriKho(viTriID, vt))
                {
                    MessageBox.Show("Cập nhật thành công");
                    int daXepViTri = db.sanPhamController.GetTotalQuantityNotInBoxBySanPhamID(cboSP.SelectedValue.ToString());
                    lblChuaXep.Text = daXepViTri.ToString();
                    selectedBox.ViTri = vt;
                    selectedBox.setBox();
                    loadProgressPanel();
                }
                else MessageBox.Show("Cập nhật thất bại");
       

            }
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

        private void btnThemXK_Click(object sender, EventArgs e)
        {
            ViTriKho vt = selectedBox.ViTri;
            string masp = vt.SanPhamID;
            int sl = vt.SoLuong;
            bool isExisting = false;
            SanPham sp = db.sanPhamController.GetSanPhamByID(masp);
            if (vt.SoLuong - Int32.Parse(txtXKSoLuong.Text) < 0) MessageBox.Show("Số lượng xuất > số lượng hiện có, nhập lại !");
            else
            {
                vt.SoLuong -= Int32.Parse(txtXKSoLuong.Text);
                //Neu da ton tai SP thi + them so luong va tong tien
                foreach (DataGridViewRow row in dgvChiTietPhieuXuat.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == masp)
                    {
                        int currentQuantity = Convert.ToInt32(row.Cells["CTSoLuong"].Value);
                        decimal tongTien = Convert.ToDecimal(row.Cells["CTTongTien"].Value);
                        row.Cells[2].Value = currentQuantity + sl;
                        row.Cells["CTTongTien"].Value = tongTien + (sl * sp.GiaBan);
                        isExisting = true;
                        break;
                    }
                }
                //Them vao lich su vi tri xuat
                dgvViTri.Rows.Add(masp, sl, vt.ViTriID, selectedDanhMuc);
                if (!isExisting)
                {

                    dgvChiTietPhieuXuat.Rows.Add(masp, lbXKTenSP.Text, sl, sp.GiaBan, (sl * sp.GiaBan));

                }
            }
        }

        void PhieuXuatChange()
        {
            if (txtTenKH.Text == null) return;
            string date = dtpNgayXuat.Value.ToString("ddMMyyyy");
            string MaPX = db.phieuXuatController.GenerateMaPhieuXuat(txtMaKH.Text,date);
            txtMaPX.Text = MaPX;
        }

        private void dtpNgayXuat_ValueChanged(object sender, EventArgs e)
        {
            PhieuXuatChange();
        }

  
        private void txtMaKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                KhachHang kh = db.khachHangController.GetKhachHangByID(txtMaKH.Text.Trim());
                txtTenKH.Text = kh.TenKH;
                PhieuXuatChange();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maKH = txtMaKH.Text;
            DateTime ngayXuat = dtpNgayXuat.Value.Date;
            string ngayThangNam = ngayXuat.ToString("dd/MM/yyyy");
            string maPX = txtMaPX.Text;

          
            List<dynamic> chiTietPhieuXuats = new List<dynamic>();
            
            foreach (DataGridViewRow row in dgvChiTietPhieuXuat.Rows)
            {
                if (row.Cells["CTMaSanPham"].Value != null)
                {
                    chiTietPhieuXuats.Add(new
                    {
                        MaSanPham = row.Cells["CTMaSanPham"].Value.ToString(),
                        SoLuong = Convert.ToInt32(row.Cells["CTSoLuong"].Value),
                        GiaXuat = Convert.ToDecimal(row.Cells["CTGiaXuat"].Value)
                    });
                }
            }

            //Vi tri
            List<dynamic>ViTris = new List<dynamic>();
            foreach (DataGridViewRow row in dgvViTri.Rows)
            {
                if (row.Cells["VTMaSanPhamID"].Value != null)
                {
                    
                    ViTris.Add(new
                    {
                        MaSanPham = row.Cells["VTMaSanPhamID"].Value.ToString(),
                        SoLuong = Convert.ToInt32(row.Cells["VTSoLuong"].Value),
                        ViTriID = row.Cells["VTViTri"].Value,
                        DanhMucID = row.Cells["VTDanhMucID"].Value.ToString()
                    });
                }
            }

            // Gọi phương thức ThemPhieuXuat để lưu vào CSDL
            bool isSuccess = db.phieuXuatController.ThemPhieuXuat(Authentication.ID, maKH, ngayXuat, maPX, chiTietPhieuXuats, txtGhiChu.Text);
            if (isSuccess)
            {
                selectedBox.ViTri.SoLuong -= Int16.Parse(txtXKSoLuong.Text);
                selectedBox.setBackColor();
                MessageBox.Show("Lưu phiếu xuất thành công!");
              
              
                // Bạn có thể thêm mã để làm mới giao diện sau khi lưu thành công, nếu cần
            }
            else
            {
                MessageBox.Show("Lưu phiếu xuất thất bại!");
            }
        }
     
    }
}
