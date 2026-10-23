SET NOCOUNT ON;
IF DB_ID(N'QuanLyNhanSuDongA') IS NULL
    EXEC(N'CREATE DATABASE QuanLyNhanSuDongA');
GO
USE QuanLyNhanSuDongA;
GO

IF OBJECT_ID('dbo.PhongBan','U') IS NULL
CREATE TABLE dbo.PhongBan(
    MaPhongBan INT IDENTITY(1,1) CONSTRAINT PK_PhongBan PRIMARY KEY,
    TenPhongBan NVARCHAR(100) NOT NULL CONSTRAINT UQ_PhongBan_Ten UNIQUE,
    TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_PhongBan_TrangThai DEFAULT N'Hoat dong'
);
IF OBJECT_ID('dbo.ChucVu','U') IS NULL
CREATE TABLE dbo.ChucVu(
    MaChucVu INT IDENTITY(1,1) CONSTRAINT PK_ChucVu PRIMARY KEY,
    TenChucVu NVARCHAR(100) NOT NULL CONSTRAINT UQ_ChucVu_Ten UNIQUE
);
IF OBJECT_ID('dbo.CaLamViec','U') IS NULL
CREATE TABLE dbo.CaLamViec(
    MaCa INT IDENTITY(1,1) CONSTRAINT PK_CaLamViec PRIMARY KEY,
    TenCa NVARCHAR(50) NOT NULL CONSTRAINT UQ_CaLamViec_Ten UNIQUE,
    GioBatDau TIME NOT NULL, GioKetThuc TIME NOT NULL
);
IF OBJECT_ID('dbo.ThamSoHeThong','U') IS NULL
CREATE TABLE dbo.ThamSoHeThong(
    MaThamSo VARCHAR(30) CONSTRAINT PK_ThamSo PRIMARY KEY,
    GiaTri DECIMAL(18,4) NOT NULL,
    DienGiai NVARCHAR(200) NOT NULL
);
IF OBJECT_ID('dbo.NhanVien','U') IS NULL
CREATE TABLE dbo.NhanVien(
    MaNV VARCHAR(10) CONSTRAINT PK_NhanVien PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh NVARCHAR(10) NOT NULL,
    CCCD VARCHAR(20) NOT NULL CONSTRAINT UQ_NhanVien_CCCD UNIQUE,
    DienThoai VARCHAR(15) NULL,
    DiaChi NVARCHAR(200) NULL,
    MaPhongBan INT NULL CONSTRAINT FK_NhanVien_PhongBan REFERENCES dbo.PhongBan(MaPhongBan),
    TrangThai NVARCHAR(30) NOT NULL CONSTRAINT DF_NhanVien_TrangThai DEFAULT N'Thu viec',
    CONSTRAINT CK_NhanVien_TrangThai CHECK(TrangThai IN(N'Thu viec',N'Chinh thuc',N'Nghi viec'))
);
IF OBJECT_ID('dbo.NhuCauTuyenDung','U') IS NULL
CREATE TABLE dbo.NhuCauTuyenDung(
    MaNhuCau INT IDENTITY(1,1) CONSTRAINT PK_NhuCau PRIMARY KEY,
    MaPhongBan INT NOT NULL CONSTRAINT FK_NhuCau_PhongBan REFERENCES dbo.PhongBan(MaPhongBan),
    ViTri NVARCHAR(100) NOT NULL, SoLuong INT NOT NULL, NgayYeuCau DATE NOT NULL,
    TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_NhuCau_TrangThai DEFAULT N'Dang tuyen',
    CONSTRAINT CK_NhuCau_SoLuong CHECK(SoLuong > 0)
);
IF OBJECT_ID('dbo.QuyetDinhTuyenDung','U') IS NULL
CREATE TABLE dbo.QuyetDinhTuyenDung(
    MaQDTD INT IDENTITY(1,1) CONSTRAINT PK_QDTD PRIMARY KEY,
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_QDTD_NhanVien REFERENCES dbo.NhanVien(MaNV),
    MaNhuCau INT NULL CONSTRAINT FK_QDTD_NhuCau REFERENCES dbo.NhuCauTuyenDung(MaNhuCau),
    NgayQuyetDinh DATE NOT NULL, TuNgay DATE NOT NULL, DenNgayThuViec DATE NOT NULL,
    LuongThuViec DECIMAL(18,2) NOT NULL, DieuKhoan NVARCHAR(1000) NULL,
    KetQua NVARCHAR(20) NOT NULL CONSTRAINT DF_QDTD_KetQua DEFAULT N'Cho danh gia',
    CONSTRAINT CK_QDTD_ThoiGian CHECK(DenNgayThuViec >= TuNgay),
    CONSTRAINT CK_QDTD_Luong CHECK(LuongThuViec > 0),
    CONSTRAINT CK_QDTD_KetQua CHECK(KetQua IN(N'Cho danh gia',N'Dat',N'Khong dat'))
);
IF OBJECT_ID('dbo.HopDongLaoDong','U') IS NULL
CREATE TABLE dbo.HopDongLaoDong(
    MaHD INT IDENTITY(1,1) CONSTRAINT PK_HopDong PRIMARY KEY,
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_HopDong_NhanVien REFERENCES dbo.NhanVien(MaNV),
    LoaiHopDong NVARCHAR(20) NOT NULL, NgayKy DATE NOT NULL, TuNgay DATE NOT NULL, DenNgay DATE NULL,
    TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_HopDong_TrangThai DEFAULT N'Hieu luc',
    CONSTRAINT CK_HopDong_Loai CHECK(LoaiHopDong IN(N'Ngan han',N'Dai han')),
    CONSTRAINT CK_HopDong_ThoiGian CHECK(DenNgay IS NULL OR DenNgay >= TuNgay)
);
IF OBJECT_ID('dbo.BaoHiem','U') IS NULL
CREATE TABLE dbo.BaoHiem(
    MaBH INT IDENTITY(1,1) CONSTRAINT PK_BaoHiem PRIMARY KEY,
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_BaoHiem_NhanVien REFERENCES dbo.NhanVien(MaNV),
    MaHD INT NOT NULL CONSTRAINT UQ_BaoHiem_HopDong UNIQUE CONSTRAINT FK_BaoHiem_HopDong REFERENCES dbo.HopDongLaoDong(MaHD),
    SoBHXH VARCHAR(20) NOT NULL CONSTRAINT UQ_BaoHiem_BHXH UNIQUE,
    SoBHYT VARCHAR(20) NOT NULL CONSTRAINT UQ_BaoHiem_BHYT UNIQUE,
    TyLeBHXH DECIMAL(5,2) NOT NULL, TyLeBHYT DECIMAL(5,2) NOT NULL, TuNgay DATE NOT NULL,
    TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_BaoHiem_TrangThai DEFAULT N'Hieu luc',
    CONSTRAINT CK_BaoHiem_TyLe CHECK(TyLeBHXH BETWEEN 0 AND 100 AND TyLeBHYT BETWEEN 0 AND 100)
);
IF OBJECT_ID('dbo.QuyetDinhLuong','U') IS NULL
CREATE TABLE dbo.QuyetDinhLuong(
    MaQDL INT IDENTITY(1,1) CONSTRAINT PK_QDL PRIMARY KEY,
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_QDL_NhanVien REFERENCES dbo.NhanVien(MaNV),
    NgayQuyetDinh DATE NOT NULL, NgayHieuLuc DATE NOT NULL,
    LuongCoBan DECIMAL(18,2) NOT NULL, PhuCapLuong DECIMAL(18,2) NOT NULL CONSTRAINT DF_QDL_PCL DEFAULT 0,
    PhuCapDocHai DECIMAL(18,2) NOT NULL CONSTRAINT DF_QDL_DH DEFAULT 0,
    CONSTRAINT CK_QDL_Tien CHECK(LuongCoBan > 0 AND PhuCapLuong >= 0 AND PhuCapDocHai >= 0)
);
IF OBJECT_ID('dbo.QuyetDinhChucVu','U') IS NULL
CREATE TABLE dbo.QuyetDinhChucVu(
    MaQDCV INT IDENTITY(1,1) CONSTRAINT PK_QDCV PRIMARY KEY,
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_QDCV_NhanVien REFERENCES dbo.NhanVien(MaNV),
    MaChucVu INT NOT NULL CONSTRAINT FK_QDCV_ChucVu REFERENCES dbo.ChucVu(MaChucVu),
    NgayQuyetDinh DATE NOT NULL, NgayHieuLuc DATE NOT NULL, PhuCapChucVu DECIMAL(18,2) NOT NULL,
    CONSTRAINT CK_QDCV_PhuCap CHECK(PhuCapChucVu >= 0)
);
IF OBJECT_ID('dbo.KeHoachTuan','U') IS NULL
CREATE TABLE dbo.KeHoachTuan(
    MaKeHoach INT IDENTITY(1,1) CONSTRAINT PK_KeHoach PRIMARY KEY,
    MaPhongBan INT NOT NULL CONSTRAINT FK_KeHoach_PhongBan REFERENCES dbo.PhongBan(MaPhongBan),
    TuNgay DATE NOT NULL, DenNgay DATE NOT NULL, NguoiLap NVARCHAR(100) NOT NULL,
    CONSTRAINT UQ_KeHoach_Phong_Tuan UNIQUE(MaPhongBan,TuNgay),
    CONSTRAINT CK_KeHoach_Tuan CHECK(DATEDIFF(DAY,TuNgay,DenNgay)=6)
);
IF OBJECT_ID('dbo.ChiTietKeHoach','U') IS NULL
CREATE TABLE dbo.ChiTietKeHoach(
    MaCTKH INT IDENTITY(1,1) CONSTRAINT PK_ChiTietKeHoach PRIMARY KEY,
    MaKeHoach INT NOT NULL CONSTRAINT FK_CTKH_KeHoach REFERENCES dbo.KeHoachTuan(MaKeHoach),
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_CTKH_NhanVien REFERENCES dbo.NhanVien(MaNV),
    NgayLam DATE NOT NULL, MaCa INT NOT NULL CONSTRAINT FK_CTKH_Ca REFERENCES dbo.CaLamViec(MaCa),
    DuKienPhuTroi BIT NOT NULL CONSTRAINT DF_CTKH_PhuTroi DEFAULT 0,
    CONSTRAINT UQ_CTKH_NhanVien_Ngay_Ca UNIQUE(MaNV,NgayLam,MaCa)
);
IF OBJECT_ID('dbo.BangChamCong','U') IS NULL
CREATE TABLE dbo.BangChamCong(
    MaBangCong INT IDENTITY(1,1) CONSTRAINT PK_BangChamCong PRIMARY KEY,
    Thang INT NOT NULL, Nam INT NOT NULL, TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_BCC_TrangThai DEFAULT N'Dang lap',
    CONSTRAINT UQ_BCC_ThangNam UNIQUE(Thang,Nam),
    CONSTRAINT CK_BCC_Thang CHECK(Thang BETWEEN 1 AND 12),
    CONSTRAINT CK_BCC_TrangThai CHECK(TrangThai IN(N'Dang lap',N'Da chot'))
);
IF OBJECT_ID('dbo.ChiTietChamCong','U') IS NULL
CREATE TABLE dbo.ChiTietChamCong(
    MaCTCC INT IDENTITY(1,1) CONSTRAINT PK_ChiTietChamCong PRIMARY KEY,
    MaBangCong INT NOT NULL CONSTRAINT FK_CTCC_BangCong REFERENCES dbo.BangChamCong(MaBangCong),
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_CTCC_NhanVien REFERENCES dbo.NhanVien(MaNV),
    NgayCong DATE NOT NULL, MaCa INT NOT NULL CONSTRAINT FK_CTCC_Ca REFERENCES dbo.CaLamViec(MaCa),
    GioVao TIME NULL, GioRa TIME NULL, SoNgayCong DECIMAL(4,2) NOT NULL,
    TrangThai NVARCHAR(30) NOT NULL,
    CONSTRAINT UQ_CTCC_NhanVien_Ngay UNIQUE(MaNV,NgayCong),
    CONSTRAINT CK_CTCC_NgayCong CHECK(SoNgayCong BETWEEN 0 AND 1)
);
IF OBJECT_ID('dbo.PhieuPhuTroi','U') IS NULL
CREATE TABLE dbo.PhieuPhuTroi(
    MaPhieuPT INT IDENTITY(1,1) CONSTRAINT PK_PhieuPhuTroi PRIMARY KEY,
    NgayPhuTroi DATE NOT NULL, HinhThuc NVARCHAR(20) NOT NULL, HeSo DECIMAL(4,2) NOT NULL,
    NguoiLap NVARCHAR(100) NOT NULL,
    CONSTRAINT CK_PhieuPT_HinhThuc CHECK(HinhThuc IN(N'Tang ca',N'Ngay le',N'Ngay nghi')),
    CONSTRAINT CK_PhieuPT_HeSo CHECK(HeSo > 0)
);
IF OBJECT_ID('dbo.ChiTietPhuTroi','U') IS NULL
CREATE TABLE dbo.ChiTietPhuTroi(
    MaCTPT INT IDENTITY(1,1) CONSTRAINT PK_ChiTietPhuTroi PRIMARY KEY,
    MaPhieuPT INT NOT NULL CONSTRAINT FK_CTPT_Phieu REFERENCES dbo.PhieuPhuTroi(MaPhieuPT),
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_CTPT_NhanVien REFERENCES dbo.NhanVien(MaNV),
    SoGio DECIMAL(5,2) NOT NULL,
    CONSTRAINT UQ_CTPT_Phieu_NV UNIQUE(MaPhieuPT,MaNV),
    CONSTRAINT CK_CTPT_SoGio CHECK(SoGio > 0)
);
IF OBJECT_ID('dbo.QuyetDinhKhenThuong','U') IS NULL
CREATE TABLE dbo.QuyetDinhKhenThuong(
    MaQDKT INT IDENTITY(1,1) CONSTRAINT PK_QDKT PRIMARY KEY,
    DoiTuong NVARCHAR(20) NOT NULL, MaNV VARCHAR(10) NULL CONSTRAINT FK_QDKT_NhanVien REFERENCES dbo.NhanVien(MaNV),
    MaPhongBan INT NULL CONSTRAINT FK_QDKT_PhongBan REFERENCES dbo.PhongBan(MaPhongBan),
    NgayQuyetDinh DATE NOT NULL, LyDo NVARCHAR(500) NOT NULL, SoTien DECIMAL(18,2) NOT NULL,
    HinhThucChi NVARCHAR(20) NOT NULL, NguoiKy NVARCHAR(100) NOT NULL,
    CONSTRAINT CK_QDKT_DoiTuong CHECK((DoiTuong=N'Ca nhan' AND MaNV IS NOT NULL AND MaPhongBan IS NULL) OR (DoiTuong=N'Tap the' AND MaNV IS NULL AND MaPhongBan IS NOT NULL)),
    CONSTRAINT CK_QDKT_Tien CHECK(SoTien > 0)
);
IF OBJECT_ID('dbo.PhieuChiKhenThuong','U') IS NULL
CREATE TABLE dbo.PhieuChiKhenThuong(
    MaPhieuChi INT IDENTITY(1,1) CONSTRAINT PK_PhieuChiKT PRIMARY KEY,
    MaQDKT INT NOT NULL CONSTRAINT UQ_PhieuChi_QD UNIQUE CONSTRAINT FK_PhieuChi_QD REFERENCES dbo.QuyetDinhKhenThuong(MaQDKT),
    NgayChi DATE NOT NULL, SoTien DECIMAL(18,2) NOT NULL, TrangThai NVARCHAR(20) NOT NULL
);
IF OBJECT_ID('dbo.QuyetDinhKyLuat','U') IS NULL
CREATE TABLE dbo.QuyetDinhKyLuat(
    MaQDKL INT IDENTITY(1,1) CONSTRAINT PK_QDKL PRIMARY KEY,
    DoiTuong NVARCHAR(20) NOT NULL, MaNV VARCHAR(10) NULL CONSTRAINT FK_QDKL_NhanVien REFERENCES dbo.NhanVien(MaNV),
    MaPhongBan INT NULL CONSTRAINT FK_QDKL_PhongBan REFERENCES dbo.PhongBan(MaPhongBan),
    NgayQuyetDinh DATE NOT NULL, LyDo NVARCHAR(500) NOT NULL, SoTien DECIMAL(18,2) NOT NULL,
    HinhThucThu NVARCHAR(20) NOT NULL, NguoiKy NVARCHAR(100) NOT NULL,
    CONSTRAINT CK_QDKL_DoiTuong CHECK((DoiTuong=N'Ca nhan' AND MaNV IS NOT NULL AND MaPhongBan IS NULL) OR (DoiTuong=N'Tap the' AND MaNV IS NULL AND MaPhongBan IS NOT NULL)),
    CONSTRAINT CK_QDKL_Tien CHECK(SoTien > 0)
);
IF OBJECT_ID('dbo.PhieuThuNopPhat','U') IS NULL
CREATE TABLE dbo.PhieuThuNopPhat(
    MaPhieuThu INT IDENTITY(1,1) CONSTRAINT PK_PhieuThuPhat PRIMARY KEY,
    MaQDKL INT NOT NULL CONSTRAINT UQ_PhieuThu_QD UNIQUE CONSTRAINT FK_PhieuThu_QD REFERENCES dbo.QuyetDinhKyLuat(MaQDKL),
    NgayThu DATE NULL, SoTien DECIMAL(18,2) NOT NULL, TrangThai NVARCHAR(20) NOT NULL
);
IF OBJECT_ID('dbo.BangLuong','U') IS NULL
CREATE TABLE dbo.BangLuong(
    MaBangLuong INT IDENTITY(1,1) CONSTRAINT PK_BangLuong PRIMARY KEY,
    Thang INT NOT NULL, Nam INT NOT NULL, NgayLap DATETIME2 NOT NULL CONSTRAINT DF_BangLuong_Ngay DEFAULT SYSDATETIME(),
    TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_BangLuong_TrangThai DEFAULT N'Dang lap',
    CONSTRAINT UQ_BangLuong_ThangNam UNIQUE(Thang,Nam),
    CONSTRAINT CK_BangLuong_Thang CHECK(Thang BETWEEN 1 AND 12)
);
IF OBJECT_ID('dbo.ChiTietBangLuong','U') IS NULL
CREATE TABLE dbo.ChiTietBangLuong(
    MaCTBL INT IDENTITY(1,1) CONSTRAINT PK_ChiTietBangLuong PRIMARY KEY,
    MaBangLuong INT NOT NULL CONSTRAINT FK_CTBL_BangLuong REFERENCES dbo.BangLuong(MaBangLuong),
    MaNV VARCHAR(10) NOT NULL CONSTRAINT FK_CTBL_NhanVien REFERENCES dbo.NhanVien(MaNV),
    SoNgayCong DECIMAL(6,2) NOT NULL, LuongCoBan DECIMAL(18,2) NOT NULL,
    LuongTheoCong DECIMAL(18,2) NOT NULL, TongPhuCap DECIMAL(18,2) NOT NULL,
    SoGioPhuTroi DECIMAL(8,2) NOT NULL, TienPhuTroi DECIMAL(18,2) NOT NULL,
    TienThuong DECIMAL(18,2) NOT NULL, TienPhat DECIMAL(18,2) NOT NULL,
    KhauTruBHXH DECIMAL(18,2) NOT NULL, KhauTruBHYT DECIMAL(18,2) NOT NULL,
    ThucLinh DECIMAL(18,2) NOT NULL,
    CONSTRAINT UQ_CTBL_BangLuong_NV UNIQUE(MaBangLuong,MaNV)
);
GO

