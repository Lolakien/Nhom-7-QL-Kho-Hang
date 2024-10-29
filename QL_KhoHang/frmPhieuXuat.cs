using System;
using System.Linq;
using System.Windows.Forms;

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
            var phieuXuatList = qlkh.PhieuXuats.Select(p => new
            {
                PhieuXuatID = p.PhieuXuatID,
                KhachHangID = p.KhachHangID,
                NhanVienID = p.NhanVienID,
                NgayXuat = p.NgayXuat,
                GhiChu = p.GhiChu,
                TongTien = p.TongTien
            }).ToList();

            dtG_phieuXuat.DataSource = phieuXuatList;

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
                var tongTien = Convert.ToDecimal(selectedRow.Cells["TongTien"].Value);

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
                var giaXuat = Convert.ToDecimal(selectedRow.Cells["GiaXuat"].Value);

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
    }
}