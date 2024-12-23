﻿using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QL_KhoHang.MiniForm;
namespace QL_KhoHang
{
    public partial class frmPhieuXuat : Form
    {
        QL_KhoHangDataContext qlkh = new QL_KhoHangDataContext();

        public frmPhieuXuat()
        {
            InitializeComponent();
            LoadPhieuXuat();
            LoadKhachHang();

            txtMaphieu.ReadOnly = true;
            txtMaNV.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txt_ct_MaPhieu.ReadOnly = true;
        }

        public void LoadPhieuXuat()
        {
            var phieuXuatList = qlkh.PhieuXuats.ToList();

            // Gán danh sách cho DataGridView
            dtG_phieuXuat.DataSource = phieuXuatList;

            // Đặt tiêu đề cho các cột
            dtG_phieuXuat.Columns["PhieuXuatID"].HeaderText = "Phiếu Xuất";
            dtG_phieuXuat.Columns["KhachHangID"].HeaderText = "Khách Hàng";
            dtG_phieuXuat.Columns["NhanVienID"].HeaderText = "Nhân Viên";
            dtG_phieuXuat.Columns["NgayXuat"].HeaderText = "Ngày Xuất";
            dtG_phieuXuat.Columns["GhiChu"].HeaderText = "Ghi Chú";
            dtG_phieuXuat.Columns["TongTien"].HeaderText = "Tổng Tiền";
        }

        private void dtG_phieuXuat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            {
                var selectedRow = dtG_phieuXuat.Rows[e.RowIndex];
                var phieuXuatID = selectedRow.Cells["PhieuXuatID"].Value.ToString();
                var khachHangID = selectedRow.Cells["KhachHangID"].Value;
                var nhanVienID = selectedRow.Cells["NhanVienID"].Value.ToString();
                var ngayXuat = Convert.ToDateTime(selectedRow.Cells["NgayXuat"].Value);
                var ghiChu = selectedRow.Cells["GhiChu"].Value.ToString();
                var tongTien = Convert.ToDouble(selectedRow.Cells["TongTien"].Value);

                txtMaphieu.Text = phieuXuatID;
                txtMaNV.Text = nhanVienID;
                txtGhichu.Text = ghiChu;
                txtTongTien.Text = tongTien.ToString();
                dtpNgayXuat.Value = ngayXuat;

                var nhanVien = qlkh.NhanViens.FirstOrDefault(nv => nv.NhanVienID == nhanVienID);
                if (nhanVien != null)
                {
                    txtMaNV.Text = nhanVien.HoTen;
                }
                cbbKH.SelectedValue = khachHangID;

                LoadChiTietPhieuXuat(phieuXuatID);
            }
        }

        private void LoadChiTietPhieuXuat(string phieuXuatID)
        {
            var chiTietList = qlkh.ChiTietPhieuXuats
                .Where(ct => ct.PhieuXuatID == phieuXuatID)
                .Select(ct => new
                {
                    SanPhamID = ct.SanPhamID,
                    TenSanPham = ct.SanPham.TenSanPham,
                    SoLuong = ct.SoLuong,
                    GiaXuat = ct.GiaXuat
                }).ToList();

            dtG_ct_phieuxuat.DataSource = chiTietList;

            dtG_ct_phieuxuat.Columns["SanPhamID"].HeaderText = "Sản Phẩm";
            dtG_ct_phieuxuat.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dtG_ct_phieuxuat.Columns["SoLuong"].HeaderText = "Số Lượng";
            dtG_ct_phieuxuat.Columns["GiaXuat"].HeaderText = "Giá Xuất";
        }

        private void LoadKhachHang()
        {
            var khachHangList = qlkh.KhachHangs.ToList();
            cbbKH.DataSource = khachHangList;
            cbbKH.DisplayMember = "TenKH";
            cbbKH.ValueMember = "KhachHangID";
        }

        private void dtG_ct_phieuxuat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dtG_ct_phieuxuat.Rows[e.RowIndex];
                var sanPhamID = selectedRow.Cells["SanPhamID"].Value.ToString();
                var soLuong = Convert.ToInt32(selectedRow.Cells["SoLuong"].Value);
                var giaXuat = Convert.ToDouble(selectedRow.Cells["GiaXuat"].Value);

