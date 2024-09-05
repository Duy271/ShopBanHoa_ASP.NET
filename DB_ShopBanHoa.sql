
CREATE DATABASE ShopBanHoa;


USE ShopBanHoa;



CREATE TABLE DangKy (
    MaKhachHang INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(255) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL,
    VaiTroNguoiDung NVARCHAR(20),
	TrangThai NVARCHAR(50),
	TenKhachHang NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    
	SoDienThoai NVARCHAR(20),
    Email NVARCHAR(255),
	AnhDaiDien NVARCHAR(255)
);





CREATE TABLE Hoa (
    MaHoa INT IDENTITY(1,1) PRIMARY KEY,
    TenHoa NVARCHAR(255) NOT NULL,
    MoTaHoa NVARCHAR(MAX),
    GiaBan INT NOT NULL,
    SoLuongTonKho INT NOT NULL,
    LoaiHoa NVARCHAR(50),
    MauHoa NVARCHAR(50),
    KichThuoc NVARCHAR(50),
	KhuyenMai INT,
    AnhHoa NVARCHAR(255),
  
);


CREATE TABLE DonDatHang (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaKhachHang INT,
    NgayDatHang DATETIME2,
    TenKhachHang NVARCHAR(255),
    DiaChiGiaoHang NVARCHAR(255),
    SoDienThoai NVARCHAR(20),
    TrangThaiDonHang NVARCHAR(20),
    TongGiaTriDonHang INT,
    FOREIGN KEY (MaKhachHang) REFERENCES DangKy(MaKhachHang)
);




CREATE TABLE ChiTietDonDatHang (
    MaChiTietDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT,
    MaHoa INT,
    SoLuong INT,
    GiaBanLe INT,
    TongTien INT,
    FOREIGN KEY (MaDonHang) REFERENCES DonDatHang(MaDonHang),
    FOREIGN KEY (MaHoa) REFERENCES Hoa(MaHoa)
);


CREATE TABLE BinhLuan (
    MaBinhLuan INT IDENTITY(1,1) PRIMARY KEY,
    MaHoa INT,
    MaKhachHang INT,
    DiemDanhGia INT,
    NoiDungBinhLuan NVARCHAR(MAX),
    ThoiGianBinhLuan DATETIME,
    FOREIGN KEY (MaHoa) REFERENCES Hoa(MaHoa),
    FOREIGN KEY (MaKhachHang) REFERENCES DangKy(MaKhachHang)
);

CREATE TABLE GioHang (
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaKhachHang INT,
    MaHoa INT,
    SoLuong INT,
    FOREIGN KEY (MaKhachHang) REFERENCES DangKy(MaKhachHang),
    FOREIGN KEY (MaHoa) REFERENCES Hoa(MaHoa)
);


CREATE TABLE DoanhThu (
    MaDoanhThu INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT,
    NgayGiaoHang DATE,
    TongDoanhThu INT,
    FOREIGN KEY (MaDonHang) REFERENCES DonDatHang(MaDonHang)
);

CREATE TRIGGER Trigger_CapNhatDoanhThu
ON ChiTietDonDatHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật thông tin doanh thu cho đơn hàng mới hoặc đã thay đổi
    UPDATE DoanhThu
    SET TongDoanhThu = (
        SELECT SUM(SoLuong * GiaBanLe)
        FROM inserted
        JOIN Hoa ON inserted.MaHoa = Hoa.MaHoa
        WHERE DoanhThu.MaDonHang = inserted.MaDonHang
    )
    FROM DoanhThu
    WHERE DoanhThu.MaDonHang IN (SELECT MaDonHang FROM inserted);

    -- Nếu có đơn hàng mới, thêm thông tin doanh thu mới
    INSERT INTO DoanhThu (MaDonHang, NgayGiaoHang, TongDoanhThu)
    SELECT
        DonDatHang.MaDonHang,
        DonDatHang.NgayDatHang,
        SUM(ChiTietDonDatHang.SoLuong * ChiTietDonDatHang.GiaBanLe) AS TongDoanhThu
    FROM
        DonDatHang
        JOIN ChiTietDonDatHang ON DonDatHang.MaDonHang = ChiTietDonDatHang.MaDonHang
        JOIN Hoa ON ChiTietDonDatHang.MaHoa = Hoa.MaHoa
    WHERE
        DonDatHang.MaDonHang IN (SELECT MaDonHang FROM inserted)
    GROUP BY
        DonDatHang.MaDonHang, DonDatHang.NgayDatHang;

    -- Nếu có đơn hàng bị xóa, xóa thông tin doanh thu tương ứng
    DELETE FROM DoanhThu
    WHERE MaDonHang IN (SELECT MaDonHang FROM deleted);