MERGE dbo.ThamSoHeThong AS t USING (VALUES
 ('NGAY_CONG_CHUAN',26,N'So ngay cong chuan mot thang'),
 ('GIO_MOT_NGAY',8,N'So gio lam viec mot ngay'),
 ('HE_SO_PHU_TROI',1.5,N'He so gio phu troi theo de bai'),
 ('TY_LE_BHXH',8,N'Ty le BHXH nguoi lao dong'),
 ('TY_LE_BHYT',1.5,N'Ty le BHYT nguoi lao dong')) s(MaThamSo,GiaTri,DienGiai)
ON t.MaThamSo=s.MaThamSo WHEN NOT MATCHED THEN INSERT VALUES(s.MaThamSo,s.GiaTri,s.DienGiai);
IF NOT EXISTS(SELECT 1 FROM dbo.PhongBan) INSERT dbo.PhongBan(TenPhongBan) VALUES(N'Tai chinh - Ke toan'),(N'Ky thuat'),(N'San xuat'),(N'Nhan su');
IF NOT EXISTS(SELECT 1 FROM dbo.ChucVu) INSERT dbo.ChucVu(TenChucVu) VALUES(N'Nhan vien'),(N'Truong phong'),(N'Quan doc');
IF NOT EXISTS(SELECT 1 FROM dbo.CaLamViec) INSERT dbo.CaLamViec(TenCa,GioBatDau,GioKetThuc) VALUES(N'Ca sang','07:30','11:30'),(N'Ca chieu','13:00','17:00'),(N'Ca toi','18:00','22:00');
IF NOT EXISTS(SELECT 1 FROM dbo.NhuCauTuyenDung) INSERT dbo.NhuCauTuyenDung(MaPhongBan,ViTri,SoLuong,NgayYeuCau) VALUES(1,N'Nhan vien ke toan',2,'2026-10-01'),(3,N'Nhan vien san xuat',5,'2026-10-01');
GO