                txt_ct_MaPhieu.Text = txtMaphieu.Text;
                txt_ct_Sanpham.Text = sanPhamID;
                txt_ct_SoLuong.Text = soLuong.ToString();
                txt_ct_GiaXuat.Text = giaXuat.ToString();

                var sanPham = qlkh.SanPhams.FirstOrDefault(sp => sp.SanPhamID == sanPhamID);
                if (sanPham != null)
                {
                    txt_ct_Sanpham.Text = sanPham.TenSanPham;
                }
            }
        }

        private void btnInPhieuXuat_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            PhieuXuat ph = qlkh.PhieuXuats.Where(t => t.PhieuXuatID == txtMaphieu.Text).FirstOrDefault();

            var cts = from ct in qlkh.ChiTietPhieuXuats
                      where ct.PhieuXuatID == ph.PhieuXuatID
                      select ct;

            string path = string.Empty;
            excel.ExportPhieuXuat(ph, ref path, false); // Corrected method call
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Ensure a PhieuXuat is selected
            if (string.IsNullOrEmpty(txtMaphieu.Text))
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected PhieuXuatID
            string phieuXuatID = txtMaphieu.Text;

            // Confirm deletion
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu xuất này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Delete ChiTietPhieuXuats associated with the selected PhieuXuat
                var chiTietPhieuXuats = qlkh.ChiTietPhieuXuats.Where(ct => ct.PhieuXuatID == phieuXuatID).ToList();
                foreach (var chiTiet in chiTietPhieuXuats)
                {
                    qlkh.ChiTietPhieuXuats.DeleteOnSubmit(chiTiet);
                }

                // Delete the selected PhieuXuat
                var phieuXuat = qlkh.PhieuXuats.FirstOrDefault(p => p.PhieuXuatID == phieuXuatID);
                if (phieuXuat != null)
                {
                    qlkh.PhieuXuats.DeleteOnSubmit(phieuXuat);
                }

                // Submit changes to the database
                qlkh.SubmitChanges();

                // Refresh the DataGridView
                LoadPhieuXuat();
                LoadChiTietPhieuXuat(null);
                MessageBox.Show("Xóa phiếu xuất thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn phiếu xuất để sửa chưa
            if (string.IsNullOrEmpty(txtMaphieu.Text))
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin cần thiết từ các trường nhập liệu
            string phieuXuatID = txtMaphieu.Text;
            string ghiChu = txtGhichu.Text;
            Double tongTien = Double.Parse(txtTongTien.Text); 
            DateTime ngayXuat = dtpNgayXuat.Value;
            string khachHangID = cbbKH.SelectedValue.ToString(); // Lấy giá trị từ ComboBox

            // Tìm NhanVienID từ tên nhân viên
            string tenNhanVien = txtMaNV.Text;
            var nhanVien = qlkh.NhanViens.FirstOrDefault(n => n.HoTen == tenNhanVien);
            string nhanVienID = nhanVien != null ? nhanVien.NhanVienID : null;

            if (nhanVienID == null)
            {
                MessageBox.Show("Không tìm thấy nhân viên với tên đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tìm phiếu xuất cần sửa trong cơ sở dữ liệu
            var phieuXuat = qlkh.PhieuXuats.FirstOrDefault(p => p.PhieuXuatID == phieuXuatID);
            if (phieuXuat != null)
            {
                // Cập nhật thông tin
                phieuXuat.NhanVienID = nhanVienID;
                phieuXuat.GhiChu = ghiChu;
                phieuXuat.TongTien = tongTien;
                phieuXuat.NgayXuat = ngayXuat;
                phieuXuat.KhachHangID = khachHangID; // Cập nhật khách hàng

                // Ghi lại thay đổi vào cơ sở dữ liệu
                qlkh.SubmitChanges();

                // Thông báo thành công và làm mới dữ liệu
                MessageBox.Show("Cập nhật phiếu xuất thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPhieuXuat(); // Làm mới danh sách phiếu xuất
            }
            else
            {
                MessageBox.Show("Không tìm thấy phiếu xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKHSetting_Click(object sender, EventArgs e)
        {
            KHForm f = new KHForm();
            f.ShowDialog();
        }
    }
}