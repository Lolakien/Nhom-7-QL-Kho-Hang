using System;
using System.Linq;
using System.Windows.Forms;

namespace QL_KhoHang
{
    public partial class frmDMvaSP : Form
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        public frmDMvaSP()
        {
            InitializeComponent();
            LoadDanhMuc();
            LoadSanPham();
            LoadNhaCungCap();
            LoadSapXepOptions();
        }

        private void lblDM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click");
        }

        private void lblDM_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.SelectedRows.Count > 0)
            {
                var selectedRow = dgvDanhMuc.SelectedRows[0];
                var danhMucID = selectedRow.Cells["DanhMucID"].Value.ToString();

                // Xóa danh mục khỏi cơ sở dữ liệu
                var danhMuc = qlkh.DanhMucs.FirstOrDefault(dm => dm.DanhMucID == danhMucID);
                if (danhMuc != null)
                {
                    qlkh.DanhMucs.DeleteOnSubmit(danhMuc);
                    qlkh.SubmitChanges();
                }

                // Cập nhật lại DataGridView
                LoadDanhMuc();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn danh mục để xóa.");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var maDanhMuc = txtMaDanhMuc.Text;
            var tenDanhMuc = txtTenDanhMuc.Text;

            if (string.IsNullOrEmpty(maDanhMuc) || string.IsNullOrEmpty(tenDanhMuc))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            // Lưu danh mục vào cơ sở dữ liệu
            var danhMuc = qlkh.DanhMucs.FirstOrDefault(dm => dm.DanhMucID == maDanhMuc);
            if (danhMuc == null)
            {
                danhMuc = new DanhMuc { DanhMucID = maDanhMuc, TenDanhMuc = tenDanhMuc };
                qlkh.DanhMucs.InsertOnSubmit(danhMuc);
            }
            else
            {
                danhMuc.TenDanhMuc = tenDanhMuc;
            }
            qlkh.SubmitChanges();

            // Cập nhật lại DataGridView
            LoadDanhMuc();
        }

        private void dgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var selectedRow = dgvSanPham.Rows[e.RowIndex];
                    var maSanPham = selectedRow.Cells["SanPhamID"].Value.ToString();
                    var tenSanPham = selectedRow.Cells["TenSanPham"].Value.ToString();
                    var soLuong = selectedRow.Cells["SoLuong"].Value.ToString();
                    var giaBan = selectedRow.Cells["GiaBan"].Value.ToString();
                    var danhMuc = selectedRow.Cells["DanhMuc"].Value.ToString();
                    var nhaCungCap = selectedRow.Cells["NhaCungCap"].Value.ToString();

                    // Hiển thị thông tin chi tiết của sản phẩm
                    txtMaSanPham.Text = maSanPham;
                    txtTenSanPham.Text = tenSanPham;
                    txtSoLuongToiThieu.Text = soLuong;
                    txtGiaBan.Text = giaBan;

                    // Hiển thị danh mục và nhà cung cấp trong ComboBox
                    cboDanhMuc.Text = danhMuc;
                    cboNhaCungCap.Text = nhaCungCap;
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var selectedRow = dgvDanhMuc.Rows[e.RowIndex];
                    var maDanhMuc = selectedRow.Cells["DanhMucID"].Value.ToString();
                    var tenDanhMuc = selectedRow.Cells["TenDanhMuc"].Value.ToString();

                    // Hiển thị thông tin chi tiết của danh mục
                    txtMaDanhMuc.Text = maDanhMuc;
                    txtTenDanhMuc.Text = tenDanhMuc;

                    // Tải danh sách sản phẩm thuộc danh mục này
                    LoadSanPham(maDanhMuc);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void txtTimSanPham_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var keyword = txtTimSanPham.Text;
                var sanPhamList = qlkh.SanPhams.Where(sp => sp.TenSanPham.Contains(keyword)).ToList();
                dgvSanPham.DataSource = sanPhamList;
            }
        }

        private void cboSapXep_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSanPham();
        }

        private void cboDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSanPham();
        }

        private void cboNhaCungCap_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSanPham();
        }

        private void LoadDanhMuc()
        {
            var danhMucList = qlkh.DanhMucs.ToList();

            // Thêm tùy chọn "Tất cả"
            // danhMucList.Insert(0, new DanhMuc { DanhMucID = "", TenDanhMuc = "Tất cả" });

            dgvDanhMuc.DataSource = danhMucList;

            // Load dữ liệu vào combobox
            cboDanhMuc.DataSource = danhMucList;
            cboDanhMuc.DisplayMember = "TenDanhMuc";
            cboDanhMuc.ValueMember = "DanhMucID";


                        // Đặt tên cột bằng tiếng Việt
            dgvDanhMuc.Columns["DanhMucID"].HeaderText = "Mã danh mục";
            dgvDanhMuc.Columns["TenDanhMuc"].HeaderText = "Tên danh mục";
        }

        private void LoadSanPham(string maDanhMuc = null)
        {
            var sanPhamList = from sp in qlkh.SanPhams
                              join dm in qlkh.DanhMucs on sp.DanhMucID equals dm.DanhMucID
                              join ncc in qlkh.NhaCungCaps on sp.NhaCungCapID equals ncc.NhaCungCapID
                              select new
                              {
                                  sp.SanPhamID,
                                  sp.TenSanPham,
                                  sp.SoLuong,
                                  sp.GiaBan,
                                  DanhMuc = dm.TenDanhMuc,
                                  NhaCungCap = ncc.TenNhaCungCap,
                                  sp.DanhMucID,
                                  sp.NhaCungCapID
                              };

            // Lọc theo danh mục
            if (cboDanhMuc.SelectedIndex > 0)
            {
                var selectedDanhMucID = cboDanhMuc.SelectedValue.ToString();
                sanPhamList = sanPhamList.Where(sp => sp.DanhMucID == selectedDanhMucID);
            }

            // Lọc theo nhà cung cấp
            if (cboNhaCungCap.SelectedIndex > 0)
            {
                var selectedNhaCungCapID = cboNhaCungCap.SelectedValue.ToString();
                sanPhamList = sanPhamList.Where(sp => sp.NhaCungCapID == selectedNhaCungCapID);
            }

            // Sắp xếp
            var selectedSort = cboSapXep.SelectedItem?.ToString();
            if (selectedSort == null)
            {
                // Handle the case where no sort option is selected
                return;
            }

            switch (selectedSort)
            {
                case "Tên sản phẩm":
                    sanPhamList = sanPhamList.OrderBy(sp => sp.TenSanPham);
                    break;
                case "Giá bán":
                    sanPhamList = sanPhamList.OrderBy(sp => sp.GiaBan);
                    break;
                case "Số lượng":
                    sanPhamList = sanPhamList.OrderBy(sp => sp.SoLuong);
                    break;
            }

            dgvSanPham.DataSource = sanPhamList.ToList();

            // Đặt tên cột bằng tiếng Việt
            dgvSanPham.Columns["SanPhamID"].HeaderText = "Mã Sản Phẩm";
            dgvSanPham.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dgvSanPham.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán";
            dgvSanPham.Columns["DanhMuc"].HeaderText = "Danh Mục";
            dgvSanPham.Columns["NhaCungCap"].HeaderText = "Nhà Cung Cấp";
        }

        private void LoadNhaCungCap()
        {
            var nhaCungCapList = qlkh.NhaCungCaps.ToList();

            // Thêm tùy chọn "Tất cả"
            // nhaCungCapList.Insert(0, new NhaCungCap { NhaCungCapID = "", TenNhaCungCap = "Tất cả" });

            cboNhaCungCap.DataSource = nhaCungCapList;
            cboNhaCungCap.DisplayMember = "TenNhaCungCap";
            cboNhaCungCap.ValueMember = "NhaCungCapID";
        }

        private void LoadSapXepOptions()
        {
            cboSapXep.Items.Add("Tên sản phẩm");
            cboSapXep.Items.Add("Giá bán");
            cboSapXep.Items.Add("Số lượng");
            cboSapXep.SelectedIndex = 0;
        }
    }
}