CREATE OR ALTER PROCEDURE dbo.sp_TiepNhanThuViec
 @HoTen NVARCHAR(100), @NgaySinh DATE, @GioiTinh NVARCHAR(10), @CCCD VARCHAR(20),
 @DienThoai VARCHAR(15), @DiaChi NVARCHAR(200), @MaPhongBan INT, @MaNhuCau INT=NULL,
 @TuNgay DATE, @DenNgayThuViec DATE, @LuongThuViec DECIMAL(18,2), @DieuKhoan NVARCHAR(1000),
 @MaNV VARCHAR(10) OUTPUT
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 IF @DenNgayThuViec < @TuNgay THROW 51001,N'Ngay ket thuc thu viec khong hop le.',1;
 IF EXISTS(SELECT 1 FROM dbo.NhanVien WHERE CCCD=@CCCD) THROW 51002,N'CCCD da ton tai.',1;
 DECLARE @stt INT=ISNULL((SELECT MAX(TRY_CONVERT(INT,SUBSTRING(MaNV,3,8))) FROM dbo.NhanVien WITH(UPDLOCK,HOLDLOCK)),0)+1;
 SET @MaNV='NV'+RIGHT('000000'+CONVERT(VARCHAR(6),@stt),6);
 INSERT dbo.NhanVien(MaNV,HoTen,NgaySinh,GioiTinh,CCCD,DienThoai,DiaChi,MaPhongBan) VALUES(@MaNV,@HoTen,@NgaySinh,@GioiTinh,@CCCD,@DienThoai,@DiaChi,@MaPhongBan);
 INSERT dbo.QuyetDinhTuyenDung(MaNV,MaNhuCau,NgayQuyetDinh,TuNgay,DenNgayThuViec,LuongThuViec,DieuKhoan) VALUES(@MaNV,@MaNhuCau,CAST(GETDATE() AS DATE),@TuNgay,@DenNgayThuViec,@LuongThuViec,@DieuKhoan);
 COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_DanhGiaThuViec @MaNV VARCHAR(10), @KetQua NVARCHAR(20)
