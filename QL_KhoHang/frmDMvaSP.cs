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

        private void btnSua_Click(object sender, EventArgs e)
        {
            var maDanhMuc = txtMaDanhMuc.Text;
            var tenDanhMuc = txtTenDanhMuc.Text;

            if (string.IsNullOrEmpty(maDanhMuc) || string.IsNullOrEmpty(tenDanhMuc))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            // Sửa thông tin danh mục trong cơ sở dữ liệu
            var danhMuc = qlkh.DanhMucs.FirstOrDefault(dm => dm.DanhMucID == maDanhMuc);
            if (danhMuc != null)
            {
                danhMuc.TenDanhMuc = tenDanhMuc;
                qlkh.SubmitChanges();
            }

            // Cập nhật lại DataGridView
            LoadDanhMuc();
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

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            try
            {
                var maSanPham = txtMaSanPham.Text;
                var tenSanPham = txtTenSanPham.Text;
                if (!int.TryParse(txtSoLuongToiThieu.Text, out var soLuong))
                {
                    MessageBox.Show("Số lượng không hợp lệ.");
                    return;
                }
                if (!float.TryParse(txtGiaBan.Text, out var giaBan))
                {
                    MessageBox.Show("Giá bán không hợp lệ.");
                    return;
                }

                // Validate giaBan range
                if (giaBan < 0 || giaBan > 9999999)
                {
                    MessageBox.Show("Giá bán vượt quá phạm vi cho phép.");
                    return;
                }

                var danhMucID = cboDanhMuc.SelectedValue.ToString();
                var nhaCungCapID = cboNhaCungCap.SelectedValue.ToString();

                if (string.IsNullOrEmpty(maSanPham) || string.IsNullOrEmpty(tenSanPham))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                // Check if the product already exists
                var existingSanPham = qlkh.SanPhams.FirstOrDefault(sp => sp.SanPhamID == maSanPham);
                if (existingSanPham != null)
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại. Vui lòng chọn mã sản phẩm khác.");
                    return;
                }

                // Thêm sản phẩm mới vào cơ sở dữ liệu
                var sanPham = new SanPham
                {
                    SanPhamID = maSanPham,
                    TenSanPham = tenSanPham,
                    SoLuong = soLuong,
                    GiaBan = giaBan,
                    DanhMucID = danhMucID,
                    NhaCungCapID = nhaCungCapID
                };
                qlkh.SanPhams.InsertOnSubmit(sanPham);
                qlkh.SubmitChanges();

                // Cập nhật lại DataGridView
                LoadSanPham();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }


        private void btnSuaSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                var maSanPham = txtMaSanPham.Text;
                var tenSanPham = txtTenSanPham.Text;
                if (!int.TryParse(txtSoLuongToiThieu.Text, out var soLuong))
                {
                    MessageBox.Show("Số lượng không hợp lệ.");
                    return;
                }
                if (!Double.TryParse(txtGiaBan.Text, out var giaBan))
                {
                    MessageBox.Show("Giá bán không hợp lệ.");
                    return;
                }

                // Validate giaBan range
                if (giaBan < 0 || giaBan > 9999999)
                {
                    MessageBox.Show("Giá bán vượt quá phạm vi cho phép.");
                    return;
                }

                var danhMucID = cboDanhMuc.SelectedValue.ToString();
                var nhaCungCapID = cboNhaCungCap.SelectedValue.ToString();

                if (string.IsNullOrEmpty(maSanPham) || string.IsNullOrEmpty(tenSanPham))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                // Sửa thông tin sản phẩm trong cơ sở dữ liệu
                var sanPham = qlkh.SanPhams.FirstOrDefault(sp => sp.SanPhamID == maSanPham);
                if (sanPham != null)
                {
                    sanPham.TenSanPham = tenSanPham;
                    sanPham.SoLuong = soLuong;
                    sanPham.GiaBan = giaBan;
                    sanPham.DanhMucID = danhMucID;
                    sanPham.NhaCungCapID = nhaCungCapID;
                    qlkh.SubmitChanges();
                }

                // Cập nhật lại DataGridView
                LoadSanPham();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }


        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanPham.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvSanPham.SelectedRows[0];
                    var sanPhamID = selectedRow.Cells["SanPhamID"].Value.ToString();

                    // Xóa sản phẩm khỏi cơ sở dữ liệu
                    var sanPham = qlkh.SanPhams.FirstOrDefault(sp => sp.SanPhamID == sanPhamID);
                    if (sanPham != null)
                    {
                        qlkh.SanPhams.DeleteOnSubmit(sanPham);
                        qlkh.SubmitChanges();
                    }

                    // Cập nhật lại DataGridView
                    LoadSanPham();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa.");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Giá trị tham số vượt quá phạm vi cho phép: {ex.Message}");
            }
        }
    }
}