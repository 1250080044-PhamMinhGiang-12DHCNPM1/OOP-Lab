/* BAI 6 - QUAN LY CONG TY DU LICH VAN HOA VIET */
IF DB_ID(N'QuanLyCongTyDuLich') IS NULL
    CREATE DATABASE QuanLyCongTyDuLich;
GO
USE QuanLyCongTyDuLich;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('dbo.KhaoSat','U') IS NOT NULL DROP TABLE dbo.KhaoSat;
IF OBJECT_ID('dbo.ThanhToanDoan','U') IS NOT NULL DROP TABLE dbo.ThanhToanDoan;
IF OBJECT_ID('dbo.PhanCongHDV','U') IS NOT NULL DROP TABLE dbo.PhanCongHDV;
IF OBJECT_ID('dbo.DangKyLe','U') IS NOT NULL DROP TABLE dbo.DangKyLe;
IF OBJECT_ID('dbo.ThanhVienDoan','U') IS NOT NULL DROP TABLE dbo.ThanhVienDoan;
IF OBJECT_ID('dbo.DangKyDoan','U') IS NOT NULL DROP TABLE dbo.DangKyDoan;
IF OBJECT_ID('dbo.DoanKhach','U') IS NOT NULL DROP TABLE dbo.DoanKhach;
IF OBJECT_ID('dbo.ChuyenLe','U') IS NOT NULL DROP TABLE dbo.ChuyenLe;
IF OBJECT_ID('dbo.TourDiemThamQuan','U') IS NOT NULL DROP TABLE dbo.TourDiemThamQuan;
IF OBJECT_ID('dbo.TourPhuongTien','U') IS NOT NULL DROP TABLE dbo.TourPhuongTien;
IF OBJECT_ID('dbo.TourDiemDung','U') IS NOT NULL DROP TABLE dbo.TourDiemDung;
IF OBJECT_ID('dbo.HuongDanVien','U') IS NOT NULL DROP TABLE dbo.HuongDanVien;
IF OBJECT_ID('dbo.DiemBanVe','U') IS NOT NULL DROP TABLE dbo.DiemBanVe;
IF OBJECT_ID('dbo.DiemThamQuan','U') IS NOT NULL DROP TABLE dbo.DiemThamQuan;
IF OBJECT_ID('dbo.PhuongTien','U') IS NOT NULL DROP TABLE dbo.PhuongTien;
IF OBJECT_ID('dbo.Tour','U') IS NOT NULL DROP TABLE dbo.Tour;
GO