AS
BEGIN
 IF @KetQua NOT IN(N'Dat',N'Khong dat') THROW 51003,N'Ket qua thu viec khong hop le.',1;
 UPDATE dbo.QuyetDinhTuyenDung SET KetQua=@KetQua WHERE MaQDTD=(SELECT TOP 1 MaQDTD FROM dbo.QuyetDinhTuyenDung WHERE MaNV=@MaNV ORDER BY MaQDTD DESC);
 IF @@ROWCOUNT=0 THROW 51004,N'Khong tim thay quyet dinh tuyen dung.',1;
 IF @KetQua=N'Khong dat' UPDATE dbo.NhanVien SET TrangThai=N'Nghi viec' WHERE MaNV=@MaNV;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_KyHopDongChinhThuc
 @MaNV VARCHAR(10), @LoaiHopDong NVARCHAR(20), @NgayKy DATE, @TuNgay DATE, @DenNgay DATE=NULL, @MaHD INT OUTPUT
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 DECLARE @HetThuViec DATE,@KetQua NVARCHAR(20);
 SELECT TOP 1 @HetThuViec=DenNgayThuViec,@KetQua=KetQua FROM dbo.QuyetDinhTuyenDung WHERE MaNV=@MaNV ORDER BY MaQDTD DESC;
 IF @KetQua<>N'Dat' OR @TuNgay<=@HetThuViec THROW 51005,N'Chi ky hop dong sau khi thu viec dat va da ket thuc.',1;
 IF EXISTS(SELECT 1 FROM dbo.HopDongLaoDong WHERE MaNV=@MaNV AND TrangThai=N'Hieu luc') THROW 51006,N'Nhan vien da co hop dong hieu luc.',1;
 INSERT dbo.HopDongLaoDong(MaNV,LoaiHopDong,NgayKy,TuNgay,DenNgay) VALUES(@MaNV,@LoaiHopDong,@NgayKy,@TuNgay,@DenNgay);
 SET @MaHD=SCOPE_IDENTITY(); UPDATE dbo.NhanVien SET TrangThai=N'Chinh thuc' WHERE MaNV=@MaNV; COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_TaoHoSoBaoHiem
 @MaNV VARCHAR(10), @SoBHXH VARCHAR(20), @SoBHYT VARCHAR(20), @TuNgay DATE
