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


INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN01', 'SP01', 20, 15000.00);
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap) 
VALUES ('PN01', 'SP02', 10, 12000.00);


INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien) 
VALUES ('PX01', 'KH01', 'NV02', GETDATE(), N'Xuất hàng cho khách hàng A', 50000.000);

INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX01', 'SP01', 5, 18000.000);
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat) 
VALUES ('PX01', 'SP02', 3, 13000.000);




