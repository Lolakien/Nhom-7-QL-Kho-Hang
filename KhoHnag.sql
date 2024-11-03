CREATE DATABASE QL_HangHoa;
GO

USE QL_HangHoa;
GO

-- Tạo bảng DanhMuc
CREATE TABLE DanhMuc (
    DanhMucID VARCHAR(20) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL
);

-- Tạo bảng NhaCungCap
CREATE TABLE NhaCungCap (
    NhaCungCapID VARCHAR(20) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(100) NOT NULL,
	DiaChi NVARCHAR (MAX),
	ThanhPho NVARCHAR(MAX),
    DienThoai NVARCHAR(15)
);


CREATE TABLE KhachHang(
	KhachHangID INT IDENTITY(1,1) PRIMARY KEY,
	TenKH NVARCHAR(MAX),
	DiaChi NVARCHAR (MAX),
	ThanhPho NVARCHAR(MAX),
	SDT NVARCHAR(20),


);


-- Tạo bảng SanPham với trường SanPhamToiThieu
CREATE TABLE SanPham (
    SanPhamID VARCHAR(20) PRIMARY KEY,
    TenSanPham NVARCHAR(100) NOT NULL,
    SoLuong INT NOT NULL,
    SanPhamToiThieu INT NOT NULL DEFAULT 0,
    GiaBan DECIMAL(10, 2) NOT NULL,
    DanhMucID  VARCHAR(20),
    NhaCungCapID  VARCHAR(20),
    FOREIGN KEY (DanhMucID) REFERENCES DanhMuc(DanhMucID),
    FOREIGN KEY (NhaCungCapID) REFERENCES NhaCungCap(NhaCungCapID)
);

-- Tạo bảng NguoiDung
CREATE TABLE VaiTro (
    VaiTroID VARCHAR(50) PRIMARY KEY,
    TenVaiTro NVARCHAR(50),
	
);

-- Tạo bảng NhanVien
CREATE TABLE NhanVien (
    NhanVienID VARCHAR(20) PRIMARY KEY,
	MatKhau char(10),
    HoTen NVARCHAR(100) NOT NULL,
    DienThoai NVARCHAR(15),
	VaiTroID VARCHAR(50),
	FOREIGN KEY (VaiTroID) REFERENCES VaiTro(VaiTroID),

);

-- Tạo bảng NhapKho
CREATE TABLE PhieuNhap (
    PhieuNhapID VARCHAR(20) PRIMARY KEY,
    NhaCungCapID VARCHAR(20),
    NhanVienID VARCHAR(20),
    NgayNhap DATETIME DEFAULT GETDATE() NOT NULL,
    GhiChu NVARCHAR(255),
	TongTien DECIMAL(10, 2) NOT NULL DEFAULT 0,
    FOREIGN KEY (NhaCungCapID) REFERENCES NhaCungCap(NhaCungCapID),
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID)
);