END;



-- Thêm dữ liệu vào bảng Hoa
INSERT INTO Hoa (TenHoa, MoTaHoa, GiaBan, SoLuongTonKho, LoaiHoa, MauHoa, KichThuoc, KhuyenMai, AnhHoa)
VALUES
(N'Hoa Đỏ1', N'Hoa đẹp màu đỏ tươi', 250000, 100, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh1.jpg'),
(N'Hoa Hồng Trắng2', N'Hoa hồng trắng tinh khôi', 300000, 2, N'Hoa Hồng', N'Đỏ', N'To', 25000, 'hh2.jpg'),
(N'Hoa Lan Vàng3', N'Hoa lan màu vàng nổi bật', 350000, 120, N'Hoa Hồng', N'Đỏ', N'To', 35000, 'hh3.jpg'),
(N'Hoa Cẩm Chướng4', N'Hoa cẩm chướng hương thơm dễ chịu', 400000, 90, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh4.jpg'),
(N'Hoa Tulip Hồng5', N'Hoa tulip màu hồng nhẹ nhàng', 280000, 110, N'Hoa Hồng', N'Đỏ', N'To', 20000, 'hh5.jpg'),
(N'Hoa Đỏ6', N'Hoa đẹp màu đỏ tươi', 250000, 100, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh1.jpg'),
(N'Hoa Hồng Trắng7', N'Hoa hồng trắng tinh khôi', 300000, 2, N'Hoa Hồng', N'Đỏ', N'To', 25000, 'hh2.jpg'),
(N'Hoa Lan Vàng8', N'Hoa lan màu vàng nổi bật', 350000, 120, N'Hoa Hồng', N'Đỏ', N'To', 35000, 'hh3.jpg'),
(N'Hoa Cẩm Chướng9', N'Hoa cẩm chướng hương thơm dễ chịu', 400000, 90, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh4.jpg'),
(N'Hoa Tulip Hồng10', N'Hoa tulip màu hồng nhẹ nhàng', 280000, 110, N'Hoa Hồng', N'Đỏ', N'To', 20000, 'hh5.jpg'),
(N'Hoa Đỏ11', N'Hoa đẹp màu đỏ tươi', 250000, 100, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh1.jpg'),
(N'Hoa Hồng Trắng12', N'Hoa hồng trắng tinh khôi', 300000, 2, N'Hoa Hồng', N'Đỏ', N'To', 25000, 'hh2.jpg'),
(N'Hoa Lan Vàng13', N'Hoa lan màu vàng nổi bật', 350000, 120, N'Hoa Hồng', N'Đỏ', N'To', 35000, 'hh3.jpg'),
(N'Hoa Cẩm Chướng14', N'Hoa cẩm chướng hương thơm dễ chịu', 400000, 90, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh4.jpg'),
(N'Hoa Tulip Hồng15', N'Hoa tulip màu hồng nhẹ nhàng', 280000, 110, N'Hoa Hồng', N'Đỏ', N'To', 20000, 'hh5.jpg'),
(N'Hoa Đỏ16', N'Hoa đẹp màu đỏ tươi', 250000, 100, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh1.jpg'),
(N'Hoa Hồng Trắng17', N'Hoa hồng trắng tinh khôi', 300000, 2, N'Hoa Hồng', N'Đỏ', N'To', 25000, 'hh2.jpg'),
(N'Hoa Lan Vàng18', N'Hoa lan màu vàng nổi bật', 350000, 120, N'Hoa Hồng', N'Đỏ', N'To', 35000, 'hh3.jpg'),
(N'Hoa Cẩm Chướng19', N'Hoa cẩm chướng hương thơm dễ chịu', 400000, 90, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh4.jpg'),
(N'Hoa Tulip Hồng20', N'Hoa tulip màu hồng nhẹ nhàng', 280000, 110, N'Hoa Hồng', N'Đỏ', N'To', 20000, 'hh5.jpg'),
(N'Hoa Đỏ21', N'Hoa đẹp màu đỏ tươi', 250000, 100, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh1.jpg'),
(N'Hoa Hồng Trắng22', N'Hoa hồng trắng tinh khôi', 300000, 2, N'Hoa Hồng', N'Đỏ', N'To', 25000, 'hh2.jpg'),
(N'Hoa Lan Vàng23', N'Hoa lan màu vàng nổi bật', 350000, 120, N'Hoa Hồng', N'Đỏ', N'To', 35000, 'hh3.jpg'),
(N'Hoa Cẩm Chướng24', N'Hoa cẩm chướng hương thơm dễ chịu', 400000, 90, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh4.jpg'),
(N'Hoa Tulip Hồng25', N'Hoa tulip màu hồng nhẹ nhàng', 280000, 110, N'Hoa Hồng', N'Đỏ', N'To', 20000, 'hh5.jpg'),
(N'Hoa Đỏ26', N'Hoa đẹp màu đỏ tươi', 250000, 100, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh1.jpg'),
(N'Hoa Hồng Trắng27', N'Hoa hồng trắng tinh khôi', 300000, 2, N'Hoa Hồng', N'Đỏ', N'To', 25000, 'hh2.jpg'),
(N'Hoa Lan Vàng28', N'Hoa lan màu vàng nổi bật', 350000, 120, N'Hoa Hồng', N'Đỏ', N'To', 35000, 'hh3.jpg'),
(N'Hoa Cẩm Chướng29', N'Hoa cẩm chướng hương thơm dễ chịu', 400000, 90, N'Hoa Hồng', N'Đỏ', N'To', 15000, 'hh4.jpg'),
(N'Hoa Đồng Tiền30', N'Hoa đồng tiền màu vàng óng ả', 220000, 150, N'Hoa Hồng', N'Đỏ', N'To', 30000, 'hh6.jpg');


INSERT INTO DangKy (TenDangNhap, MatKhau, VaiTroNguoiDung, TrangThai, TenKhachHang, DiaChi, SoDienThoai, Email, AnhDaiDien)
VALUES 
('admin123', '5063ddebd25043a5eb5d31b839e344f8', 'Admin', 'Duoc Dung', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('khanhduy', '5063ddebd25043a5eb5d31b839e344f8', 'User', 'Duoc Dung', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('khanhduy1', '5063ddebd25043a5eb5d31b839e344f8', 'User', 'Duoc Dung', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user4', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user5', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user6', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user7', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user8', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user9', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user10', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user11', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user12', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user13', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user14', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user15', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user16', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user17', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user18', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user19', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 1', 'Địa chỉ 1', '1234567890', 'user1@example.com', 'hh1.jpg'),
('user20', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 2', 'Địa chỉ 2', '1234567891', 'user2@example.com', 'hh1.jpg'),
('user21', 'e10adc3949ba59abbe56e057f20f883e', 'User', 'Chan', 'User 30', 'Địa chỉ 30', '12345678929', 'user30@example.com', 'hh1.jpg');



select*from Hoa
select*from DangKy
select*from GioHang
select*from DonDatHang
select*from ChiTietDonDatHang

