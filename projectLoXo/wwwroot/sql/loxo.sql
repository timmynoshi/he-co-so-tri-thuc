USE MASTER
GO

IF EXISTS (SELECT * FROM SYSDATABASES WHERE name='loxo')
		DROP DATABASE loxo
GO

CREATE DATABASE loxo
GO

USE loxo
GO

CREATE TABLE NguoiDung  (  TenDangNhap NVARCHAR(50) PRIMARY KEY,
						   MatKhau NVARCHAR(50),
						   HoTen NVARCHAR(100),
						   DiaChi NVARCHAR(100),
						   SDT NVARCHAR(10),
						   QuyenHan NVARCHAR(50)
						   )
GO
SET DATEFORMAT dmy
INSERT INTO NguoiDung VALUES (N'admin',N'123', N'admin', N'Điện Biên Phủ', N'0292182451', N'admin')
INSERT INTO NguoiDung VALUES (N'ptd',N'123', N'Phan Thành Đạt', N'Điện Biên Phủ-Bình Thạnh', N'0982382124', N'user')
INSERT INTO NguoiDung VALUES (N'ntt',N'123', N'Nguyễn Thanh Tín', N'Điện Biên Phủ', N'0231252525', N'user')

CREATE TABLE LichSu ( Id INT IDENTITY(1,1) PRIMARY KEY,
					  TenDangNhap NVARCHAR(50),
					  Ngay DATE,
					  CauHoi NVARCHAR(MAX),
					  CauTraLoi NVARCHAR(MAX),
					  FOREIGN KEY (TenDangNhap) REFERENCES NguoiDung(TenDangNhap)
					  )
GO

SELECT TenDangNhap,MatKhau,HoTen,DiaChi,SDT,QuyenHan, COUNT(*) FROM NguoiDung WHERE TenDangNhap = 'admin' AND MatKhau = '123' GROUP BY TenDangNhap,MatKhau,HoTen,DiaChi,SDT,QuyenHan

-----------------------------------------------------
SELECT * FROM NguoiDung
SELECT * FROM LichSu

UPDATE NguoiDung SET MatKhau = '123', HoTen = 'admin123', DiaChi = N'Điện Biên Phủ', SDT = '0292182451' WHERE TenDangNhap = 'admin'


INSERT INTO LichSu VALUES (N'admin','25/07/2003',N'siu?',N'SIUUUU')

SELECT Id, TenDangNhap, Ngay, CauHoi, CauTraLoi FROM LichSu WHERE TenDangNhap = 'admin' ORDER BY Id ASC