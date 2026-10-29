# Hệ thống quản lý quy trình xuất - nhập kho nguyên vật liệu (Inventory Management)

## MỤC 1: THÔNG TIN SINH VIÊN

- **Họ và tên sinh viên:** chu chấn huy
- **Mã sinh viên:** 24810310470
- **Tên Đề tài / Bài tập:** Bài tập 5.5

## MỤC 2: HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN

**Yêu cầu:** Visual Studio 2022 (workload .NET desktop development, .NET 8), SQL Server và SSMS.

**Các bước cài đặt:**

1. Mở SQL Server Management Studio (SSMS), chạy file `Database/ScriptDB.sql` để khởi tạo CSDL `InventoryDB` và dữ liệu mẫu.
2. Mở file `InventoryManagement.sln` bằng Visual Studio 2022.
3. Mở file `InventoryManagement/App.config`, sửa lại chuỗi kết nối `ConnectionString` (mục `Data Source`) cho phù hợp với máy cá nhân.
4. Build & Run dự án (F5).

**Cấu trúc thư mục:**

```
InventoryManagement/
├── Database/
│   └── ScriptDB.sql
├── InventoryManagement/
│   ├── DTO/
│   ├── DAL/
│   ├── BUS/
│   ├── GUI/
│   ├── App.config
│   └── Program.cs
├── images/
├── README.md
└── InventoryManagement.sln
```

**Chức năng chính:**

- CRUD Vật tư và Nhà cung cấp qua DAL/BUS, tìm kiếm bằng SqlParameter (chống SQL Injection).
- Lập phiếu Nhập / Xuất bằng SqlTransaction: chèn PhieuKho, ChiTietPhieuKho, cập nhật SoLuongTon, Rollback khi không đủ hàng ("Không đủ hàng trong kho").
- Cảnh báo vật tư dưới ngưỡng an toàn.
- Tính tổng giá trị hàng tồn bằng ExecuteScalar.

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
