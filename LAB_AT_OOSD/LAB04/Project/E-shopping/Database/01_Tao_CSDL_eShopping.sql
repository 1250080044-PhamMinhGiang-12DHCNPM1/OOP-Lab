/* Chạy tệp này trong SQL Server Management Studio trước khi chạy ứng dụng. */
IF DB_ID(N'EShoppingDb') IS NULL
    CREATE DATABASE EShoppingDb;
GO
USE EShoppingDb;
GO

IF OBJECT_ID(N'dbo.ChiTietDonHang', N'U') IS NOT NULL DROP TABLE dbo.ChiTietDonHang;
IF OBJECT_ID(N'dbo.GiaoDichThanhToan', N'U') IS NOT NULL DROP TABLE dbo.GiaoDichThanhToan;
IF OBJECT_ID(N'dbo.MucGioHang', N'U') IS NOT NULL DROP TABLE dbo.MucGioHang;
IF OBJECT_ID(N'dbo.GioHang', N'U') IS NOT NULL DROP TABLE dbo.GioHang;
IF OBJECT_ID(N'dbo.DonHang', N'U') IS NOT NULL DROP TABLE dbo.DonHang;
IF OBJECT_ID(N'dbo.SanPham', N'U') IS NOT NULL DROP TABLE dbo.SanPham;
IF OBJECT_ID(N'dbo.KhachHang', N'U') IS NOT NULL DROP TABLE dbo.KhachHang;
GO

CREATE TABLE dbo.KhachHang (
    MaKhachHang INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ThuDienTu NVARCHAR(100) NULL UNIQUE,
    MatKhau NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.SanPham (
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    MaHienThi NVARCHAR(20) NOT NULL UNIQUE,
    TenSanPham NVARCHAR(150) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    SoLuongTon INT NOT NULL CHECK (SoLuongTon >= 0),
    DangBan BIT NOT NULL DEFAULT 1
);

CREATE TABLE dbo.GioHang (
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaKhachHang INT NOT NULL UNIQUE,
    CONSTRAINT FK_GioHang_KhachHang FOREIGN KEY (MaKhachHang) REFERENCES dbo.KhachHang(MaKhachHang)
);

CREATE TABLE dbo.MucGioHang (
    MaGioHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    CONSTRAINT PK_MucGioHang PRIMARY KEY (MaGioHang, MaSanPham),
    CONSTRAINT FK_MucGioHang_GioHang FOREIGN KEY (MaGioHang) REFERENCES dbo.GioHang(MaGioHang),
    CONSTRAINT FK_MucGioHang_SanPham FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham)
);

CREATE TABLE dbo.DonHang (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaKhachHang INT NOT NULL,
    TenNguoiNhan NVARCHAR(100) NOT NULL,
    SoDienThoaiNguoiNhan NVARCHAR(20) NOT NULL,
    DiaChiGiaoHang NVARCHAR(250) NOT NULL,
    HinhThucGiaoHang NVARCHAR(50) NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL,
    NgayDat DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DonHang_KhachHang FOREIGN KEY (MaKhachHang) REFERENCES dbo.KhachHang(MaKhachHang)
);

CREATE TABLE dbo.ChiTietDonHang (
    MaChiTietDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_ChiTietDonHang_DonHang FOREIGN KEY (MaDonHang) REFERENCES dbo.DonHang(MaDonHang),
    CONSTRAINT FK_ChiTietDonHang_SanPham FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham)
);

CREATE TABLE dbo.GiaoDichThanhToan (
    MaGiaoDich INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
    SoTien DECIMAL(18,2) NOT NULL,
    KetQua NVARCHAR(50) NOT NULL,
    ThoiGian DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_GiaoDichThanhToan_DonHang FOREIGN KEY (MaDonHang) REFERENCES dbo.DonHang(MaDonHang)
);
GO

INSERT INTO dbo.KhachHang(HoTen, ThuDienTu, MatKhau)
VALUES (N'Khách hàng mẫu', N'khachhang@example.com', N'123456');

INSERT INTO dbo.SanPham(MaHienThi, TenSanPham, DonGia, SoLuongTon) VALUES
(N'SP01', N'Áo thun cơ bản', 150000, 30),
(N'SP02', N'Balo sinh viên', 280000, 20),
(N'SP03', N'Bình nước thể thao', 90000, 50);
GO
