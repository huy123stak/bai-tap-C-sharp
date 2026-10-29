# Hệ thống quản lý quán cafe & bán hàng (POS Cafe System)

## MỤC 1: THÔNG TIN SINH VIÊN

- **Họ và tên sinh viên:** chu chấn huy
- **Mã sinh viên:** 24810310470
- **Tên Đề tài / Bài tập:** Bài tập 5.3

## MỤC 2: HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN

**Yêu cầu:** Visual Studio 2022 (workload .NET desktop development, .NET 8), SQL Server và SSMS.

**Các bước cài đặt:**

1. Mở SQL Server Management Studio (SSMS), chạy file `Database/ScriptDB.sql` để khởi tạo CSDL `CafePOSDB` và dữ liệu mẫu.
2. Mở file `CafePOS.sln` bằng Visual Studio 2022.
3. Mở file `CafePOS/App.config`, sửa lại chuỗi kết nối `ConnectionString` (mục `Data Source`) cho phù hợp với máy cá nhân.
4. Build & Run dự án (F5).

**Cấu trúc thư mục:**

```
CafePOS/
├── Database/
│   └── ScriptDB.sql
├── CafePOS/
│   ├── DTO/
│   ├── DAL/
│   ├── BUS/
│   ├── GUI/
│   ├── App.config
│   └── Program.cs
├── images/
├── README.md
└── CafePOS.sln
```

**Chức năng chính:**

- Sơ đồ bàn dạng Button động: bàn Trống màu xanh, bàn Có khách màu đỏ.
- Gọi món: nạp chi tiết hóa đơn bằng SqlDataAdapter + DataTable, thêm món, tự chuyển bàn sang Có khách.
- Đổi bàn bằng SqlTransaction.
- Thanh toán bằng SqlTransaction: cập nhật GioRa, TongTien, TrangThai = 1 và trả bàn về Trống.
- Quản lý danh mục và thực đơn (Thêm, Sửa, Xóa) theo 3-Tier, validation không cho đơn giá âm hoặc tên món rỗng.

## MỤC 3: HÌNH ẢNH KẾT QUẢ CÁC MÀN HÌNH CHỨC NĂNG

1. Màn hình Chính / Tổng quan:

![Màn hình chính](images/01_main_screen.png)

2. Màn hình Quản lý Danh mục / Thêm Sửa Xóa (CRUD):

![Màn hình CRUD](images/02_crud_screen.png)

3. Màn hình Thực thi Nghiệp vụ chính:

![Màn hình Nghiệp vụ](images/03_transaction_screen.png)

4. Màn hình Báo cáo - Thống kê:

![Màn hình Thống kê](images/04_report_screen.png)

5. Minh họa chống SQL Injection hoặc Báo lỗi Giao dịch (Rollback):

![Minh họa Xử lý lỗi](images/05_error_handling.png)

