INSERT INTO DanhMuc (DanhMucID, TenDanhMuc) VALUES ('DM01', N'Điện thoại');
INSERT INTO DanhMuc (DanhMucID, TenDanhMuc) VALUES ('DM02', N'Máy tính bảng');

INSERT INTO NhaCungCap (NhaCungCapID, TenNhaCungCap, DiaChi, ThanhPho, DienThoai) 
VALUES ('NCC01', N'Công ty A', N'123 Đường A', N'Hà Nội', N'0123456789');
INSERT INTO NhaCungCap (NhaCungCapID, TenNhaCungCap, DiaChi, ThanhPho, DienThoai) 
VALUES ('NCC02', N'Công ty B', N'456 Đường B', N'Hồ Chí Minh', N'0987654321');


INSERT INTO KhachHang (KhachHangID, TenKH, DiaChi, ThanhPho, SDT) 
VALUES ('KH01', N'Nguyễn Văn A', N'789 Đường C', N'Đà Nẵng', N'0912345678');
INSERT INTO KhachHang (KhachHangID, TenKH, DiaChi, ThanhPho, SDT) 
VALUES ('KH02', N'Trần Thị B', N'321 Đường D', N'Hải Phòng', N'0901234567');


INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP01', N'Iphone 12', 50, 10, 20000.000, 'DM01', 'NCC01');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP02', N'Samsung Galaxy Tab', 30, 5, 15000.000, 'DM02', 'NCC02');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP03', N'Xiaomi Mi 11', 45, 8, 18000.000, 'DM01', 'NCC01');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP04', N'Oppo Reno 5', 40, 7, 16000.000, 'DM01', 'NCC01');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP05', N'Sony Xperia 10', 25, 3, 22000.000, 'DM02', 'NCC02');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP06', N'Samsung Galaxy A52', 35, 6, 17000.000, 'DM02', 'NCC02');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP07', N'LG V60 ThinQ', 15, 2, 30000.000, 'DM01', 'NCC01');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP08', N'Huawei P40', 20, 3, 25000.000, 'DM02', 'NCC02');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP09', N'OnePlus 9', 30, 5, 28000.000, 'DM01', 'NCC01');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP10', N'Vivo X60', 25, 4, 22000.000, 'DM02', 'NCC02');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP11', N'Samsung Galaxy M32', 40, 7, 15000.000, 'DM01', 'NCC01');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP12', N'Xiaomi Redmi Note 10', 50, 10, 12000.000, 'DM02', 'NCC02');




INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP13', N'Vivo Y20', 3, 5, 8000.000, 'DM01', 'NCC01');
INSERT INTO SanPham (SanPhamID, TenSanPham, SoLuong, SanPhamToiThieu, GiaBan, DanhMucID, NhaCungCapID) 
VALUES ('SP14', N'OPPO A54', 6, 10, 9500.000, 'DM02', 'NCC02');



INSERT INTO VaiTro (VaiTroID, TenVaiTro) 
VALUES ('NHAPKHO', N'Nhân viên nhập kho');
INSERT INTO VaiTro (VaiTroID, TenVaiTro) 
VALUES ('XUATKHO', N'Nhân viên xuất kho');
INSERT INTO VaiTro (VaiTroID, TenVaiTro) 
VALUES ('QUANLY', N'Nhân viên quản lý');


INSERT INTO NhanVien (NhanVienID, MatKhau, HoTen, DienThoai, VaiTroID) 
VALUES ('NV01', '123456', N'Nguyễn Văn Nam', N'0981234567', 'NHAPKHO');
INSERT INTO NhanVien (NhanVienID, MatKhau, HoTen, DienThoai, VaiTroID) 
VALUES ('NV02', 'abcdef', N'Trần Thị Lan', N'0977654321', 'XUATKHO');
INSERT INTO NhanVien (NhanVienID, MatKhau, HoTen, DienThoai, VaiTroID) 
VALUES ('1', '1', N'ADMIN', N'0977654321', 'QUANLY');

