IF DB_ID('HRMDB') IS NULL CREATE DATABASE HRMDB;
GO
USE HRMDB;
GO
IF OBJECT_ID('BangLuong') IS NOT NULL DROP TABLE BangLuong;
IF OBJECT_ID('BangChamCong') IS NOT NULL DROP TABLE BangChamCong;
IF OBJECT_ID('NhanVien') IS NOT NULL DROP TABLE NhanVien;
IF OBJECT_ID('PhongBan') IS NOT NULL DROP TABLE PhongBan;
GO
CREATE TABLE PhongBan(
    MaPB INT IDENTITY(1,1) PRIMARY KEY,
    TenPB NVARCHAR(100) NOT NULL,
    Mota NVARCHAR(255)
);
CREATE TABLE NhanVien(
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATETIME,
    CCCD VARCHAR(20) UNIQUE,
    MaPB INT NOT NULL FOREIGN KEY REFERENCES PhongBan(MaPB),
    LuongCoBan DECIMAL(18,2) NOT NULL CHECK (LuongCoBan > 0),
    HeSoLuong FLOAT NOT NULL DEFAULT 1.0 CHECK (HeSoLuong > 0)
);
CREATE TABLE BangChamCong(
    MaCong INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    ThangNam VARCHAR(7) NOT NULL,
    SoNgayCong FLOAT NOT NULL DEFAULT 0 CHECK (SoNgayCong >= 0 AND SoNgayCong <= 31),
    SoGioOT FLOAT NOT NULL DEFAULT 0 CHECK (SoGioOT >= 0),
    DaKhoa BIT NOT NULL DEFAULT 0,
    CONSTRAINT UQ_ChamCong UNIQUE (MaNV, ThangNam)
);
CREATE TABLE BangLuong(
    MaLuong INT IDENTITY(1,1) NOT NULL UNIQUE,
    MaNV INT NOT NULL FOREIGN KEY REFERENCES NhanVien(MaNV),
    ThangNam VARCHAR(7) NOT NULL,
    ThucLinh DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_BangLuong PRIMARY KEY (MaNV, ThangNam)
);
GO
INSERT INTO PhongBan(TenPB, Mota) VALUES
(N'Kế toán', N'Quản lý tài chính, kế toán'),
(N'Nhân sự', N'Tuyển dụng và quản lý nhân sự'),
(N'Kinh doanh', N'Bán hàng và chăm sóc khách hàng'),
(N'Kỹ thuật', N'Phát triển và vận hành hệ thống');
INSERT INTO NhanVien(HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) VALUES
(N'Nguyễn Văn An', '1995-03-12', '001095000001', 1, 12000000, 1.2),
(N'Trần Thị Bình', '1993-07-25', '001093000002', 1, 11000000, 1.0),
(N'Lê Minh Châu', '1998-01-05', '001098000003', 2, 10000000, 1.0),
(N'Phạm Quốc Dũng', '1990-11-30', '001090000004', 3, 13000000, 1.5),
(N'Hoàng Thị Em', '1996-09-18', '001096000005', 3, 9500000, 1.0),
(N'Vũ Đức Phúc', '1992-05-02', '001092000006', 4, 15000000, 1.8),
(N'Đặng Thu Giang', '1999-12-21', '001099000007', 4, 14000000, 1.3),
(N'Bùi Thanh Hà', '1994-04-09', '001094000008', 2, 10500000, 1.1);
GO
INSERT INTO BangChamCong(MaNV, ThangNam, SoNgayCong, SoGioOT, DaKhoa)
SELECT MaNV, '09/2026', 22, MaNV * 2, 1 FROM NhanVien;
INSERT INTO BangLuong(MaNV, ThangNam, ThucLinh)
SELECT nv.MaNV, cc.ThangNam,
       CAST((nv.LuongCoBan * nv.HeSoLuong / 22.0) * cc.SoNgayCong + cc.SoGioOT * 50000.0 AS DECIMAL(18,2))
FROM BangChamCong cc JOIN NhanVien nv ON nv.MaNV = cc.MaNV
WHERE cc.ThangNam = '09/2026';
INSERT INTO BangChamCong(MaNV, ThangNam, SoNgayCong, SoGioOT, DaKhoa)
SELECT MaNV, '10/2026', 20 + (MaNV % 3), MaNV % 5, 0 FROM NhanVien;
GO