CREATE TABLE dbo.Tour (
    MaTour varchar(20) NOT NULL PRIMARY KEY,
    TenTour nvarchar(180) NOT NULL,
    SoNgay int NOT NULL CHECK (SoNgay > 0),
    SoDem int NOT NULL CHECK (SoDem >= 0),
    DonGiaKhach decimal(18,2) NOT NULL CHECK (DonGiaKhach >= 0),
    MoTa nvarchar(1000) NULL,
    DangMoBan bit NOT NULL DEFAULT 1
);
CREATE TABLE dbo.PhuongTien (
    MaPT varchar(20) NOT NULL PRIMARY KEY,
    TenPT nvarchar(120) NOT NULL UNIQUE,
    GhiChu nvarchar(300) NULL
);
CREATE TABLE dbo.DiemThamQuan (
    MaDiemTQ varchar(20) NOT NULL PRIMARY KEY,
    TenDiemTQ nvarchar(180) NOT NULL,
    DiaDiem nvarchar(250) NOT NULL,
    NoiDung nvarchar(1000) NULL,
    YNghia nvarchar(1000) NULL
);
CREATE TABLE dbo.DiemBanVe (
    MaDiemBan varchar(20) NOT NULL PRIMARY KEY,
    TenDiemBan nvarchar(150) NOT NULL,
    DiaChi nvarchar(250) NOT NULL,
    DienThoai varchar(20) NULL
);
CREATE TABLE dbo.HuongDanVien (
    MaHDV varchar(20) NOT NULL PRIMARY KEY,
    HoTen nvarchar(120) NOT NULL,
    DienThoai varchar(20) NULL,
    LuongCoBan decimal(18,2) NOT NULL CHECK (LuongCoBan >= 0),
    DangLamViec bit NOT NULL DEFAULT 1
);
CREATE TABLE dbo.TourDiemDung (
    MaTour varchar(20) NOT NULL,
    ThuTu int NOT NULL CHECK (ThuTu > 0),
    TenDiemDung nvarchar(180) NOT NULL,
    DoiPhuongTien bit NOT NULL DEFAULT 0,
    CoNoiAn bit NOT NULL DEFAULT 0,
    CoKhachSan bit NOT NULL DEFAULT 0,
    HangSaoKhachSan int NULL CHECK (HangSaoKhachSan BETWEEN 2 AND 5),
    GhiChu nvarchar(500) NULL,
    PRIMARY KEY (MaTour, ThuTu),
    FOREIGN KEY (MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT CK_TDD_KhachSan CHECK ((CoKhachSan=0 AND HangSaoKhachSan IS NULL) OR (CoKhachSan=1 AND HangSaoKhachSan BETWEEN 2 AND 5))
);
CREATE TABLE dbo.TourPhuongTien (
    MaTour varchar(20) NOT NULL,
    ThuTuChang int NOT NULL CHECK (ThuTuChang > 0),
    MaPT varchar(20) NOT NULL,
    GhiChu nvarchar(300) NULL,
    PRIMARY KEY (MaTour, ThuTuChang, MaPT),
    FOREIGN KEY (MaTour) REFERENCES dbo.Tour(MaTour),
    FOREIGN KEY (MaPT) REFERENCES dbo.PhuongTien(MaPT)
);
CREATE TABLE dbo.TourDiemThamQuan (
    MaTour varchar(20) NOT NULL,
    MaDiemTQ varchar(20) NOT NULL,
    ThuTu int NOT NULL CHECK (ThuTu > 0),
    PRIMARY KEY (MaTour, MaDiemTQ),
    UNIQUE (MaTour, ThuTu),
    FOREIGN KEY (MaTour) REFERENCES dbo.Tour(MaTour),
    FOREIGN KEY (MaDiemTQ) REFERENCES dbo.DiemThamQuan(MaDiemTQ)
);
CREATE TABLE dbo.ChuyenLe (
    MaChuyen varchar(20) NOT NULL PRIMARY KEY,
    MaTour varchar(20) NOT NULL,
    NgayDi date NOT NULL,
    NgayVe date NOT NULL,
    DiaDiemDon nvarchar(250) NOT NULL,
    TrangThai nvarchar(40) NOT NULL DEFAULT N'Mở đăng ký',
    FOREIGN KEY (MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT CK_Chuyen_TrangThai CHECK (TrangThai IN (N'Mở đăng ký',N'Đóng đăng ký')),
    CONSTRAINT CK_Chuyen_Ngay CHECK (NgayVe >= NgayDi)
);
CREATE TABLE dbo.DoanKhach (
    MaDoan varchar(20) NOT NULL PRIMARY KEY,
    TenCoQuanDaiDien nvarchar(180) NOT NULL,
    DiaChi nvarchar(250) NOT NULL,
    DienThoai varchar(20) NOT NULL,
    NguoiDaiDien nvarchar(120) NOT NULL
);
CREATE TABLE dbo.DangKyDoan (
    SoDKDoan varchar(20) NOT NULL PRIMARY KEY,
    MaDoan varchar(20) NOT NULL,
    MaTour varchar(20) NOT NULL,
    NgayDangKy datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    NgayDi date NOT NULL,
    NgayKetThucDuKien date NOT NULL,
    SoNguoi int NOT NULL CHECK (SoNguoi > 12),
    DiaDiemDon nvarchar(250) NOT NULL,
    MuaBaoHiem bit NOT NULL DEFAULT 0,
    TienCoc decimal(18,2) NOT NULL CHECK (TienCoc > 0),
    DaThanhToanCoc bit NOT NULL DEFAULT 1,
    TongTienDuKien decimal(18,2) NOT NULL CHECK (TongTienDuKien >= 0),
    TrangThai nvarchar(40) NOT NULL DEFAULT N'Đã đăng ký',
    FOREIGN KEY (MaDoan) REFERENCES dbo.DoanKhach(MaDoan),
    FOREIGN KEY (MaTour) REFERENCES dbo.Tour(MaTour),
    CONSTRAINT CK_DKDoan_TrangThai CHECK (TrangThai IN (N'Đã đăng ký',N'Hủy - mất cọc',N'Đã hoàn tất thanh toán')),
    CONSTRAINT CK_DKDoan_Coc CHECK (TienCoc <= TongTienDuKien),
    CONSTRAINT CK_DKDoan_Ngay CHECK (NgayKetThucDuKien >= NgayDi)
);
CREATE TABLE dbo.ThanhVienDoan (
    SoDKDoan varchar(20) NOT NULL,
    STT int NOT NULL CHECK (STT > 0),
    HoTen nvarchar(120) NOT NULL,
    NgaySinh date NULL,
    SoGiayTo nvarchar(40) NULL,
    PRIMARY KEY (SoDKDoan, STT),
    FOREIGN KEY (SoDKDoan) REFERENCES dbo.DangKyDoan(SoDKDoan)
);
CREATE TABLE dbo.DangKyLe (
    SoDKLe varchar(20) NOT NULL PRIMARY KEY,
    MaChuyen varchar(20) NOT NULL,
    MaDiemBan varchar(20) NOT NULL,
    NgayDangKy datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    TenNguoiDangKy nvarchar(120) NOT NULL,
    DienThoai varchar(20) NOT NULL,
    SoNguoi int NOT NULL CHECK (SoNguoi BETWEEN 1 AND 11),
    ThanhTien decimal(18,2) NOT NULL CHECK (ThanhTien >= 0),
    DaThanhToan bit NOT NULL DEFAULT 1 CHECK (DaThanhToan=1),
    TrangThai nvarchar(40) NOT NULL DEFAULT N'Đã đăng ký',
    FOREIGN KEY (MaChuyen) REFERENCES dbo.ChuyenLe(MaChuyen),
    FOREIGN KEY (MaDiemBan) REFERENCES dbo.DiemBanVe(MaDiemBan)
);
CREATE TABLE dbo.PhanCongHDV (
    MaPC varchar(20) NOT NULL PRIMARY KEY,
    MaHDV varchar(20) NOT NULL,
    LoaiDoiTuong varchar(10) NOT NULL CHECK (LoaiDoiTuong IN ('LE','DOAN')),
    MaChuyen varchar(20) NULL,
    SoDKDoan varchar(20) NULL,
    NgayBatDau date NOT NULL,
    NgayKetThuc date NOT NULL,
    ThuLaoTour decimal(18,2) NOT NULL CHECK (ThuLaoTour >= 0),
    FOREIGN KEY (MaHDV) REFERENCES dbo.HuongDanVien(MaHDV),
    FOREIGN KEY (MaChuyen) REFERENCES dbo.ChuyenLe(MaChuyen),
    FOREIGN KEY (SoDKDoan) REFERENCES dbo.DangKyDoan(SoDKDoan),
    CONSTRAINT CK_PC_Target CHECK ((LoaiDoiTuong='LE' AND MaChuyen IS NOT NULL AND SoDKDoan IS NULL) OR (LoaiDoiTuong='DOAN' AND SoDKDoan IS NOT NULL AND MaChuyen IS NULL)),
    CONSTRAINT CK_PC_Ngay CHECK (NgayKetThuc >= NgayBatDau)
);
CREATE UNIQUE INDEX UX_PC_ChuyenLe ON dbo.PhanCongHDV(MaChuyen) WHERE MaChuyen IS NOT NULL;
CREATE INDEX IX_PC_HDV_Ngay ON dbo.PhanCongHDV(MaHDV, NgayBatDau, NgayKetThuc);
CREATE TABLE dbo.ThanhToanDoan (
    SoTT varchar(20) NOT NULL PRIMARY KEY,
    SoDKDoan varchar(20) NOT NULL,
    NgayThanhToan datetime2 NOT NULL,
    SoTien decimal(18,2) NOT NULL CHECK (SoTien > 0),
    GhiChu nvarchar(300) NULL,
    FOREIGN KEY (SoDKDoan) REFERENCES dbo.DangKyDoan(SoDKDoan)
);
CREATE TABLE dbo.KhaoSat (
    MaKhaoSat varchar(20) NOT NULL PRIMARY KEY,
    LoaiKhach varchar(10) NOT NULL CHECK (LoaiKhach IN ('LE','DOAN')),
    SoDKLe varchar(20) NULL,
    SoDKDoan varchar(20) NULL,
    NgayGui date NOT NULL,
    NgayPhanHoi date NULL,
    DiemDanhGia int NULL CHECK (DiemDanhGia BETWEEN 1 AND 5),
    GopY nvarchar(1500) NULL,
    FOREIGN KEY (SoDKLe) REFERENCES dbo.DangKyLe(SoDKLe),
    FOREIGN KEY (SoDKDoan) REFERENCES dbo.DangKyDoan(SoDKDoan),
    CONSTRAINT CK_KS_Target CHECK ((LoaiKhach='LE' AND SoDKLe IS NOT NULL AND SoDKDoan IS NULL) OR (LoaiKhach='DOAN' AND SoDKDoan IS NOT NULL AND SoDKLe IS NULL)),
    CONSTRAINT CK_KS_PhanHoi CHECK (NgayPhanHoi IS NULL OR NgayPhanHoi >= NgayGui)
);
CREATE UNIQUE INDEX UX_KS_Le ON dbo.KhaoSat(SoDKLe) WHERE SoDKLe IS NOT NULL;
CREATE UNIQUE INDEX UX_KS_Doan ON dbo.KhaoSat(SoDKDoan) WHERE SoDKDoan IS NOT NULL;
GO

INSERT dbo.Tour VALUES
('T001',N'Miền Tây 3 ngày 2 đêm',3,2,2500000,N'TP.HCM - Mỹ Tho - Cần Thơ - TP.HCM',1),
('T002',N'Đà Lạt 4 ngày 3 đêm',4,3,3200000,N'TP.HCM - Đà Lạt - TP.HCM',1),
('T003',N'Hà Nội - Hạ Long 5 ngày 4 đêm',5,4,8900000,N'TP.HCM - Hà Nội - Hạ Long - TP.HCM',1);
INSERT dbo.PhuongTien VALUES ('PT01',N'Xe du lịch',NULL),('PT02',N'Máy bay',NULL),('PT03',N'Tàu hỏa',NULL),('PT04',N'Tàu thủy',NULL);
INSERT dbo.DiemBanVe VALUES ('DB01',N'Điểm bán Quận 1',N'12 Lê Lợi, Quận 1, TP.HCM','0281000001'),('DB02',N'Điểm bán Thủ Đức',N'5 Võ Văn Ngân, TP. Thủ Đức','0281000002');
INSERT dbo.HuongDanVien VALUES ('HDV01',N'Nguyễn Minh Anh','0903000001',9000000,1),('HDV02',N'Trần Quốc Bình','0903000002',9500000,1),('HDV03',N'Lê Thu Cúc','0903000003',8500000,1);
INSERT dbo.DiemThamQuan VALUES ('DTQ01',N'Chợ nổi Cái Răng',N'Cần Thơ',N'Tham quan chợ trên sông',N'Nét văn hóa sông nước miền Tây'),('DTQ02',N'Chùa Vĩnh Tràng',N'Mỹ Tho',N'Tham quan kiến trúc chùa',N'Di tích kiến trúc nghệ thuật'),('DTQ03',N'Hồ Xuân Hương',N'Đà Lạt',N'Dạo quanh hồ trung tâm',N'Biểu tượng thành phố Đà Lạt');
INSERT dbo.TourDiemDung VALUES ('T001',1,N'Mỹ Tho',0,1,0,NULL,NULL),('T001',2,N'Cần Thơ',0,1,1,3,NULL),('T001',3,N'TP.HCM',0,0,0,NULL,N'Kết thúc tour'),('T002',1,N'Đà Lạt',0,1,1,4,NULL),('T002',2,N'TP.HCM',0,0,0,NULL,N'Kết thúc tour');
INSERT dbo.TourPhuongTien VALUES ('T001',1,'PT01',NULL),('T001',2,'PT01',NULL),('T001',3,'PT01',NULL),('T002',1,'PT01',N'TP.HCM - Đà Lạt'),('T002',2,'PT01',N'Đà Lạt - TP.HCM');
INSERT dbo.TourDiemThamQuan VALUES ('T001','DTQ02',1),('T001','DTQ01',2),('T002','DTQ03',1);
INSERT dbo.ChuyenLe VALUES ('CL001','T001','20260905','20260907',N'Nhà Văn hóa Thanh Niên, Quận 1',N'Đóng đăng ký'),('CL002','T001','20261115','20261117',N'Nhà Văn hóa Thanh Niên, Quận 1',N'Mở đăng ký'),('CL003','T002','20261120','20261123',N'Công viên 23/9, Quận 1',N'Mở đăng ký');
INSERT dbo.DangKyLe VALUES ('DKL001','CL001','DB01','20260820 09:00',N'Phạm Văn Long','0912000001',2,5000000,1,N'Đã đăng ký'),('DKL002','CL002','DB02','20261001 10:00',N'Võ Thị Mai','0912000002',3,7500000,1,N'Đã đăng ký');
INSERT dbo.DoanKhach VALUES ('DK01',N'Công ty CP Phần mềm Sao Việt',N'25 Nguyễn Thị Minh Khai, Quận 3','0283900001',N'Lê Văn Hải'),('DK02',N'Gia đình ông Trần Văn Nam',N'8 Phan Xích Long, Phú Nhuận','0909111222',N'Trần Văn Nam');
INSERT dbo.DangKyDoan VALUES ('DD001','DK01','T001','20260801 08:30','20260910','20260912',20,N'25 Nguyễn Thị Minh Khai, Quận 3',0,10000000,1,50000000,N'Đã đăng ký'),('DD002','DK02','T002','20260925 14:00','20261210','20261213',15,N'8 Phan Xích Long, Phú Nhuận',0,12000000,1,48000000,N'Đã đăng ký');
INSERT dbo.PhanCongHDV VALUES ('PC001','HDV01','LE','CL001',NULL,'20260905','20260907',1500000),('PC002','HDV02','DOAN',NULL,'DD001','20260910','20260912',2000000),('PC003','HDV03','DOAN',NULL,'DD001','20260910','20260912',2000000);
INSERT dbo.KhaoSat VALUES ('KS001','LE','DKL001',NULL,'20260908','20260910',5,N'Hướng dẫn viên nhiệt tình');
GO
PRINT N'Đã tạo CSDL QuanLyCongTyDuLich và dữ liệu mẫu.';