INSERT INTO PhieuNhap (PhieuNhapID, NhaCungCapID, NhanVienID, NgayNhap, GhiChu, TongTien) 
VALUES ('PN01', 'NCC01', 'NV01', GETDATE(), N'Nhập hàng đầu tháng', 100000.00);
INSERT INTO PhieuNhap (PhieuNhapID, NhaCungCapID, NhanVienID, NgayNhap, GhiChu, TongTien) 
VALUES ('PN02', 'NCC02', 'NV02', GETDATE(), N'Nhập hàng giữa tháng', 75000.00);
INSERT INTO PhieuNhap (PhieuNhapID, NhaCungCapID, NhanVienID, NgayNhap, GhiChu, TongTien) 
VALUES ('PN03', 'NCC01', 'NV01', GETDATE(), N'Nhập hàng cuối tháng', 90000.00);
INSERT INTO PhieuNhap (PhieuNhapID, NhaCungCapID, NhanVienID, NgayNhap, GhiChu, TongTien) 
VALUES ('PN04', 'NCC02', 'NV01', GETDATE(), N'Nhập hàng đặc biệt', 120000.00);
INSERT INTO PhieuNhap (PhieuNhapID, NhaCungCapID, NhanVienID, NgayNhap, GhiChu, TongTien) 
VALUES ('PN05', 'NCC01', 'NV02', GETDATE(), N'Nhập hàng theo đơn đặt', 65000.00);
INSERT INTO PhieuNhap (PhieuNhapID, NhaCungCapID, NhanVienID, NgayNhap, GhiChu, TongTien) 
VALUES ('PN06', 'NCC02', 'NV01', GETDATE(), N'Nhập hàng khẩn cấp', 85000.00);


INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN01', 'SP01', 20, 15000.00);
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN01', 'SP02', 10, 12000.00);
-- Chi tiết cho PN02
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN02', 'SP03', 15, 20000.00);
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN02', 'SP04', 5, 10000.00);
-- Chi tiết cho PN03
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN03', 'SP01', 25, 15000.00);
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN03', 'SP05', 10, 18000.00);
-- Chi tiết cho PN04
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN04', 'SP06', 20, 30000.00);
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN04', 'SP02', 10, 12000.00);
-- Chi tiết cho PN05
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN05', 'SP07', 30, 22000.00);
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN05', 'SP08', 10, 15000.00);
-- Chi tiết cho PN06
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN06', 'SP09', 5, 40000.00);
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN06', 'SP10', 15, 20000.00);


INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien) 
VALUES ('PX01', 'KH01', 'NV02', GETDATE(), N'Xuất hàng cho khách hàng A', 50000.000);
INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien) 
VALUES ('PX02', 'KH02', 'NV01', GETDATE(), N'Xuất hàng cho khách hàng B', 75000.000);
INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien) 
VALUES ('PX03', 'KH01', 'NV01', GETDATE(), N'Xuất hàng cho khách hàng C', 92000.000);
INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien) 
VALUES ('PX04', 'KH02', 'NV02', GETDATE(), N'Xuất hàng cho khách hàng D', 60000.000);
INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien) 
VALUES ('PX05', 'KH01', 'NV01', GETDATE(), N'Xuất hàng cho khách hàng E', 85000.000);
INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien) 
VALUES ('PX06', 'KH02', 'NV01', GETDATE(), N'Xuất hàng cho khách hàng F', 47000.000);

delete PhieuXuat





INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX01', 'SP01', 5, 18000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX01', 'SP02', 3, 13000.000);
-- Chi tiết cho PX02
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX02', 'SP03', 4, 20000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX02', 'SP04', 2, 25000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX02', 'SP05', 3, 15000.000);
-- Chi tiết cho PX03
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX03', 'SP01', 5, 18000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX03', 'SP02', 4, 13000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX03', 'SP06', 2, 25000.000);
-- Chi tiết cho PX04
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX04', 'SP07', 3, 22000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX04', 'SP08', 1, 38000.000);
-- Chi tiết cho PX05
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX05', 'SP09', 6, 14000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX05', 'SP10', 3, 25000.000);
-- Chi tiết cho PX06
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX06', 'SP11', 5, 5400.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX06', 'SP12', 2, 16000.000);

delete ChiTietPhieuXuat





