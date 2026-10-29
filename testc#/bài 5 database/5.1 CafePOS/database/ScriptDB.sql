IF DB_ID('CafePOSDB') IS NULL CREATE DATABASE CafePOSDB;
GO
USE CafePOSDB;
GO
IF OBJECT_ID('ChiTietHoaDon') IS NOT NULL DROP TABLE ChiTietHoaDon;
IF OBJECT_ID('HoaDon') IS NOT NULL DROP TABLE HoaDon;
IF OBJECT_ID('MonAn') IS NOT NULL DROP TABLE MonAn;
IF OBJECT_ID('DanhMuc') IS NOT NULL DROP TABLE DanhMuc;
IF OBJECT_ID('BanAn') IS NOT NULL DROP TABLE BanAn;
GO
CREATE TABLE BanAn (
    MaBan INT IDENTITY(1,1) PRIMARY KEY,
    TenBan NVARCHAR(50) NOT NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT N'Trống' CHECK (TrangThai IN (N'Trống', N'Có khách'))
);
CREATE TABLE DanhMuc (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL
);
CREATE TABLE MonAn (
    MaMon INT IDENTITY(1,1) PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL,
    MaDanhMuc INT NOT NULL FOREIGN KEY REFERENCES DanhMuc(MaDanhMuc),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0)
);
CREATE TABLE HoaDon (
    MaHoaDon INT IDENTITY(1,1) PRIMARY KEY,
    MaBan INT NOT NULL FOREIGN KEY REFERENCES BanAn(MaBan),
    GioVao DATETIME NOT NULL DEFAULT GETDATE(),
    GioRa DATETIME NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThai BIT NOT NULL DEFAULT 0
);
CREATE TABLE ChiTietHoaDon (
    MaHoaDon INT NOT NULL FOREIGN KEY REFERENCES HoaDon(MaHoaDon),
    MaMon INT NOT NULL FOREIGN KEY REFERENCES MonAn(MaMon),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaHoaDon, MaMon)
);
GO
INSERT INTO BanAn (TenBan) VALUES (N'Bàn 01'),(N'Bàn 02'),(N'Bàn 03'),(N'Bàn 04'),(N'Bàn 05'),(N'Bàn 06'),(N'Bàn 07'),(N'Bàn 08'),(N'Bàn 09'),(N'Bàn 10');
INSERT INTO DanhMuc (TenDanhMuc) VALUES (N'Cà phê'),(N'Trà sữa'),(N'Đồ ăn nhẹ');
INSERT INTO MonAn (TenMon, MaDanhMuc, DonGia) VALUES
(N'Cà phê đen',1,20000),(N'Cà phê sữa',1,25000),(N'Bạc xỉu',1,28000),(N'Cappuccino',1,45000),
(N'Trà sữa truyền thống',2,35000),(N'Trà sữa matcha',2,40000),(N'Trà sữa khoai môn',2,42000),
(N'Bánh tiramisu',3,38000),(N'Khoai tây chiên',3,30000),(N'Bánh mì que',3,15000);
GO