AS
BEGIN
 DECLARE @MaHD INT=(SELECT TOP 1 MaHD FROM dbo.HopDongLaoDong WHERE MaNV=@MaNV AND LoaiHopDong=N'Dai han' AND TrangThai=N'Hieu luc' ORDER BY TuNgay DESC);
 IF @MaHD IS NULL THROW 51007,N'Bao hiem chi ap dung cho hop dong dai han.',1;
 IF EXISTS(SELECT 1 FROM dbo.BaoHiem WHERE MaHD=@MaHD) THROW 51008,N'Hop dong da co ho so bao hiem.',1;
 INSERT dbo.BaoHiem(MaNV,MaHD,SoBHXH,SoBHYT,TyLeBHXH,TyLeBHYT,TuNgay)
 SELECT @MaNV,@MaHD,@SoBHXH,@SoBHYT,(SELECT GiaTri FROM dbo.ThamSoHeThong WHERE MaThamSo='TY_LE_BHXH'),(SELECT GiaTri FROM dbo.ThamSoHeThong WHERE MaThamSo='TY_LE_BHYT'),@TuNgay;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_BanHanhQuyetDinhLuong
 @MaNV VARCHAR(10),@NgayHieuLuc DATE,@LuongCoBan DECIMAL(18,2),@PhuCapLuong DECIMAL(18,2),@PhuCapDocHai DECIMAL(18,2)