-- Tạo bảng ChiTietPhieuNhap
CREATE TABLE ChiTietPhieuNhap (
    ChiTietPhieuNhapID INT PRIMARY KEY IDENTITY(1,1),
    PhieuNhapID VARCHAR(20) ,
    SanPhamID VARCHAR(20),
    SoLuong INT NOT NULL,
    GiaNhap DECIMAL(10, 2) NOT NULL,
	TongTien AS (SoLuong * GiaNhap),
    FOREIGN KEY (PhieuNhapID) REFERENCES PhieuNhap(PhieuNhapID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

-- Tạo bảng XuatKho
CREATE TABLE PhieuXuat (
    PhieuXuatID VARCHAR(20) PRIMARY KEY,
    KhachHangID INT,
    NhanVienID  VARCHAR(20),
    NgayXuat DATETIME NOT NULL DEFAULT GETDATE(),
    GhiChu NVARCHAR(255),
	TongTien DECIMAL(10, 2) NOT NULL DEFAULT 0,
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID),
	FOREIGN KEY (KhachHangID) REFERENCES KhachHang(KhachHangID),
);

-- Tạo bảng ChiTietPhieuXuat
CREATE TABLE ChiTietPhieuXuat (
    PhieuXuatID VARCHAR(20),
    SanPhamID VARCHAR(20),
    SoLuong INT NOT NULL,
    GiaXuat DECIMAL(10, 2) NOT NULL,
	TongTien AS (SoLuong * GiaXuat),
	PRIMARY KEY (PhieuXuatID, SanPhamID),
    FOREIGN KEY (PhieuXuatID) REFERENCES PhieuXuat(PhieuXuatID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

drop ChiTietPhieuX 
CREATE TABLE ViTriKho (
    ViTriID VARCHAR(20),
    DanhMucID VARCHAR(20),
    SanPhamID VARCHAR(20),
    SoLuong INT NOT NULL DEFAULT 0,
    SoLuongToiDa INT NOT NULL DEFAULT 10,
    
    PRIMARY KEY (ViTriID, DanhMucID),
    
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID),
    FOREIGN KEY (DanhMucID) REFERENCES DanhMuc(DanhMucID)
);

GO 


-- Insert into NhaCungCap first
INSERT INTO NhaCungCap (NhaCungCapID, TenNhaCungCap, DiaChi, ThanhPho, DienThoai)
VALUES 
    ('NCC001', N'Công ty Điện tử Việt', N'123 Đường ABC', N'TP. Hồ Chí Minh', '0909123456'),
    ('NCC002', N'Công ty Công nghệ Hà Nội', N'456 Đường XYZ', N'Hà Nội', '0912345678');

-- Insert into DanhMuc
INSERT INTO DanhMuc (DanhMucID, TenDanhMuc)
VALUES 
    ('DM001', N'Điện thoại di động'),
    ('DM002', N'Laptop'),
    ('DM003', N'Phụ kiện điện tử');

    
-- Insert into VaiTro
INSERT INTO VaiTro (VaiTroID, TenVaiTro)
VALUES 
    ('QUANLY', N'Nhân viện Quản lý kho'),
    ('ADMIN', N'Quản trị viên'),
    ('XUATKHO', N'Nhân viện nhập kho'),
    ('NHAPKHO', N'Nhân viên xuất kho');

-- Insert into NhanVien with valid VaiTroID
INSERT INTO NhanVien (NhanVienID, MatKhau, HoTen, DienThoai, VaiTroID)
VALUES 
    ('NV001', '123456', N'Nguyễn Văn A', '0905123456', 'ADMIN'),
    ('NV002', 'abcdef', N'Lê Thị B', '0912345678', 'QUANLY');

-- Insert into KhachHang
INSERT INTO KhachHang (TenKH, DiaChi, ThanhPho, SDT)
VALUES 
    (N'Triệu Văn', N'789 Đường DEF', N'TP. Đà Nẵng', '0933456789'),
    (N'Lê Bị', N'321 Đường GHI', N'Hải Phòng', '0944567890');

-- Insert into PhieuNhap with existing NhaCungCap and NhanVien
INSERT INTO PhieuNhap (PhieuNhapID, NhaCungCapID, NhanVienID, NgayNhap, GhiChu, TongTien)
VALUES 
    ('PN001', 'NCC001', 'NV001', '2024-01-15', N'Nhập hàng tháng 1', 10000000),
    ('PN002', 'NCC002', 'NV002', '2024-02-10', N'Nhập hàng tháng 2', 15000000),
    ('PN003', 'NCC001', 'NV001', '2024-03-20', N'Nhập hàng tháng 3', 12000000),
    ('PN004', 'NCC002', 'NV002', '2024-04-05', N'Nhập hàng tháng 4', 8000000),
    ('PN005', 'NCC001', 'NV001', '2024-05-12', N'Nhập hàng tháng 5', 20000000);

-- Insert into ChiTietPhieuNhap with existing PhieuNhap and SanPham
INSERT INTO ChiTietPhieuNhap (PhieuNhapID, SanPhamID, SoLuong, GiaNhap)
VALUES 
    ('PN001', 'SP001', 20, 18000000), 
    ('PN001', 'SP003', 50, 450000),
    ('PN002', 'SP002', 10, 42000000),
    ('PN002', 'SP003', 100, 450000),
    ('PN003', 'SP001', 30, 18000000);

-- Insert into PhieuXuat with existing KhachHang and NhanVien
INSERT INTO PhieuXuat (PhieuXuatID, KhachHangID, NhanVienID, NgayXuat, GhiChu, TongTien)
VALUES 
    ('PX001', 1, 'NV002', '2024-01-18', N'Xuất hàng cho khách tháng 1', 4000000),
    ('PX002', 2, 'NV001', '2024-02-15', N'Xuất hàng cho khách tháng 2', 5000000),
    ('PX003', 1, 'NV002', '2024-03-10', N'Xuất hàng cho khách tháng 3', 7000000),
    ('PX004', 2, 'NV001', '2024-04-25', N'Xuất hàng cho khách tháng 4', 10000000),
    ('PX005', 1, 'NV002', '2024-05-05', N'Xuất hàng cho khách tháng 5', 3000000);

-- Insert into ChiTietPhieuXuat with existing PhieuXuat and SanPham
INSERT INTO ChiTietPhieuXuat (PhieuXuatID, SanPhamID, SoLuong, GiaXuat)
VALUES 
    ('PX001', 'SP001', 2, 19500000),
    ('PX001', 'SP003', 10, 500000),
    ('PX002', 'SP002', 1, 44500000),
    ('PX002', 'SP003', 20, 500000),
    ('PX003', 'SP001', 3, 19500000);










---- Tạo trigger để đảm bảo tổng số lượng ở các vị trí bằng số lượng trong bảng sản phẩm
--CREATE TRIGGER trg_SoLuongViTri
--ON ViTriKho
--AFTER INSERT, UPDATE, DELETE
--AS
--BEGIN
--    DECLARE @SanPhamID INT;

--    -- Lấy SanPhamID từ bảng ViTriKho
--    SELECT @SanPhamID = SanPhamID FROM inserted;

--    -- Cập nhật lại số lượng sản phẩm trong bảng SanPham dựa trên tổng số lượng ở các vị trí
--    UPDATE SanPham
--    SET SoLuong = (
--        SELECT SUM(SoLuong) 
--        FROM ViTriKho 
--        WHERE SanPhamID = @SanPhamID
--    )
--    WHERE SanPhamID = @SanPhamID;
--END;
--GO




---- Tạo trigger để trừ số lượng ở các vị trí khi xuất kho
--drop TRIGGER trg_TruSoLuongXuatKho
--ON ChiTietPhieuXuat
--AFTER INSERT
--AS
--BEGIN
--    DECLARE @SanPhamID INT;
--    DECLARE @SoLuong INT;

--    -- Lấy SanPhamID và SoLuong từ bảng ChiTietPhieuXuat
--    SELECT @SanPhamID = SanPhamID, @SoLuong = SoLuong FROM inserted;

--    -- Giảm số lượng ở các vị trí
--    UPDATE ViTriKho
--    SET SoLuong = SoLuong - @SoLuong
--    WHERE SanPhamID = @SanPhamID AND SoLuong >= @SoLuong;

--    -- Cập nhật lại số lượng sản phẩm trong bảng SanPham
--    UPDATE SanPham
--    SET SoLuong = (
--        SELECT SUM(SoLuong) 
--        FROM ViTriKho 
--        WHERE SanPhamID = @SanPhamID
--    )
--    WHERE SanPhamID = @SanPhamID;
--END;
--GO


---- Tạo trigger để cập nhật TổngTiền trong NhapKho
--CREATE TRIGGER trg_CapNhatTongTienNhapKho
--ON NhapKho
--AFTER INSERT
--AS
--BEGIN
--    DECLARE @NhapKhoID INT;

--    -- Lấy NhapKhoID từ bảng NhapKho
--    SELECT @NhapKhoID = NhapKhoID FROM inserted;

--    -- Cập nhật TổngTiền trong NhapKho
--    UPDATE NhapKho
--    SET TongTien = (
--        SELECT SUM(ThanhTien)
--        FROM ChiTietPhieuNhap
--        WHERE NhapKhoID = @NhapKhoID
--    )
--    WHERE NhapKhoID = @NhapKhoID;
--END;
--GO

---- Tạo trigger để cập nhật TổngTiền trong XuatKho
--CREATE TRIGGER trg_CapNhatTongTienXuatKho
--ON XuatKho
--AFTER INSERT
--AS
--BEGIN
--    DECLARE @XuatKhoID INT;

--    -- Lấy XuatKhoID từ bảng XuatKho
--    SELECT @XuatKhoID = XuatKhoID FROM inserted;

--    -- Cập nhật TổngTiền trong XuatKho
--    UPDATE XuatKho
--    SET TongTien = (
--        SELECT SUM(ThanhTien)
--        FROM ChiTietPhieuXuat
--        WHERE XuatKhoID = @XuatKhoID
--    )
--    WHERE XuatKhoID = @XuatKhoID;
--END;
--GO