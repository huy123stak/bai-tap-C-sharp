Câu 1. 
  - Value Type (int, double, bool, struct, enum) lưu trực tiếp giá trị, thường nằm trên Stack. Gán biến này cho biến kia thì sao chép giá trị, hai biến độc lập nhau.
  - Reference Type (class, string, array, interface) có biến nằm trên Stack chỉ chứa địa chỉ, còn đối tượng thật nằm trên Heap và do Garbage Collector dọn. Gán thì chỉ sao chép địa chỉ, hai biến cùng trỏ một đối tượng, nên sửa qua biến này thì biến kia cũng đổi. Reference Type có thể là null, Value Type thì không (trừ khi dùng int?).

Câu 2. 
  - Thuộc tính init chỉ cho gán giá trị lúc khởi tạo đối tượng (trong constructor hoặc object initializer), sau đó chỉ đọc.
  - Thuộc tính set thường thì gán lại được bất cứ lúc nào. Dùng thực tế cho các đối tượng không nên bị sửa sau khi tạo như DTO, model trả về từ API, đối tượng cấu hình.

ví dụ:
public string Ten { get; init; }
var sp = new SanPham { Ten = "Laptop" };   // được
sp.Ten = "Điện thoại";                      // lỗi biên dịch

Câu 3.
  - virtual đặt ở lớp cha, cho phép lớp con viết lại phương thức và có sẵn một cài đặt mặc định.
  - override đặt ở lớp con, viết lại phương thức đó với cùng chữ ký để thay đổi hành vi. Khi gọi qua biến kiểu lớp cha đang trỏ tới đối tượng lớp con, chương trình sẽ chạy bản override của lớp con lúc chạy, đó chính là đa hình. Nếu lớp con không override thì dùng bản của lớp cha.

Câu 4.  
  - Thành phần static thuộc về chính lớp, chỉ có một bản dùng chung và tồn tại mà không cần tạo đối tượng.
  - thành phần thường thì mỗi đối tượng có một bản riêng. C# bắt buộc gọi static qua tên lớp (TenLop.ThanhPhan), truy cập qua đối tượng tạo bằng new sẽ báo lỗi CS0176, để tránh nhầm với thành viên của từng đối tượng.