AS
BEGIN
 IF EXISTS(SELECT 1 FROM dbo.QuyetDinhLuong WHERE MaNV=@MaNV AND NgayHieuLuc=@NgayHieuLuc) THROW 51009,N'Da co quyet dinh luong cung ngay hieu luc.',1;
 INSERT dbo.QuyetDinhLuong(MaNV,NgayQuyetDinh,NgayHieuLuc,LuongCoBan,PhuCapLuong,PhuCapDocHai) VALUES(@MaNV,CAST(GETDATE() AS DATE),@NgayHieuLuc,@LuongCoBan,@PhuCapLuong,@PhuCapDocHai);
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_BanHanhQuyetDinhChucVu
 @MaNV VARCHAR(10),@MaChucVu INT,@NgayHieuLuc DATE,@PhuCapChucVu DECIMAL(18,2)
AS
BEGIN
 IF EXISTS(SELECT 1 FROM dbo.QuyetDinhChucVu WHERE MaNV=@MaNV AND NgayHieuLuc=@NgayHieuLuc) THROW 51010,N'Da co quyet dinh chuc vu cung ngay hieu luc.',1;
 INSERT dbo.QuyetDinhChucVu(MaNV,MaChucVu,NgayQuyetDinh,NgayHieuLuc,PhuCapChucVu) VALUES(@MaNV,@MaChucVu,CAST(GETDATE() AS DATE),@NgayHieuLuc,@PhuCapChucVu);
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_LapKeHoachTuan
 @MaPhongBan INT,@TuNgay DATE,@NguoiLap NVARCHAR(100),@MaNV VARCHAR(10),@NgayLam DATE,@MaCa INT,@DuKienPhuTroi BIT
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 IF DATEDIFF(DAY,'19000101',@TuNgay)%7<>0 THROW 51011,N'Tu ngay phai la thu Hai.',1;
 IF @NgayLam<@TuNgay OR @NgayLam>DATEADD(DAY,6,@TuNgay) THROW 51012,N'Ngay lam nam ngoai tuan ke hoach.',1;
 DECLARE @MaKeHoach INT=(SELECT MaKeHoach FROM dbo.KeHoachTuan WHERE MaPhongBan=@MaPhongBan AND TuNgay=@TuNgay);
 IF @MaKeHoach IS NULL BEGIN INSERT dbo.KeHoachTuan(MaPhongBan,TuNgay,DenNgay,NguoiLap) VALUES(@MaPhongBan,@TuNgay,DATEADD(DAY,6,@TuNgay),@NguoiLap); SET @MaKeHoach=SCOPE_IDENTITY(); END;
 INSERT dbo.ChiTietKeHoach(MaKeHoach,MaNV,NgayLam,MaCa,DuKienPhuTroi) VALUES(@MaKeHoach,@MaNV,@NgayLam,@MaCa,@DuKienPhuTroi); COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_GhiNhanChamCong
 @MaNV VARCHAR(10),@NgayCong DATE,@MaCa INT,@SoNgayCong DECIMAL(4,2),@TrangThai NVARCHAR(30)
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 DECLARE @MaBangCong INT=(SELECT MaBangCong FROM dbo.BangChamCong WHERE Thang=MONTH(@NgayCong) AND Nam=YEAR(@NgayCong));
 IF @MaBangCong IS NULL BEGIN INSERT dbo.BangChamCong(Thang,Nam) VALUES(MONTH(@NgayCong),YEAR(@NgayCong)); SET @MaBangCong=SCOPE_IDENTITY(); END;
 IF EXISTS(SELECT 1 FROM dbo.BangChamCong WHERE MaBangCong=@MaBangCong AND TrangThai=N'Da chot') THROW 51013,N'Bang cham cong da chot.',1;
 INSERT dbo.ChiTietChamCong(MaBangCong,MaNV,NgayCong,MaCa,SoNgayCong,TrangThai) VALUES(@MaBangCong,@MaNV,@NgayCong,@MaCa,@SoNgayCong,@TrangThai); COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_ChotChamCong @Thang INT,@Nam INT
