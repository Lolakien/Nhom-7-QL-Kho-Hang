using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            txtMaphieu.ReadOnly = true;
            txtMaNV.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txt_ct_MaPhieu.ReadOnly = true;
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
            if (e.RowIndex >= 0)
            {
                var selectedRow = dtG_ct_phieunhap.Rows[e.RowIndex];
                var sanPhamID = selectedRow.Cells["SanPhamID"].Value.ToString();
                var soLuong = Convert.ToInt32(selectedRow.Cells["SoLuong"].Value);
                var giaNhap = Convert.ToDecimal(selectedRow.Cells["GiaNhap"].Value);

                txt_ct_MaPhieu.Text = txtMaphieu.Text;
                txt_ct_Sanpham.Text = sanPhamID; 
                txt_ct_SoLuong.Text = soLuong.ToString();
                txt_ct_GiaNhap.Text = giaNhap.ToString();

                var sanPham = qlkh.SanPhams.FirstOrDefault(sp => sp.SanPhamID == sanPhamID);
                if (sanPham != null)
                {
                    txt_ct_Sanpham.Text = sanPham.TenSanPham; 
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {

        }

        private void btnInPhieuNhap_Click(object sender, EventArgs e)
        {
            ExcelExport excel = new ExcelExport();
            PhieuNhap ph = qlkh.PhieuNhaps.Where(t => t.PhieuNhapID == txtMaphieu.Text).FirstOrDefault();

            var cts = from ct in qlkh.ChiTietPhieuNhaps
                      where ct.PhieuNhapID == ph.PhieuNhapID
                      select ct;

            string path = string.Empty;
            excel.ExportPhieuNhap(ph, ref path, false);
        }
    }
}