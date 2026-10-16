/* LAB06 - Quan ly ho so benh an. Chay bang SQL Server / LocalDB voi ma UTF-8. */
USE master;
GO
IF DB_ID(N'QuanLyHoSoBenhAnLab06') IS NULL
    CREATE DATABASE QuanLyHoSoBenhAnLab06;
GO
USE QuanLyHoSoBenhAnLab06;
GO

/* Xoa schema cu de script co the chay lai tren database lab. */
IF OBJECT_ID(N'dbo.sp_ThanhToanHoanTat', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ThanhToanHoanTat;
IF OBJECT_ID(N'dbo.sp_LapPhieuKhamVaToa', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_LapPhieuKhamVaToa;
IF OBJECT_ID(N'dbo.sp_DangKyCapHoSo', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_DangKyCapHoSo;
IF OBJECT_ID(N'dbo.ThanhToanVienPhi', N'U') IS NOT NULL DROP TABLE dbo.ThanhToanVienPhi;
IF OBJECT_ID(N'dbo.LichHen', N'U') IS NOT NULL DROP TABLE dbo.LichHen;
IF OBJECT_ID(N'dbo.ChiTietToaThuoc', N'U') IS NOT NULL DROP TABLE dbo.ChiTietToaThuoc;
IF OBJECT_ID(N'dbo.ToaThuoc', N'U') IS NOT NULL DROP TABLE dbo.ToaThuoc;
IF OBJECT_ID(N'dbo.KetQuaXetNghiem', N'U') IS NOT NULL DROP TABLE dbo.KetQuaXetNghiem;
IF OBJECT_ID(N'dbo.ChiDinhXetNghiem', N'U') IS NOT NULL DROP TABLE dbo.ChiDinhXetNghiem;
IF OBJECT_ID(N'dbo.PhieuKham', N'U') IS NOT NULL DROP TABLE dbo.PhieuKham;
IF OBJECT_ID(N'dbo.Thuoc', N'U') IS NOT NULL DROP TABLE dbo.Thuoc;
IF OBJECT_ID(N'dbo.BacSi', N'U') IS NOT NULL DROP TABLE dbo.BacSi;
IF OBJECT_ID(N'dbo.Khoa', N'U') IS NOT NULL DROP TABLE dbo.Khoa;
IF OBJECT_ID(N'dbo.HoSoBenhAn', N'U') IS NOT NULL DROP TABLE dbo.HoSoBenhAn;
IF OBJECT_ID(N'dbo.LePhiNam', N'U') IS NOT NULL DROP TABLE dbo.LePhiNam;
IF OBJECT_ID(N'dbo.PhieuDangKy', N'U') IS NOT NULL DROP TABLE dbo.PhieuDangKy;
IF OBJECT_ID(N'dbo.BenhNhan', N'U') IS NOT NULL DROP TABLE dbo.BenhNhan;
IF TYPE_ID(N'dbo.TT_ChiTietToa') IS NOT NULL DROP TYPE dbo.TT_ChiTietToa;
GO

CREATE TABLE dbo.BenhNhan (
    MaBN INT IDENTITY(1,1) CONSTRAINT PK_BenhNhan PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL, NgaySinh DATE NOT NULL, DiaChi NVARCHAR(200) NULL,
    DienThoai VARCHAR(15) NOT NULL CONSTRAINT UQ_BenhNhan_DienThoai UNIQUE,
    Anh3x4 NVARCHAR(260) NULL, NgayTao DATETIME2 NOT NULL CONSTRAINT DF_BenhNhan_NgayTao DEFAULT SYSDATETIME()
);
CREATE TABLE dbo.PhieuDangKy (
    MaDK INT IDENTITY(1,1) CONSTRAINT PK_PhieuDangKy PRIMARY KEY,
    MaBN INT NOT NULL CONSTRAINT FK_PhieuDangKy_BenhNhan REFERENCES dbo.BenhNhan(MaBN),
    NgayDangKy DATETIME2 NOT NULL CONSTRAINT DF_PhieuDangKy_Ngay DEFAULT SYSDATETIME(),
    TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_PhieuDangKy_TrangThai DEFAULT N'Hợp lệ',
    CONSTRAINT CK_PhieuDangKy_TrangThai CHECK (TrangThai IN (N'Hợp lệ', N'Hủy'))
);
CREATE TABLE dbo.LePhiNam (
    MaLePhi INT IDENTITY(1,1) CONSTRAINT PK_LePhiNam PRIMARY KEY,
    MaBN INT NOT NULL CONSTRAINT FK_LePhiNam_BenhNhan REFERENCES dbo.BenhNhan(MaBN),
    Nam INT NOT NULL, SoTien DECIMAL(18,2) NOT NULL, NgayThu DATETIME2 NOT NULL CONSTRAINT DF_LePhiNam_Ngay DEFAULT SYSDATETIME(),
    TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_LePhiNam_TrangThai DEFAULT N'Đã thanh toán',
    CONSTRAINT UQ_LePhiNam_BenhNhan_Nam UNIQUE (MaBN, Nam),
    CONSTRAINT CK_LePhiNam_Nam CHECK (Nam BETWEEN 2000 AND 2100),
    CONSTRAINT CK_LePhiNam_SoTien CHECK (SoTien > 0),
    CONSTRAINT CK_LePhiNam_TrangThai CHECK (TrangThai IN (N'Đã thanh toán', N'Hủy'))
);
CREATE TABLE dbo.HoSoBenhAn (
    MaHoSo VARCHAR(20) NOT NULL CONSTRAINT PK_HoSoBenhAn PRIMARY KEY,
    MaBN INT NOT NULL CONSTRAINT UQ_HoSoBenhAn_MaBN UNIQUE CONSTRAINT FK_HoSoBenhAn_BenhNhan REFERENCES dbo.BenhNhan(MaBN),
    NgayCap DATETIME2 NOT NULL CONSTRAINT DF_HoSoBenhAn_NgayCap DEFAULT SYSDATETIME(),
    TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_HoSoBenhAn_TrangThai DEFAULT N'Đang sử dụng',
    CONSTRAINT CK_HoSoBenhAn_TrangThai CHECK (TrangThai IN (N'Đang sử dụng', N'Ngừng sử dụng'))
);
CREATE TABLE dbo.Khoa (
    MaKhoa INT IDENTITY(1,1) CONSTRAINT PK_Khoa PRIMARY KEY, TenKhoa NVARCHAR(100) NOT NULL CONSTRAINT UQ_Khoa_Ten UNIQUE, MoTa NVARCHAR(300) NULL
);
CREATE TABLE dbo.BacSi (
    MaBS INT IDENTITY(1,1) CONSTRAINT PK_BacSi PRIMARY KEY,
    MaKhoa INT NOT NULL CONSTRAINT FK_BacSi_Khoa REFERENCES dbo.Khoa(MaKhoa),
    HoTen NVARCHAR(100) NOT NULL, ChuyenMon NVARCHAR(100) NULL, DienThoai VARCHAR(15) NULL CONSTRAINT UQ_BacSi_DienThoai UNIQUE
);
CREATE TABLE dbo.Thuoc (
    MaThuoc INT IDENTITY(1,1) CONSTRAINT PK_Thuoc PRIMARY KEY, TenThuoc NVARCHAR(150) NOT NULL CONSTRAINT UQ_Thuoc_Ten UNIQUE,
    DonViTinh NVARCHAR(30) NOT NULL, DonGia DECIMAL(18,2) NOT NULL, TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_Thuoc_TrangThai DEFAULT N'Đang dùng',
    CONSTRAINT CK_Thuoc_DonGia CHECK (DonGia >= 0), CONSTRAINT CK_Thuoc_TrangThai CHECK (TrangThai IN (N'Đang dùng', N'Ngừng dùng'))
);
CREATE TABLE dbo.PhieuKham (
    MaPK INT IDENTITY(1,1) CONSTRAINT PK_PhieuKham PRIMARY KEY, MaHoSo VARCHAR(20) NOT NULL CONSTRAINT FK_PhieuKham_HoSo REFERENCES dbo.HoSoBenhAn(MaHoSo),
    MaBS INT NOT NULL CONSTRAINT FK_PhieuKham_BacSi REFERENCES dbo.BacSi(MaBS), MaKhoa INT NOT NULL CONSTRAINT FK_PhieuKham_Khoa REFERENCES dbo.Khoa(MaKhoa),
    NgayGioKham DATETIME2 NOT NULL CONSTRAINT DF_PhieuKham_Ngay DEFAULT SYSDATETIME(), TrieuChung NVARCHAR(1000) NULL, ChanDoan NVARCHAR(1000) NULL,
    LoaiKham NVARCHAR(30) NOT NULL CONSTRAINT DF_PhieuKham_Loai DEFAULT N'Điều trị', TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_PhieuKham_TrangThai DEFAULT N'Đang điều trị',
    CONSTRAINT CK_PhieuKham_Loai CHECK (LoaiKham IN (N'Điều trị', N'Định kỳ')), CONSTRAINT CK_PhieuKham_TrangThai CHECK (TrangThai IN (N'Đang điều trị', N'Hoàn tất', N'Hủy'))
);
CREATE TABLE dbo.ChiDinhXetNghiem (
    MaCDXN INT IDENTITY(1,1) CONSTRAINT PK_ChiDinhXetNghiem PRIMARY KEY, MaPK INT NOT NULL CONSTRAINT FK_ChiDinhXN_PhieuKham REFERENCES dbo.PhieuKham(MaPK),
    NoiDung NVARCHAR(500) NOT NULL, NgayChiDinh DATETIME2 NOT NULL CONSTRAINT DF_ChiDinhXN_Ngay DEFAULT SYSDATETIME(), TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_ChiDinhXN_TrangThai DEFAULT N'Chờ kết quả',
    CONSTRAINT CK_ChiDinhXN_TrangThai CHECK (TrangThai IN (N'Chờ kết quả', N'Đã có kết quả', N'Hủy'))
);
CREATE TABLE dbo.KetQuaXetNghiem (
    MaKQXN INT IDENTITY(1,1) CONSTRAINT PK_KetQuaXetNghiem PRIMARY KEY, MaCDXN INT NOT NULL CONSTRAINT UQ_KetQuaXN_ChiDinh UNIQUE CONSTRAINT FK_KetQuaXN_ChiDinh REFERENCES dbo.ChiDinhXetNghiem(MaCDXN),
    ChiSo NVARCHAR(500) NULL, KetQua NVARCHAR(1000) NOT NULL, NgayTraKQ DATETIME2 NOT NULL CONSTRAINT DF_KetQuaXN_Ngay DEFAULT SYSDATETIME()
);
CREATE TABLE dbo.ToaThuoc (
    MaToa INT IDENTITY(1,1) CONSTRAINT PK_ToaThuoc PRIMARY KEY, MaPK INT NOT NULL CONSTRAINT UQ_ToaThuoc_MaPK UNIQUE CONSTRAINT FK_ToaThuoc_PhieuKham REFERENCES dbo.PhieuKham(MaPK),
    NgayLap DATETIME2 NOT NULL CONSTRAINT DF_ToaThuoc_Ngay DEFAULT SYSDATETIME(), LoiDan NVARCHAR(1000) NULL
);
CREATE TABLE dbo.ChiTietToaThuoc (
    MaCTToa INT IDENTITY(1,1) CONSTRAINT PK_ChiTietToaThuoc PRIMARY KEY, MaToa INT NOT NULL CONSTRAINT FK_CTToa_ToaThuoc REFERENCES dbo.ToaThuoc(MaToa),
    MaThuoc INT NOT NULL CONSTRAINT FK_CTToa_Thuoc REFERENCES dbo.Thuoc(MaThuoc), SoLuong INT NOT NULL, LieuDung NVARCHAR(200) NOT NULL, CachDung NVARCHAR(300) NULL,
    CONSTRAINT UQ_CTToa_ToaThuoc_Thuoc UNIQUE (MaToa, MaThuoc), CONSTRAINT CK_CTToa_SoLuong CHECK (SoLuong > 0)
);
CREATE TABLE dbo.LichHen (
    MaLichHen INT IDENTITY(1,1) CONSTRAINT PK_LichHen PRIMARY KEY, MaHoSo VARCHAR(20) NOT NULL CONSTRAINT FK_LichHen_HoSo REFERENCES dbo.HoSoBenhAn(MaHoSo),
    NgayGioHen DATETIME2 NOT NULL, LoaiHen NVARCHAR(30) NOT NULL, TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_LichHen_TrangThai DEFAULT N'Đã đặt',
    CONSTRAINT UQ_LichHen_HoSo_Ngay UNIQUE (MaHoSo, NgayGioHen), CONSTRAINT CK_LichHen_Loai CHECK (LoaiHen IN (N'Định kỳ', N'Tái khám')),
    CONSTRAINT CK_LichHen_TrangThai CHECK (TrangThai IN (N'Đã đặt', N'Đã khám', N'Đã hủy'))
);
CREATE TABLE dbo.ThanhToanVienPhi (
    MaTT INT IDENTITY(1,1) CONSTRAINT PK_ThanhToanVienPhi PRIMARY KEY, MaPK INT NOT NULL CONSTRAINT UQ_ThanhToanVienPhi_MaPK UNIQUE CONSTRAINT FK_ThanhToanVienPhi_PhieuKham REFERENCES dbo.PhieuKham(MaPK),
    SoTien DECIMAL(18,2) NOT NULL, NgayThanhToan DATETIME2 NOT NULL CONSTRAINT DF_ThanhToan_Ngay DEFAULT SYSDATETIME(), PhuongThuc NVARCHAR(30) NOT NULL, TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_ThanhToan_TrangThai DEFAULT N'Đã thanh toán',
    CONSTRAINT CK_ThanhToan_SoTien CHECK (SoTien >= 0), CONSTRAINT CK_ThanhToan_PhuongThuc CHECK (PhuongThuc IN (N'Tiền mặt', N'Chuyển khoản', N'Thẻ')),
    CONSTRAINT CK_ThanhToan_TrangThai CHECK (TrangThai IN (N'Đã thanh toán', N'Hủy'))
);
GO
CREATE INDEX IX_PhieuKham_MaHoSo_Ngay ON dbo.PhieuKham(MaHoSo, NgayGioKham DESC);
CREATE INDEX IX_ChiDinhXN_MaPK ON dbo.ChiDinhXetNghiem(MaPK);
CREATE INDEX IX_LichHen_MaHoSo_Ngay ON dbo.LichHen(MaHoSo, NgayGioHen);
GO
CREATE TYPE dbo.TT_ChiTietToa AS TABLE (MaThuoc INT NOT NULL, SoLuong INT NOT NULL, LieuDung NVARCHAR(200) NOT NULL, CachDung NVARCHAR(300) NULL);
GO
CREATE OR ALTER PROCEDURE dbo.sp_DangKyCapHoSo
 @HoTen NVARCHAR(100), @NgaySinh DATE, @DiaChi NVARCHAR(200), @DienThoai VARCHAR(15), @Anh3x4 NVARCHAR(260), @SoTienLePhi DECIMAL(18,2), @Nam INT, @MaHoSo VARCHAR(20) OUTPUT
AS
BEGIN
 SET NOCOUNT ON; SET XACT_ABORT ON;
 BEGIN TRANSACTION;
 BEGIN TRY
   DECLARE @MaBN INT = (SELECT MaBN FROM dbo.BenhNhan WHERE DienThoai=@DienThoai);
   IF @MaBN IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.HoSoBenhAn WHERE MaBN=@MaBN) THROW 51001, N'Bệnh nhân đã có hồ sơ bệnh án.', 1;
   IF @MaBN IS NULL BEGIN INSERT dbo.BenhNhan(HoTen,NgaySinh,DiaChi,DienThoai,Anh3x4) VALUES(@HoTen,@NgaySinh,@DiaChi,@DienThoai,@Anh3x4); SET @MaBN=SCOPE_IDENTITY(); END;
   INSERT dbo.PhieuDangKy(MaBN) VALUES(@MaBN);
   INSERT dbo.LePhiNam(MaBN,Nam,SoTien) VALUES(@MaBN,@Nam,@SoTienLePhi);
   SET @MaHoSo='HS'+RIGHT('000000'+CAST(@MaBN AS VARCHAR(6)),6);
   INSERT dbo.HoSoBenhAn(MaHoSo,MaBN) VALUES(@MaHoSo,@MaBN);
   COMMIT TRANSACTION;
 END TRY
 BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_LapPhieuKhamVaToa
 @MaHoSo VARCHAR(20), @MaBS INT, @MaKhoa INT, @TrieuChung NVARCHAR(1000), @ChanDoan NVARCHAR(1000), @NoiDungXetNghiem NVARCHAR(500)=NULL, @LoiDan NVARCHAR(1000)=NULL, @ChiTiet dbo.TT_ChiTietToa READONLY, @MaPK INT OUTPUT, @MaToa INT OUTPUT
AS
BEGIN
 SET NOCOUNT ON; SET XACT_ABORT ON;
 BEGIN TRANSACTION;
 BEGIN TRY
   IF NOT EXISTS(SELECT 1 FROM dbo.HoSoBenhAn WHERE MaHoSo=@MaHoSo AND TrangThai=N'Đang sử dụng') THROW 51002, N'Hồ sơ không hợp lệ.', 1;
   IF NOT EXISTS(SELECT 1 FROM @ChiTiet) THROW 51003, N'Toa thuốc phải có ít nhất một chi tiết.', 1;
   INSERT dbo.PhieuKham(MaHoSo,MaBS,MaKhoa,TrieuChung,ChanDoan) VALUES(@MaHoSo,@MaBS,@MaKhoa,@TrieuChung,@ChanDoan); SET @MaPK=SCOPE_IDENTITY();
   IF NULLIF(LTRIM(RTRIM(@NoiDungXetNghiem)),N'') IS NOT NULL INSERT dbo.ChiDinhXetNghiem(MaPK,NoiDung) VALUES(@MaPK,@NoiDungXetNghiem);
   INSERT dbo.ToaThuoc(MaPK,LoiDan) VALUES(@MaPK,@LoiDan); SET @MaToa=SCOPE_IDENTITY();
   INSERT dbo.ChiTietToaThuoc(MaToa,MaThuoc,SoLuong,LieuDung,CachDung) SELECT @MaToa,MaThuoc,SoLuong,LieuDung,CachDung FROM @ChiTiet;
   COMMIT TRANSACTION;
 END TRY
 BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_ThanhToanHoanTat
 @MaPK INT, @SoTien DECIMAL(18,2), @PhuongThuc NVARCHAR(30), @MaTT INT OUTPUT
AS
BEGIN
 SET NOCOUNT ON; SET XACT_ABORT ON;
 BEGIN TRANSACTION;
 BEGIN TRY
   IF NOT EXISTS(SELECT 1 FROM dbo.PhieuKham WHERE MaPK=@MaPK AND TrangThai=N'Đang điều trị') THROW 51004, N'Phiếu khám không ở trạng thái đang điều trị.', 1;
   IF EXISTS(SELECT 1 FROM dbo.ThanhToanVienPhi WHERE MaPK=@MaPK AND TrangThai=N'Đã thanh toán') THROW 51005, N'Phiếu khám đã thanh toán.', 1;
   INSERT dbo.ThanhToanVienPhi(MaPK,SoTien,PhuongThuc) VALUES(@MaPK,@SoTien,@PhuongThuc); SET @MaTT=SCOPE_IDENTITY();
   UPDATE dbo.PhieuKham SET TrangThai=N'Hoàn tất' WHERE MaPK=@MaPK;
   COMMIT TRANSACTION;
 END TRY
 BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO
INSERT dbo.Khoa(TenKhoa,MoTa) VALUES (N'Nội tổng quát',N'Khám bệnh nội khoa'),(N'Xét nghiệm',N'Thực hiện xét nghiệm');
INSERT dbo.BacSi(MaKhoa,HoTen,ChuyenMon,DienThoai) VALUES (1,N'Nguyễn Văn An',N'Nội tổng quát','0900000001');
INSERT dbo.Thuoc(TenThuoc,DonViTinh,DonGia) VALUES (N'Paracetamol 500mg',N'Viên',1000),(N'Vitamin C',N'Viên',500);
GO