AS
BEGIN
 UPDATE dbo.BangChamCong SET TrangThai=N'Da chot' WHERE Thang=@Thang AND Nam=@Nam;
 IF @@ROWCOUNT=0 THROW 51014,N'Khong co bang cham cong de chot.',1;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_LapPhieuPhuTroi
 @MaNV VARCHAR(10),@NgayPhuTroi DATE,@HinhThuc NVARCHAR(20),@SoGio DECIMAL(5,2),@NguoiLap NVARCHAR(100)
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 DECLARE @HeSo DECIMAL(4,2)=(SELECT GiaTri FROM dbo.ThamSoHeThong WHERE MaThamSo='HE_SO_PHU_TROI');
 INSERT dbo.PhieuPhuTroi(NgayPhuTroi,HinhThuc,HeSo,NguoiLap) VALUES(@NgayPhuTroi,@HinhThuc,@HeSo,@NguoiLap);
 INSERT dbo.ChiTietPhuTroi(MaPhieuPT,MaNV,SoGio) VALUES(SCOPE_IDENTITY(),@MaNV,@SoGio); COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_LapQuyetDinhKhenThuong
 @DoiTuong NVARCHAR(20),@MaNV VARCHAR(10)=NULL,@MaPhongBan INT=NULL,@Ngay DATE,@LyDo NVARCHAR(500),@SoTien DECIMAL(18,2),@HinhThuc NVARCHAR(20),@NguoiKy NVARCHAR(100)
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 INSERT dbo.QuyetDinhKhenThuong(DoiTuong,MaNV,MaPhongBan,NgayQuyetDinh,LyDo,SoTien,HinhThucChi,NguoiKy) VALUES(@DoiTuong,@MaNV,@MaPhongBan,@Ngay,@LyDo,@SoTien,@HinhThuc,@NguoiKy);
 DECLARE @id INT=SCOPE_IDENTITY(); INSERT dbo.PhieuChiKhenThuong(MaQDKT,NgayChi,SoTien,TrangThai) VALUES(@id,@Ngay,@SoTien,CASE WHEN @HinhThuc=N'Cong luong' THEN N'Cho tinh luong' ELSE N'Da chi' END); COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_LapQuyetDinhKyLuat
 @DoiTuong NVARCHAR(20),@MaNV VARCHAR(10)=NULL,@MaPhongBan INT=NULL,@Ngay DATE,@LyDo NVARCHAR(500),@SoTien DECIMAL(18,2),@HinhThuc NVARCHAR(20),@NguoiKy NVARCHAR(100)
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 INSERT dbo.QuyetDinhKyLuat(DoiTuong,MaNV,MaPhongBan,NgayQuyetDinh,LyDo,SoTien,HinhThucThu,NguoiKy) VALUES(@DoiTuong,@MaNV,@MaPhongBan,@Ngay,@LyDo,@SoTien,@HinhThuc,@NguoiKy);
 DECLARE @id INT=SCOPE_IDENTITY(); INSERT dbo.PhieuThuNopPhat(MaQDKL,NgayThu,SoTien,TrangThai) VALUES(@id,CASE WHEN @HinhThuc=N'Tru truc tiep' THEN @Ngay ELSE NULL END,@SoTien,CASE WHEN @HinhThuc=N'Tru luong' THEN N'Cho tru luong' ELSE N'Da thu' END); COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_TinhLuongThang @Thang INT,@Nam INT
