IF DB_ID(N'QuanLyThuVienDB') IS NULL
BEGIN
    CREATE DATABASE QuanLyThuVienDB;
END
GO

USE QuanLyThuVienDB;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'dbo.NhanVien', N'U') IS NULL
CREATE TABLE dbo.NhanVien
(
    MaNhanVien NVARCHAR(20) NOT NULL PRIMARY KEY,
    Ho NVARCHAR(50) NOT NULL,
    Ten NVARCHAR(50) NOT NULL,
    Phai NVARCHAR(10) NOT NULL CHECK (Phai IN (N'Nam', N'Nữ')),
    NgaySinh DATE NOT NULL,
    ChucVu NVARCHAR(80) NOT NULL,
    SoDienThoai NVARCHAR(20) NULL
);

IF OBJECT_ID(N'dbo.TheLoai', N'U') IS NULL
CREATE TABLE dbo.TheLoai
(
    MaTheLoai NVARCHAR(20) NOT NULL PRIMARY KEY,
    TenTheLoai NVARCHAR(100) NOT NULL UNIQUE
);

IF OBJECT_ID(N'dbo.NhaXuatBan', N'U') IS NULL
CREATE TABLE dbo.NhaXuatBan
(
    MaNhaXuatBan NVARCHAR(20) NOT NULL PRIMARY KEY,
    DiaChi NVARCHAR(250) NOT NULL,
    SoDienThoai NVARCHAR(20) NULL
);

IF OBJECT_ID(N'dbo.DauSach', N'U') IS NULL
CREATE TABLE dbo.DauSach
(
    MaDauSach NVARCHAR(20) NOT NULL PRIMARY KEY,
    TenSach NVARCHAR(200) NOT NULL,
    NamXuatBan INT NOT NULL CHECK (NamXuatBan BETWEEN 1900 AND 2100),
    SoLuongHienCo INT NOT NULL CHECK (SoLuongHienCo >= 0),
    MaTheLoai NVARCHAR(20) NOT NULL,
    MaNhaXuatBan NVARCHAR(20) NOT NULL,
    CONSTRAINT FK_DauSach_TheLoai FOREIGN KEY (MaTheLoai) REFERENCES dbo.TheLoai(MaTheLoai),
    CONSTRAINT FK_DauSach_NhaXuatBan FOREIGN KEY (MaNhaXuatBan) REFERENCES dbo.NhaXuatBan(MaNhaXuatBan)
);

IF OBJECT_ID(N'dbo.DocGia', N'U') IS NULL
CREATE TABLE dbo.DocGia
(
    MaDocGia NVARCHAR(20) NOT NULL PRIMARY KEY,
    Ho NVARCHAR(50) NOT NULL,
    Ten NVARCHAR(50) NOT NULL,
    NgaySinh DATE NOT NULL,
    Phai NVARCHAR(10) NOT NULL CHECK (Phai IN (N'Nam', N'Nữ')),
    SoDienThoai NVARCHAR(20) NULL,
    DiaChi NVARCHAR(250) NOT NULL,
    Email NVARCHAR(150) NOT NULL CHECK (Email LIKE N'%@%')
);

IF OBJECT_ID(N'dbo.TheDocGia', N'U') IS NULL
CREATE TABLE dbo.TheDocGia
(
    MaThe NVARCHAR(30) NOT NULL PRIMARY KEY,
    MaDocGia NVARCHAR(20) NOT NULL,
    NgayCap DATE NOT NULL,
    HanSuDung DATE NOT NULL,
    DaDongLePhi BIT NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    CONSTRAINT CK_TheDocGia_HanSuDung CHECK (HanSuDung >= NgayCap),
    CONSTRAINT FK_TheDocGia_DocGia FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia)
);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_TheDocGia_MotTheHoatDong')
CREATE UNIQUE INDEX UX_TheDocGia_MotTheHoatDong ON dbo.TheDocGia(MaDocGia) WHERE TrangThai = 1;

IF OBJECT_ID(N'dbo.PhieuMuon', N'U') IS NULL
CREATE TABLE dbo.PhieuMuon
(
    MaPhieuMuon NVARCHAR(30) NOT NULL PRIMARY KEY,
    MaDocGia NVARCHAR(20) NOT NULL,
    MaNhanVien NVARCHAR(20) NOT NULL,
    NgayMuon DATE NOT NULL,
    NgayHenTra DATE NOT NULL,
    CONSTRAINT CK_PhieuMuon_Ngay CHECK (NgayHenTra >= NgayMuon),
    CONSTRAINT FK_PhieuMuon_DocGia FOREIGN KEY (MaDocGia) REFERENCES dbo.DocGia(MaDocGia),
    CONSTRAINT FK_PhieuMuon_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien)
);

IF OBJECT_ID(N'dbo.ChiTietPhieuMuon', N'U') IS NULL
CREATE TABLE dbo.ChiTietPhieuMuon
(
    MaChiTiet NVARCHAR(35) NOT NULL PRIMARY KEY,
    MaPhieuMuon NVARCHAR(30) NOT NULL,
    MaDauSach NVARCHAR(20) NOT NULL,
    NgayTraThucTe DATE NULL,
    TinhTrangTra NVARCHAR(50) NULL,
    CONSTRAINT UQ_ChiTietPhieuMuon UNIQUE (MaPhieuMuon, MaDauSach),
    CONSTRAINT FK_ChiTietPhieuMuon_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES dbo.PhieuMuon(MaPhieuMuon),
    CONSTRAINT FK_ChiTietPhieuMuon_DauSach FOREIGN KEY (MaDauSach) REFERENCES dbo.DauSach(MaDauSach)
);

