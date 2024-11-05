
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


CREATE TABLE LichSuXuatKho (
    PhieuXuatID VARCHAR(20),
    ViTriID VARCHAR(20),
    DanhMucID VARCHAR(20),
    PRIMARY KEY (ViTriID, DanhMucID, PhieuXuatID),
    SanPhamID VARCHAR(20),
    SoLuong INT NOT NULL,
    FOREIGN KEY (PhieuXuatID) REFERENCES PhieuXuat(PhieuXuatID),
    FOREIGN KEY (ViTriID, DanhMucID) REFERENCES ViTriKho(ViTriID, DanhMucID),

);

CREATE TRIGGER trg_UpdateViTriKhoOnInsert
ON LichSuXuatKho
AFTER INSERT
AS
BEGIN
    -- Khai báo các biến để lưu thông tin từ bản ghi mới được thêm vào
    DECLARE @ViTriID VARCHAR(20),
            @DanhMucID VARCHAR(20),
            @SanPhamID VARCHAR(20),
            @SoLuong INT;

    -- Lấy dữ liệu từ bản ghi được thêm vào LichSuXuatKho
    SELECT @ViTriID = i.ViTriID,
           @DanhMucID = i.DanhMucID,
           @SanPhamID = i.SanPhamID,
           @SoLuong = i.SoLuong
    FROM INSERTED i;

    -- Cập nhật số lượng trong ViTriKho
    UPDATE ViTriKho
    SET SoLuong = SoLuong - @SoLuong
    WHERE ViTriID = @ViTriID AND DanhMucID = @DanhMucID;

    -- Kiểm tra nếu số lượng = 0 thì đặt SanPhamID thành NULL
    UPDATE ViTriKho
    SET SanPhamID = NULL
    WHERE ViTriID = @ViTriID AND DanhMucID = @DanhMucID AND SoLuong = 0;
END;








drop TRIGGER trg_SoLuongViTri
ON ViTriKho
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @SanPhamID VARCHAR(20);

    -- Lấy các SanPhamID bị ảnh hưởng
    DECLARE affectedSanPhamIDs CURSOR FOR 
    SELECT DISTINCT SanPhamID FROM inserted
    UNION
    SELECT DISTINCT SanPhamID FROM deleted;

    OPEN affectedSanPhamIDs;
    FETCH NEXT FROM affectedSanPhamIDs INTO @SanPhamID;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Cập nhật lại số lượng sản phẩm trong bảng SanPham dựa trên tổng số lượng ở các vị trí
        UPDATE SanPham
        SET SoLuong = (
            SELECT COALESCE(SUM(SoLuong), 0)
            FROM ViTriKho 
            WHERE SanPhamID = @SanPhamID
        )
        WHERE SanPhamID = @SanPhamID;

        FETCH NEXT FROM affectedSanPhamIDs INTO @SanPhamID;
    END

    CLOSE affectedSanPhamIDs;
    DEALLOCATE affectedSanPhamIDs;
END;
GO

CREATE TRIGGER trg_TruSoLuongXuatKho
ON ChiTietPhieuXuat
AFTER INSERT
AS
BEGIN
    DECLARE @SanPhamID VARCHAR(20);
    DECLARE @SoLuong INT;

    DECLARE affectedItems CURSOR FOR 
    SELECT SanPhamID, SoLuong FROM inserted;

    OPEN affectedItems;
    FETCH NEXT FROM affectedItems INTO @SanPhamID, @SoLuong;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Giảm số lượng ở các vị trí
        UPDATE ViTriKho
        SET SoLuong = SoLuong - @SoLuong
        WHERE SanPhamID = @SanPhamID AND SoLuong >= @SoLuong;

        -- Cập nhật lại số lượng sản phẩm trong bảng SanPham
        UPDATE SanPham
        SET SoLuong = (
            SELECT COALESCE(SUM(SoLuong), 0) 
            FROM ViTriKho 
            WHERE SanPhamID = @SanPhamID
        )
        WHERE SanPhamID = @SanPhamID;

        FETCH NEXT FROM affectedItems INTO @SanPhamID, @SoLuong;
    END

    CLOSE affectedItems;
    DEALLOCATE affectedItems;
END;
GO



CREATE TRIGGER trg_CapNhatTongTienPhieuXuat
ON ChiTietPhieuXuat
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @PhieuXuatID VARCHAR(20);

    DECLARE affectedPhieuXuatIDs CURSOR FOR 
    SELECT DISTINCT PhieuXuatID FROM inserted
    UNION
    SELECT DISTINCT PhieuXuatID FROM deleted;

    OPEN affectedPhieuXuatIDs;
    FETCH NEXT FROM affectedPhieuXuatIDs INTO @PhieuXuatID;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Cập nhật TongTien cho mỗi phiếu xuất
        UPDATE PhieuXuat
        SET TongTien = (
            SELECT COALESCE(SUM(CAST(SoLuong AS DECIMAL(10, 2)) * GiaXuat), 0)
            FROM ChiTietPhieuXuat
            WHERE PhieuXuatID = @PhieuXuatID
        )
        WHERE PhieuXuatID = @PhieuXuatID;

        FETCH NEXT FROM affectedPhieuXuatIDs INTO @PhieuXuatID;
    END

    CLOSE affectedPhieuXuatIDs;
    DEALLOCATE affectedPhieuXuatIDs;
END;
GO

CREATE TRIGGER trg_CapNhatSoLuongSanPhamKhiNhap
ON ChiTietPhieuNhap
AFTER INSERT
AS
BEGIN
    DECLARE @SanPhamID VARCHAR(20);
    DECLARE @SoLuong INT;

    DECLARE affectedItems CURSOR FOR 
    SELECT SanPhamID, SoLuong FROM inserted;

    OPEN affectedItems;
    FETCH NEXT FROM affectedItems INTO @SanPhamID, @SoLuong;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Tăng số lượng sản phẩm trong bảng SanPham dựa trên số lượng nhập
        UPDATE SanPham
        SET SoLuong = SoLuong + @SoLuong
        WHERE SanPhamID = @SanPhamID;

        FETCH NEXT FROM affectedItems INTO @SanPhamID, @SoLuong;
    END

    CLOSE affectedItems;
    DEALLOCATE affectedItems;
END;
GO


CREATE TRIGGER trg_CapNhatSoLuongSanPhamKhiXuat
ON ChiTietPhieuXuat
AFTER INSERT
AS
BEGIN
    DECLARE @SanPhamID VARCHAR(20);
    DECLARE @SoLuong INT;

    DECLARE affectedItems CURSOR FOR 
    SELECT SanPhamID, SoLuong FROM inserted;

    OPEN affectedItems;
    FETCH NEXT FROM affectedItems INTO @SanPhamID, @SoLuong;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Giảm số lượng sản phẩm trong bảng SanPham dựa trên số lượng xuất
        UPDATE SanPham
        SET SoLuong = SoLuong - @SoLuong
        WHERE SanPhamID = @SanPhamID AND SoLuong >= @SoLuong;

        -- Kiểm tra nếu số lượng sau khi giảm bằng 0, đặt sản phẩm đó về trạng thái không còn trong kho (nếu cần)
        IF (SELECT SoLuong FROM SanPham WHERE SanPhamID = @SanPhamID) <= 0
        BEGIN
            UPDATE SanPham
            SET SoLuong = 0
            WHERE SanPhamID = @SanPhamID;
        END

        FETCH NEXT FROM affectedItems INTO @SanPhamID, @SoLuong;
    END

    CLOSE affectedItems;
    DEALLOCATE affectedItems;
END;
GO