AS
BEGIN
 SET XACT_ABORT ON; BEGIN TRAN;
 DECLARE @MaBangCong INT=(SELECT MaBangCong FROM dbo.BangChamCong WHERE Thang=@Thang AND Nam=@Nam AND TrangThai=N'Da chot');
 IF @MaBangCong IS NULL THROW 51015,N'Bang cham cong chua duoc chot.',1;
 IF EXISTS(SELECT 1 FROM dbo.BangLuong WHERE Thang=@Thang AND Nam=@Nam) THROW 51016,N'Bang luong thang da ton tai.',1;
 DECLARE @MaBangLuong INT,@NgayCongChuan DECIMAL(18,4)=(SELECT GiaTri FROM dbo.ThamSoHeThong WHERE MaThamSo='NGAY_CONG_CHUAN'),@GioNgay DECIMAL(18,4)=(SELECT GiaTri FROM dbo.ThamSoHeThong WHERE MaThamSo='GIO_MOT_NGAY');
 INSERT dbo.BangLuong(Thang,Nam) VALUES(@Thang,@Nam); SET @MaBangLuong=SCOPE_IDENTITY();
 ;WITH Cong AS(SELECT MaNV,SUM(SoNgayCong) SoNgayCong FROM dbo.ChiTietChamCong WHERE MaBangCong=@MaBangCong GROUP BY MaNV),
 Luong AS(SELECT c.MaNV,c.SoNgayCong,q.LuongCoBan,q.PhuCapLuong,q.PhuCapDocHai FROM Cong c CROSS APPLY(SELECT TOP 1 * FROM dbo.QuyetDinhLuong q WHERE q.MaNV=c.MaNV AND q.NgayHieuLuc<=EOMONTH(DATEFROMPARTS(@Nam,@Thang,1)) ORDER BY q.NgayHieuLuc DESC,q.MaQDL DESC) q),
 CV AS(SELECT l.*,ISNULL((SELECT TOP 1 PhuCapChucVu FROM dbo.QuyetDinhChucVu v WHERE v.MaNV=l.MaNV AND v.NgayHieuLuc<=EOMONTH(DATEFROMPARTS(@Nam,@Thang,1)) ORDER BY v.NgayHieuLuc DESC,v.MaQDCV DESC),0) PhuCapChucVu FROM Luong l),
 PT AS(SELECT ct.MaNV,SUM(ct.SoGio) SoGio,SUM(ct.SoGio*p.HeSo) GioQuyDoi FROM dbo.ChiTietPhuTroi ct JOIN dbo.PhieuPhuTroi p ON p.MaPhieuPT=ct.MaPhieuPT WHERE MONTH(p.NgayPhuTroi)=@Thang AND YEAR(p.NgayPhuTroi)=@Nam GROUP BY ct.MaNV),
 KT AS(SELECT MaNV,SUM(SoTien) Tien FROM dbo.QuyetDinhKhenThuong WHERE DoiTuong=N'Ca nhan' AND HinhThucChi=N'Cong luong' AND MONTH(NgayQuyetDinh)=@Thang AND YEAR(NgayQuyetDinh)=@Nam GROUP BY MaNV),
 KL AS(SELECT MaNV,SUM(SoTien) Tien FROM dbo.QuyetDinhKyLuat WHERE DoiTuong=N'Ca nhan' AND HinhThucThu=N'Tru luong' AND MONTH(NgayQuyetDinh)=@Thang AND YEAR(NgayQuyetDinh)=@Nam GROUP BY MaNV)
 INSERT dbo.ChiTietBangLuong(MaBangLuong,MaNV,SoNgayCong,LuongCoBan,LuongTheoCong,TongPhuCap,SoGioPhuTroi,TienPhuTroi,TienThuong,TienPhat,KhauTruBHXH,KhauTruBHYT,ThucLinh)
 SELECT @MaBangLuong,c.MaNV,c.SoNgayCong,c.LuongCoBan,
  ROUND(c.LuongCoBan*c.SoNgayCong/@NgayCongChuan,0),ROUND(c.PhuCapLuong+c.PhuCapDocHai+c.PhuCapChucVu,0),ISNULL(p.SoGio,0),
  ROUND(c.LuongCoBan/(@NgayCongChuan*@GioNgay)*ISNULL(p.GioQuyDoi,0),0),ISNULL(k.Tien,0),ISNULL(l.Tien,0),
  ROUND(CASE WHEN b.MaBH IS NULL THEN 0 ELSE c.LuongCoBan*b.TyLeBHXH/100 END,0),ROUND(CASE WHEN b.MaBH IS NULL THEN 0 ELSE c.LuongCoBan*b.TyLeBHYT/100 END,0),
  ROUND(c.LuongCoBan*c.SoNgayCong/@NgayCongChuan+c.PhuCapLuong+c.PhuCapDocHai+c.PhuCapChucVu+c.LuongCoBan/(@NgayCongChuan*@GioNgay)*ISNULL(p.GioQuyDoi,0)+ISNULL(k.Tien,0)-ISNULL(l.Tien,0)-CASE WHEN b.MaBH IS NULL THEN 0 ELSE c.LuongCoBan*(b.TyLeBHXH+b.TyLeBHYT)/100 END,0)
 FROM CV c LEFT JOIN PT p ON p.MaNV=c.MaNV LEFT JOIN KT k ON k.MaNV=c.MaNV LEFT JOIN KL l ON l.MaNV=c.MaNV LEFT JOIN dbo.BaoHiem b ON b.MaNV=c.MaNV AND b.TrangThai=N'Hieu luc';
 COMMIT;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_ChotBangLuong @Thang INT,@Nam INT
AS
BEGIN
 UPDATE dbo.BangLuong SET TrangThai=N'Da chot' WHERE Thang=@Thang AND Nam=@Nam AND TrangThai=N'Dang lap';
 IF @@ROWCOUNT=0 THROW 51017,N'Bang luong khong ton tai hoac da chot.',1;
END
GO