IF OBJECT_ID(N'dbo.PhieuPhat', N'U') IS NULL
CREATE TABLE dbo.PhieuPhat
(
    MaPhieuPhat NVARCHAR(35) NOT NULL PRIMARY KEY,
    MaChiTiet NVARCHAR(35) NOT NULL UNIQUE,
    MaNhanVien NVARCHAR(20) NOT NULL,
    NgayPhat DATE NOT NULL,
    LyDo NVARCHAR(250) NOT NULL,
    PhiPhat DECIMAL(18, 0) NOT NULL CHECK (PhiPhat > 0),
    CONSTRAINT FK_PhieuPhat_ChiTiet FOREIGN KEY (MaChiTiet) REFERENCES dbo.ChiTietPhieuMuon(MaChiTiet),
    CONSTRAINT FK_PhieuPhat_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien)
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.NhanVien)
INSERT INTO dbo.NhanVien (MaNhanVien, Ho, Ten, Phai, NgaySinh, ChucVu, SoDienThoai) VALUES
(N'NV001', N'Nguyễn', N'An', N'Nam', '1990-02-15', N'Thủ thư', N'0901000001'),
(N'NV002', N'Trần', N'Bình', N'Nữ', '1992-08-20', N'Nhân viên quản lý sách', N'0901000002'),
(N'NV003', N'Lê', N'Hà', N'Nữ', '1995-11-05', N'Thủ thư', N'0901000003');

IF NOT EXISTS (SELECT 1 FROM dbo.TheLoai)
INSERT INTO dbo.TheLoai (MaTheLoai, TenTheLoai) VALUES
(N'TL001', N'Tin học'), (N'TL002', N'Tiểu thuyết'), (N'TL003', N'Kỹ năng sống');

IF NOT EXISTS (SELECT 1 FROM dbo.NhaXuatBan)
INSERT INTO dbo.NhaXuatBan (MaNhaXuatBan, DiaChi, SoDienThoai) VALUES
(N'NXB001', N'Quận 1, TP.HCM', N'0283000001'),
(N'NXB002', N'Cầu Giấy, Hà Nội', N'0243000002'),
(N'NXB003', N'Hải Châu, Đà Nẵng', N'02363000003');

IF NOT EXISTS (SELECT 1 FROM dbo.DauSach)
INSERT INTO dbo.DauSach (MaDauSach, TenSach, NamXuatBan, SoLuongHienCo, MaTheLoai, MaNhaXuatBan) VALUES
(N'S001', N'Lập trình C# căn bản', 2025, 5, N'TL001', N'NXB001'),
(N'S002', N'Cơ sở dữ liệu', 2024, 4, N'TL001', N'NXB001'),
(N'S003', N'Đắc nhân tâm', 2023, 3, N'TL003', N'NXB002'),
(N'S004', N'Nhà giả kim', 2022, 2, N'TL002', N'NXB003');

IF NOT EXISTS (SELECT 1 FROM dbo.DocGia)
INSERT INTO dbo.DocGia (MaDocGia, Ho, Ten, NgaySinh, Phai, SoDienThoai, DiaChi, Email) VALUES
(N'DG001', N'Phạm', N'Minh Giang', '2004-05-12', N'Nam', N'0911000001', N'TP.HCM', N'giang@example.com'),
(N'DG002', N'Nguyễn', N'Thảo', '2003-10-23', N'Nữ', N'0911000002', N'Hà Nội', N'thao@example.com'),
(N'DG003', N'Trần', N'Quang', '2004-02-01', N'Nam', N'0911000003', N'Đà Nẵng', N'quang@example.com');

IF NOT EXISTS (SELECT 1 FROM dbo.TheDocGia)
INSERT INTO dbo.TheDocGia (MaThe, MaDocGia, NgayCap, HanSuDung, DaDongLePhi, TrangThai) VALUES
(N'THE001', N'DG001', '2026-01-01', '2026-12-31', 1, 1),
(N'THE002', N'DG002', '2026-01-01', '2026-12-31', 1, 1),
(N'THE003', N'DG003', '2026-01-01', '2026-12-31', 0, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PhieuMuon)
INSERT INTO dbo.PhieuMuon (MaPhieuMuon, MaDocGia, MaNhanVien, NgayMuon, NgayHenTra) VALUES
(N'PM001', N'DG001', N'NV001', '2026-06-10', '2026-06-15'),
(N'PM002', N'DG002', N'NV003', '2026-07-01', '2026-07-08'),
(N'PM003', N'DG003', N'NV001', '2026-08-10', '2026-08-17');

IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietPhieuMuon)
INSERT INTO dbo.ChiTietPhieuMuon (MaChiTiet, MaPhieuMuon, MaDauSach, NgayTraThucTe, TinhTrangTra) VALUES
(N'CT001', N'PM001', N'S001', '2026-06-18', N'Bình thường'),
(N'CT002', N'PM001', N'S002', NULL, NULL),
(N'CT003', N'PM002', N'S003', '2026-07-07', N'Rách/Hư hỏng'),
(N'CT004', N'PM003', N'S004', '2026-08-15', N'Mất');

IF NOT EXISTS (SELECT 1 FROM dbo.PhieuPhat)
INSERT INTO dbo.PhieuPhat (MaPhieuPhat, MaChiTiet, MaNhanVien, NgayPhat, LyDo, PhiPhat) VALUES
(N'PP001', N'CT001', N'NV001', '2026-06-18', N'Trả trễ hạn', 20000),
(N'PP002', N'CT003', N'NV003', '2026-07-07', N'Rách/Hư hỏng', 50000),
(N'PP003', N'CT004', N'NV001', '2026-08-15', N'Mất sách', 100000);
GO
