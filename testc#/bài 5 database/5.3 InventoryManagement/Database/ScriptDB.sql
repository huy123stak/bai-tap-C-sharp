IF DB_ID('InventoryDB') IS NULL CREATE DATABASE InventoryDB;
GO
USE InventoryDB;
GO
IF OBJECT_ID('ChiTietPhieuKho') IS NOT NULL DROP TABLE ChiTietPhieuKho;
IF OBJECT_ID('PhieuKho') IS NOT NULL DROP TABLE PhieuKho;
IF OBJECT_ID('VatTu') IS NOT NULL DROP TABLE VatTu;
IF OBJECT_ID('NhaCungCap') IS NOT NULL DROP TABLE NhaCungCap;
GO
CREATE TABLE NhaCungCap (
    MaNCC INT IDENTITY(1,1) PRIMARY KEY,
    TenNCC NVARCHAR(150) NOT NULL,
    DienThoai VARCHAR(15) NULL UNIQUE,
    DiaChi NVARCHAR(255) NULL
);
CREATE TABLE VatTu (
    MaVatTu VARCHAR(20) PRIMARY KEY,
    TenVatTu NVARCHAR(150) NOT NULL,
    DVT NVARCHAR(20) NULL,
    SoLuongTon DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0)
);
CREATE TABLE PhieuKho (
    MaPhieu INT IDENTITY(1,1) PRIMARY KEY,
    LoaiPhieu NVARCHAR(10) NOT NULL CHECK (LoaiPhieu IN (N'NHAP', N'XUAT')),
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    MaNCC INT NULL FOREIGN KEY REFERENCES NhaCungCap(MaNCC),
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0
);
CREATE TABLE ChiTietPhieuKho (
    MaPhieu INT NOT NULL FOREIGN KEY REFERENCES PhieuKho(MaPhieu),
    MaVatTu VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES VatTu(MaVatTu),
    SoLuong DECIMAL(18,2) NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    PRIMARY KEY (MaPhieu, MaVatTu)
);
GO
INSERT INTO NhaCungCap (TenNCC, DienThoai, DiaChi) VALUES
(N'Công ty TNHH Thực phẩm Việt', '0241111111', N'Hà Nội'),
(N'Công ty CP Nguyên liệu Sài Gòn', '0282222222', N'TP. Hồ Chí Minh'),
(N'Nhà phân phối Đà Nẵng', '0236333333', N'Đà Nẵng');
INSERT INTO VatTu (MaVatTu, TenVatTu, DVT, SoLuongTon) VALUES
('VT001', N'Đường cát', N'Kg', 0),
('VT002', N'Bột mì', N'Kg', 0),
('VT003', N'Sữa tươi', N'Thùng', 0),
('VT004', N'Hộp giấy', N'Cái', 0),
('VT005', N'Cà phê hạt', N'Kg', 0);
GO
DECLARE @p INT;
INSERT INTO PhieuKho (LoaiPhieu, MaNCC, TongTien) VALUES (N'NHAP', 1, 0);
SET @p = SCOPE_IDENTITY();
INSERT INTO ChiTietPhieuKho (MaPhieu, MaVatTu, SoLuong, DonGia) VALUES
(@p, 'VT001', 100, 20000),
(@p, 'VT002', 80, 18000),
(@p, 'VT005', 50, 150000);
UPDATE VatTu SET SoLuongTon = 100 WHERE MaVatTu = 'VT001';
UPDATE VatTu SET SoLuongTon = 80 WHERE MaVatTu = 'VT002';
UPDATE VatTu SET SoLuongTon = 50 WHERE MaVatTu = 'VT005';
UPDATE PhieuKho SET TongTien = 100*20000 + 80*18000 + 50*150000 WHERE MaPhieu = @p;
INSERT INTO PhieuKho (LoaiPhieu, MaNCC, TongTien) VALUES (N'NHAP', 2, 0);
SET @p = SCOPE_IDENTITY();
INSERT INTO ChiTietPhieuKho (MaPhieu, MaVatTu, SoLuong, DonGia) VALUES
(@p, 'VT003', 8, 320000),
(@p, 'VT004', 15, 5000);
UPDATE VatTu SET SoLuongTon = 8 WHERE MaVatTu = 'VT003';
UPDATE VatTu SET SoLuongTon = 15 WHERE MaVatTu = 'VT004';
UPDATE PhieuKho SET TongTien = 8*320000 + 15*5000 WHERE MaPhieu = @p;
GO
