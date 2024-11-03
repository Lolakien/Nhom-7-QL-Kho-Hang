
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
	KhachHangID VARCHAR(20) PRIMARY KEY,
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
    GiaBan DECIMAL(10, 3) NOT NULL,
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
    PhieuNhapID VARCHAR(20) ,
    SanPhamID VARCHAR(20),
	PRIMARY KEY (PhieuNhapID,SanPhamID),
    SoLuong INT NOT NULL,
    GiaNhap DECIMAL(10, 2) NOT NULL,
	TongTien AS (SoLuong * GiaNhap),
    FOREIGN KEY (PhieuNhapID) REFERENCES PhieuNhap(PhieuNhapID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

-- Tạo bảng XuatKho
CREATE TABLE PhieuXuat (
    PhieuXuatID VARCHAR(20) PRIMARY KEY,
    KhachHangID VARCHAR(20),
    NhanVienID  VARCHAR(20),
    NgayXuat DATETIME NOT NULL DEFAULT GETDATE(),
    GhiChu NVARCHAR(255),
	TongTien DECIMAL(10, 3) 
    FOREIGN KEY (NhanVienID) REFERENCES NhanVien(NhanVienID),
	FOREIGN KEY (KhachHangID) REFERENCES KhachHang(KhachHangID),
);

-- Tạo bảng ChiTietPhieuXuat

CREATE TABLE LichSuXuatKho(
	 PhieuXuatID VARCHAR(20),
	 ViTriID VARCHAR(20),
	 PRIMARY KEY (ViTriID, PhieuXuatID),
	 SanPhamID VARCHAR(20),
	 SoLuong INT NOT NULL,
)
CREATE TABLE ChiTietPhieuXuat (
   
    PhieuXuatID VARCHAR(20),
    SanPhamID VARCHAR(20),
	PRIMARY KEY (PhieuXuatID,SanPhamID),
    SoLuong INT NOT NULL,
    GiaXuat DECIMAL(10, 3) NOT NULL,
	TongTien AS (SoLuong * GiaXuat),
    FOREIGN KEY (PhieuXuatID) REFERENCES PhieuXuat(PhieuXuatID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(SanPhamID)
);

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

select * from ViTriKho


























-- Tạo trigger để đảm bảo tổng số lượng ở các vị trí bằng số lượng trong bảng sản phẩm
CREATE TRIGGER trg_SoLuongViTri
ON ViTriKho
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @SanPhamID INT;

    -- Lấy SanPhamID từ bảng ViTriKho
    SELECT @SanPhamID = SanPhamID FROM inserted;

    -- Cập nhật lại số lượng sản phẩm trong bảng SanPham dựa trên tổng số lượng ở các vị trí
    UPDATE SanPham
    SET SoLuong = (
        SELECT SUM(SoLuong) 
        FROM ViTriKho 
        WHERE SanPhamID = @SanPhamID
    )
    WHERE SanPhamID = @SanPhamID;
END;
GO




-- Tạo trigger để trừ số lượng ở các vị trí khi xuất kho
drop TRIGGER trg_TruSoLuongXuatKho
ON ChiTietPhieuXuat
AFTER INSERT
AS
BEGIN
    DECLARE @SanPhamID INT;
    DECLARE @SoLuong INT;

    -- Lấy SanPhamID và SoLuong từ bảng ChiTietPhieuXuat
    SELECT @SanPhamID = SanPhamID, @SoLuong = SoLuong FROM inserted;

    -- Giảm số lượng ở các vị trí
    UPDATE ViTriKho
    SET SoLuong = SoLuong - @SoLuong
    WHERE SanPhamID = @SanPhamID AND SoLuong >= @SoLuong;

    -- Cập nhật lại số lượng sản phẩm trong bảng SanPham
    UPDATE SanPham
    SET SoLuong = (
        SELECT SUM(SoLuong) 
        FROM ViTriKho 
        WHERE SanPhamID = @SanPhamID
    )
    WHERE SanPhamID = @SanPhamID;
END;
GO


-- Tạo trigger để cập nhật TổngTiền trong NhapKho
CREATE TRIGGER trg_CapNhatTongTienNhapKho
ON NhapKho
AFTER INSERT
AS
BEGIN
    DECLARE @NhapKhoID INT;

    -- Lấy NhapKhoID từ bảng NhapKho
    SELECT @NhapKhoID = NhapKhoID FROM inserted;

    -- Cập nhật TổngTiền trong NhapKho
    UPDATE NhapKho
    SET TongTien = (
        SELECT SUM(ThanhTien)
        FROM ChiTietPhieuNhap
        WHERE NhapKhoID = @NhapKhoID
    )
    WHERE NhapKhoID = @NhapKhoID;
END;
GO

-- Tạo trigger để cập nhật TổngTiền trong XuatKho
CREATE TRIGGER trg_CapNhatTongTienXuatKho
ON XuatKho
AFTER INSERT
AS
BEGIN
    DECLARE @XuatKhoID INT;

    -- Lấy XuatKhoID từ bảng XuatKho
    SELECT @XuatKhoID = XuatKhoID FROM inserted;

    -- Cập nhật TổngTiền trong XuatKho
    UPDATE XuatKho
    SET TongTien = (
        SELECT SUM(ThanhTien)
        FROM ChiTietPhieuXuat
        WHERE XuatKhoID = @XuatKhoID
    )
    WHERE XuatKhoID = @XuatKhoID;
END;
GO