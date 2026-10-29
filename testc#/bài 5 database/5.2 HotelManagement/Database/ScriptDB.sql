IF DB_ID('HotelDB') IS NULL CREATE DATABASE HotelDB;
GO
USE HotelDB;
GO
IF OBJECT_ID('SuDungDichVu') IS NOT NULL DROP TABLE SuDungDichVu;
IF OBJECT_ID('PhieuDatPhong') IS NOT NULL DROP TABLE PhieuDatPhong;
IF OBJECT_ID('DichVu') IS NOT NULL DROP TABLE DichVu;
IF OBJECT_ID('KhachHang') IS NOT NULL DROP TABLE KhachHang;
IF OBJECT_ID('Phong') IS NOT NULL DROP TABLE Phong;
GO
CREATE TABLE Phong (
    SoPhong VARCHAR(10) PRIMARY KEY,
    LoaiPhong NVARCHAR(50) NOT NULL,
    GiaTheoGio DECIMAL(18,2) NOT NULL CHECK (GiaTheoGio >= 0),
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Trống' CHECK (TrangThai IN (N'Trống', N'Đang ở', N'Đang dọn dẹp'))
);
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    CCCD VARCHAR(20) NOT NULL UNIQUE,
    SoDienThoai VARCHAR(15) NULL
);
CREATE TABLE DichVu (
    MaDV INT IDENTITY(1,1) PRIMARY KEY,
    TenDV NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0)
);
CREATE TABLE PhieuDatPhong (
    MaPhieu INT IDENTITY(1,1) PRIMARY KEY,
    SoPhong VARCHAR(10) NOT NULL FOREIGN KEY REFERENCES Phong(SoPhong),
    MaKH INT NOT NULL FOREIGN KEY REFERENCES KhachHang(MaKH),
    NgayNhanPhong DATETIME NOT NULL DEFAULT GETDATE(),
    NgayTraPhong DATETIME NULL,
    TongTienThanhToan DECIMAL(18,2) NULL
);
CREATE TABLE SuDungDichVu (
    MaSuDung INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieu INT NOT NULL FOREIGN KEY REFERENCES PhieuDatPhong(MaPhieu),
    MaDV INT NOT NULL FOREIGN KEY REFERENCES DichVu(MaDV),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    ThoiGian DATETIME NOT NULL DEFAULT GETDATE()
);
GO
INSERT INTO Phong (SoPhong, LoaiPhong, GiaTheoGio, TrangThai) VALUES
('P101', N'Đơn', 80000, N'Trống'),
('P102', N'Đơn', 80000, N'Trống'),
('P103', N'Đơn', 80000, N'Trống'),
('P201', N'Đôi', 120000, N'Trống'),
('P202', N'Đôi', 120000, N'Trống'),
('P203', N'Đôi', 120000, N'Đang dọn dẹp'),
('P301', N'VIP', 250000, N'Trống'),
('P302', N'VIP', 250000, N'Trống');
INSERT INTO KhachHang (HoTen, CCCD, SoDienThoai) VALUES
(N'Nguyễn Văn An', '001099000001', '0901000001'),
(N'Trần Thị Bình', '001099000002', '0901000002');
INSERT INTO DichVu (TenDV, DonGia) VALUES
(N'Nước suối', 10000),
(N'Nước ngọt', 15000),
(N'Giặt là', 50000),
(N'Mì ly', 20000),
(N'Bữa sáng', 60000);
GO
