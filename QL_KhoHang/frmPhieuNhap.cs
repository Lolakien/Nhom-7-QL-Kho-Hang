using QL_KhoHang.MachineLearning;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using QL_KhoHang.MiniForm;

namespace QL_KhoHang
{
    public partial class frmPhieuNhap : Form
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        public frmPhieuNhap()
        {
            InitializeComponent();
            LoadPhieuNhap();
            LoadNhaCungCap();
            CheckLowStockProducts();

            txtMaphieu.ReadOnly = true;
            txtMaNV.ReadOnly = true;
            txtTongTien.ReadOnly = true;
         
        }
        List<SanPham> ListLowStockProducts = new List<SanPham>();
        private void CheckLowStockProducts()
        {
            var lowStockProducts = qlkh.SanPhams
                .Where(sp => sp.SoLuong <= sp.SanPhamToiThieu) // Kiểm tra số lượng tồn kho so với số lượng tối thiểu
                .Select(sp => new
                {
                    sp.SanPhamID,
                    sp.TenSanPham,
                    sp.SoLuong,
                    sp.SanPhamToiThieu // Lấy thêm thông tin số lượng tối thiểu
                }).ToList();
            if (lowStockProducts.Any())
            {
                dgvSapHet.Columns.Add("SanPhamID", "Mã sản phẩm");
                dgvSapHet.Columns.Add("TenSanPham", "Tên sản phẩm");
                dgvSapHet.Columns.Add("SoLuong", "Số lượng hiện tại");
                dgvSapHet.Columns[0].Width = 50;
                dgvSapHet.Columns[1].Width = 160;
                dgvSapHet.HeaderBackColor = Color.Red;
                foreach (var product in lowStockProducts)
                {
                    dgvSapHet.Rows.Add(
                        product.SanPhamID,
                        product.TenSanPham,
                        product.SoLuong
                    );
                }
            }
        }
        private void LoadDataAndTrainModel()
        {
            // Lấy dữ liệu từ PhieuXuat và ChiTietPhieuXuat
            var phieuXuatData = qlkh.PhieuXuats
     .Join(qlkh.ChiTietPhieuXuats,
           px => px.PhieuXuatID,
           ctx => ctx.PhieuXuatID,
           (px, ctx) => new
           {
               px.NgayXuat,
               ctx.SanPhamID,
               SoLuong = (float)ctx.SoLuong // Chuyển đổi từ int sang float
           })
     .GroupBy(x => new { x.NgayXuat.Year, x.NgayXuat.Month, x.SanPhamID })
     .Select(g => new PhieuXuatData
     {
         Nam = g.Key.Year,
         Thang = g.Key.Month,
         SanPhamID = g.Key.SanPhamID,
         SoLuong = g.Sum(x => x.SoLuong)
     })
     .ToList();

            // Kiểm tra xem dữ liệu có tồn tại không
            if (phieuXuatData.Any())
            {
                // Khởi tạo và huấn luyện mô hình dự đoán
                var predictor = new DemandPredictor();
                predictor.TrainModel(phieuXuatData);

                dgvDuDoan.Columns.Add("SanPhamID", "Mã sản phẩm");
                dgvDuDoan.Columns.Add("TenSanPham", "Tên sản phẩm");
                dgvDuDoan.Columns.Add("SoLuongDuDoan", "Số lượng dự đoán");

                // Lặp qua các sản phẩm trong dgvSapHet và thực hiện dự đoán
                foreach (DataGridViewRow row in dgvSapHet.Rows)
                {
                    if (row.Cells["SanPhamID"].Value != null)
                    {
                        string sanPhamID = row.Cells["SanPhamID"].Value.ToString();
                        string tenSanPham = row.Cells["TenSanPham"].Value.ToString();

                        // Thực hiện dự đoán
                        float soLuongDuDoan = predictor.Predict(DateTime.Now.Year, DateTime.Now.Month + 1, sanPhamID);

                        dgvDuDoan.Rows.Add(sanPhamID, tenSanPham, soLuongDuDoan);
                    }
                }

            }
            else
            {
                MessageBox.Show("Không có dữ liệu để huấn luyện mô hình.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
 
        public void LoadPhieuNhap()
        {
            var phieuNhapList = qlkh.PhieuNhaps.Select(p => new
            {
                PhieuNhapID = p.PhieuNhapID,
                NhaCungCapID = p.NhaCungCapID,
                NhanVienID = p.NhanVienID,
                NgayNhap = p.NgayNhap,
                GhiChu = p.GhiChu,
                TongTien = p.TongTien
            }).ToList();

            dtG_phieuNhap.DataSource = phieuNhapList;
            dtG_phieuNhap.Columns["PhieuNhapID"].HeaderText = "Phiếu Nhập";
            dtG_phieuNhap.Columns["NhaCungCapID"].HeaderText = "Nhà Cung Cấp";
            dtG_phieuNhap.Columns["NhanVienID"].HeaderText = "Nhân Viên";
            dtG_phieuNhap.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
            dtG_phieuNhap.Columns["GhiChu"].HeaderText = "Ghi Chú";
            dtG_phieuNhap.Columns["TongTien"].HeaderText = "Tổng Tiền";
        }

        private void LoadChiTietPhieuNhap(string phieuNhapID)
        {
            var chiTietList = qlkh.ChiTietPhieuNhaps
                .Where(ct => ct.PhieuNhapID == phieuNhapID)
                .Select(ct => new
                {
                    SanPhamID = ct.SanPhamID,
                    TenSanPham = ct.SanPham.TenSanPham,
                    SoLuong = ct.SoLuong,
                    GiaNhap = ct.GiaNhap
                }).ToList();

            dtG_ct_phieunhap.DataSource = chiTietList;

            dtG_ct_phieunhap.Columns["SanPhamID"].HeaderText = "Sản Phẩm";
            dtG_ct_phieunhap.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dtG_ct_phieunhap.Columns["SoLuong"].HeaderText = "Số Lượng";
            dtG_ct_phieunhap.Columns["GiaNhap"].HeaderText = "Giá Nhập";
        }

        private void LoadNhaCungCap()
        {
            var nhaCungCapList = qlkh.NhaCungCaps.ToList();
            cbbNCC.DataSource = nhaCungCapList;
            cbbNCC.DisplayMember = "TenNhaCungCap";
            cbbNCC.ValueMember = "NhaCungCapID";
        }

        private void dtG_ct_phieunhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnInPhieuNhap_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            PhieuNhap ph = qlkh.PhieuNhaps.FirstOrDefault(t => t.PhieuNhapID == txtMaphieu.Text);

            var cts = from ct in qlkh.ChiTietPhieuNhaps
                      where ct.PhieuNhapID == ph.PhieuNhapID
                      select ct;

            string path = string.Empty;
            excel.ExportPhieuNhap(ph, ref path, false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Ensure a PhieuNhap is selected
            if (string.IsNullOrEmpty(txtMaphieu.Text))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected PhieuNhapID
            string phieuNhapID = txtMaphieu.Text;

            // Confirm deletion
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu nhập này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Delete ChiTietPhieuNhaps associated with the selected PhieuNhap
                var chiTietPhieuNhaps = qlkh.ChiTietPhieuNhaps.Where(ct => ct.PhieuNhapID == phieuNhapID).ToList();
                foreach (var chiTiet in chiTietPhieuNhaps)
                {
                    qlkh.ChiTietPhieuNhaps.DeleteOnSubmit(chiTiet);
                }

                // Delete the selected PhieuNhap
                var phieuNhap = qlkh.PhieuNhaps.FirstOrDefault(p => p.PhieuNhapID == phieuNhapID);
                if (phieuNhap != null)
                {
                    qlkh.PhieuNhaps.DeleteOnSubmit(phieuNhap);
                }

                // Submit changes to the database
                qlkh.SubmitChanges();

                // Refresh the DataGridView
                LoadPhieuNhap();
                LoadChiTietPhieuNhap(null);
                MessageBox.Show("Xóa phiếu nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_them_ex_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "D:\\Do AN\\Nhom-7-QL-Kho-Hang";
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
                openFileDialog.Title = "Chọn file Excel để nhập";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Tạo một phiên bản ứng dụng Excel
                    var excelApp = new Excel.Application();
                    var workbook = excelApp.Workbooks.Open(filePath);
                    var worksheet = (Excel.Worksheet)workbook.Sheets[1];

                    // Đọc thông tin phiếu nhập
                    string tenNhaCungCap = worksheet.Cells[10, 2].Value.ToString();
                    decimal tongTien = decimal.Parse(worksheet.Cells[16, 6].Value.ToString().Replace(".", "").Replace(",", ".")); // Định dạng lại tiền
                    string nhanVienID = Authentication.getUserID();

                    // Tìm ID nhà cung cấp
                    string nhaCungCapID = qlkh.NhaCungCaps
                        .Where(ncc => ncc.TenNhaCungCap == tenNhaCungCap)
                        .Select(ncc => ncc.NhaCungCapID)
                        .FirstOrDefault();

                    if (string.IsNullOrEmpty(nhaCungCapID))
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp với tên: " + tenNhaCungCap, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Tạo một phiếu nhập mới
                    string maPN = GetNextPhieuNhapID();
                    var phieuNhap = new PhieuNhap
                    {
                        PhieuNhapID = maPN,
                        NhaCungCapID = nhaCungCapID,
                        NhanVienID = nhanVienID,
                        NgayNhap = DateTime.Now,
                        GhiChu = "",
                        TongTien = tongTien
                    };

                    // Chèn phiếu nhập mới vào cơ sở dữ liệu
                    qlkh.PhieuNhaps.InsertOnSubmit(phieuNhap);
                    qlkh.SubmitChanges();

                    // Đọc chi tiết phiếu nhập
                    decimal tongThanhTien = 0; // Khởi tạo biến tổng thành tiền
                    for (int row = 14; row <= worksheet.UsedRange.Rows.Count; row++)
                    {
                        var sanPhamIDCell = worksheet.Cells[row, 2].Value;
                        var soLuongCell = worksheet.Cells[row, 4].Value;
                        var giaNhapCell = worksheet.Cells[row, 5].Value;

                        if (sanPhamIDCell == null || soLuongCell == null || giaNhapCell == null)
                        {
                            break; // Thoát vòng lặp nếu có bất kỳ ô nào trống
                        }

                        string sanPhamID = sanPhamIDCell.ToString();
                        int soLuong = int.Parse(soLuongCell.ToString());
                        decimal giaNhap = decimal.Parse(giaNhapCell.ToString());

                        // Tính thành tiền cho sản phẩm
                        decimal thanhTien = soLuong * giaNhap;
                        tongThanhTien += thanhTien; // Cộng dồn thành tiền

                        var chiTiet = new ChiTietPhieuNhap
                        {
                            PhieuNhapID = maPN,
                            SanPhamID = sanPhamID,
                            SoLuong = soLuong,
                            GiaNhap = giaNhap
                        };

                        // Chèn chi tiết phiếu nhập vào cơ sở dữ liệu
                        qlkh.ChiTietPhieuNhaps.InsertOnSubmit(chiTiet);
                    }

                    // Cập nhật tổng tiền cho phiếu nhập
                    phieuNhap.TongTien = tongThanhTien;
                    qlkh.SubmitChanges(); // Lưu thay đổi

                    LoadPhieuNhap(); // Làm mới danh sách phiếu nhập 
                    MessageBox.Show("Nhập phiếu nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Đóng workbook và ứng dụng Excel
                    workbook.Close(false);
                    excelApp.Quit();
                }
            }
        }

        private string GetNextPhieuNhapID()
        {
            // Lấy tất cả các mã phiếu nhập hiện có
            var phieuNhaps = qlkh.PhieuNhaps.Select(p => p.PhieuNhapID).ToList();

            // Tìm mã phiếu nhập cuối cùng
            int maxId = 0;
            foreach (var pn in phieuNhaps)
            {
                if (int.TryParse(pn.Substring(2), out int id)) // Lấy phần số sau "PN"
                {
                    if (id > maxId)
                    {
                        maxId = id;
                    }
                }
            }

            // Tạo mã phiếu nhập tiếp theo
            maxId++; // Tăng lên 1
            return "PN" + maxId.ToString("D2"); // Định dạng với 2 chữ số
        }

        private void frmPhieuNhap_Load(object sender, EventArgs e)
        {
      
        }

        private void dtG_phieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dtG_phieuNhap.Rows[e.RowIndex];
                var phieuNhapID = selectedRow.Cells["PhieuNhapID"].Value.ToString();
                var nhaCungCapID = selectedRow.Cells["NhaCungCapID"].Value.ToString();
                var nhanVienID = selectedRow.Cells["NhanVienID"].Value.ToString();
                var ngayNhap = Convert.ToDateTime(selectedRow.Cells["NgayNhap"].Value);
                var ghiChu = selectedRow.Cells["GhiChu"].Value.ToString();
                var tongTien = Convert.ToDecimal(selectedRow.Cells["TongTien"].Value);

                txtMaphieu.Text = phieuNhapID;
                txtMaNV.Text = nhanVienID;
                txtGhichu.Text = ghiChu;
                txtTongTien.Text = tongTien.ToString();
                dtpNgayNhap.Value = ngayNhap;

                var nhanVien = qlkh.NhanViens.FirstOrDefault(nv => nv.NhanVienID == nhanVienID);
                if (nhanVien != null)
                {
                    txtMaNV.Text = nhanVien.HoTen;
                }
                cbbNCC.SelectedValue = nhaCungCapID;

                LoadChiTietPhieuNhap(phieuNhapID);
            }
        }

        private void btnDuDoan_Click(object sender, EventArgs e)
        {
            LoadDataAndTrainModel();
        }

        private void btnNCCSetting_Click(object sender, EventArgs e)
        {
            NCCForm f = new NCCForm();
            f.ShowDialog();
        }
    }
}