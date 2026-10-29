# 🛒 LAB04 — Hệ thống mua sắm trực tuyến e-SHOPPING

> Bài thực hành phân tích và thiết kế hướng đối tượng cho cửa hàng ABC, từ chọn sản phẩm, quản lý giỏ hàng đến đặt hàng và xác nhận thanh toán.

## 🌟 Kết quả nổi bật

- Hoàn thành [báo cáo LAB04](Word/PhamMinhGiang-12DHCNPM1-Lab04.docx) theo hai pha **phân tích → thiết kế**.
- Thiết kế [9 trang sơ đồ draw.io](ThietKe/PhamMInhGiang-12DHCNPM1-ThietKeLab04.drawio), có thể mở và chỉnh sửa tiếp trong diagrams.net.
- Xây dựng prototype **C# WinForms .NET Framework 4.7.2** cho luồng xem sản phẩm, thêm giỏ và đặt hàng.
- Có [script SQL Server](Project/E-shopping/Database/01_Tao_CSDL_eShopping.sql) tạo **7 bảng** và dữ liệu mẫu cho khách hàng, sản phẩm.
- Lập [kế hoạch kiểm thử và ma trận truy vết](Project/E-shopping/KiemThu/Ke_hoach_kiem_thu_va_truy_vet.md) với **8 kịch bản** từ yêu cầu đến UML, code và CSDL.

## 🧭 Nghiệp vụ e-SHOPPING

| # | Nghiệp vụ | Nội dung |
|---:|---|---|
| 1 | 🛍️ Xem sản phẩm | Lấy danh sách và thông tin sản phẩm từ nguồn quản lý sản phẩm |
| 2 | 🧺 Quản lý giỏ hàng | Chọn sản phẩm, số lượng, cập nhật giỏ và tính tiền hàng |
| 3 | 👤 Tài khoản & người nhận | Đăng nhập/đăng ký theo đề; người nhận có thể khác người mua |
| 4 | 🚚 Giao hàng | Ba hình thức giao hàng; miễn phí chuyển phát nhanh từ 1 triệu đồng và giao nhanh trong ngày từ 5 triệu đồng theo đề |
| 5 | 💳 Thanh toán | Gửi yêu cầu đến dịch vụ thanh toán; chỉ ghi đơn khi thanh toán thành công |
| 6 | ✉️ Xác nhận đơn | Gửi thư xác nhận nếu khách cung cấp địa chỉ email |

### Phạm vi prototype hiện có

`Form1` hiển thị sản phẩm, cho thêm và cộng dồn số lượng trong giỏ, nhập thông tin người nhận và số thẻ, sau đó gọi `DichVuMuaHang.DatHang()`. Thanh toán và thư điện tử được **mô phỏng bằng Adapter**; sản phẩm hiện đọc từ SQL Server qua `KhoSanPham`. Đơn hàng, chi tiết và giao dịch thanh toán được lưu trong cùng một transaction sau khi thanh toán mô phỏng thành công.

Các phần **đăng nhập/đăng ký, chi tiết và nhóm sản phẩm, ba lựa chọn giao hàng cùng cách tính phí/miễn phí, kiểm tra thẻ đầy đủ và tích hợp dịch vụ ngoài thật** thuộc phạm vi phân tích thiết kế của đề nhưng chưa có trong prototype. Giao diện hiện gán cố định hình thức **giao hàng thường**.

## 🧩 Bộ sơ đồ UML

File draw.io gồm đúng chín trang:

1. Use Case tổng quát.
2. Lớp phân tích.
3. Trạng thái quá trình đặt hàng.
4. Sequence chọn sản phẩm và quản lý giỏ hàng.
5. Sequence đặt hàng, thanh toán và gửi thư.
6. Lớp thiết kế nghiệp vụ.
7. Lớp thiết kế kỹ thuật.
8. Thiết kế theo chức năng.
9. Activity đặt hàng.

Các sơ đồ dùng cùng phong cách trình bày của tài liệu mẫu: nền sáng, khối vàng nhạt, đường nối đỏ và chữ serif.

## 🏗️ Kiến trúc prototype

```text
┌──────────────────────────────┐
│ Form1 · WinForms UI          │  Sản phẩm · Giỏ hàng · Thông tin nhận hàng
└──────────────┬───────────────┘
               ▼
┌──────────────────────────────┐
│ Services/DichVuMuaHang       │  Kiểm tra đầu vào, điều phối đặt hàng
└───────┬───────────────┬──────┘
        ▼               ▼
┌───────────────┐  ┌─────────────────────────────┐
│ Adapters      │  │ Data/KhoSanPham, KhoDonHang │
│ Thanh toán &  │  │ ADO.NET · SQL có tham số     │
│ email mô phỏng│  └──────────────┬──────────────┘
└───────────────┘                 ▼
                         ┌──────────────┐
                         │ SQL Server   │
                         │ EShoppingDb  │
                         └──────────────┘
```

## 🛠️ Công nghệ sử dụng

![C#](https://img.shields.io/badge/C%23-WinForms-512BD4?logo=csharp&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.7.2-5C2D91)
![SQL Server](https://img.shields.io/badge/SQL_Server-ADO.NET-CC2927?logo=microsoftsqlserver&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-2022-5C2D91?logo=visualstudio&logoColor=white)
![draw.io](https://img.shields.io/badge/draw.io-UML-F08705?logo=diagramsdotnet&logoColor=white)

## 🚀 Mở và chạy prototype

1. Chạy `Project/E-shopping/Database/01_Tao_CSDL_eShopping.sql` trong SQL Server Management Studio để tạo `EShoppingDb` và dữ liệu mẫu.
2. Chỉnh chuỗi kết nối `EShoppingDb` trong `Project/E-shopping/App.config` cho đúng SQL Server trên máy.
3. Mở [solution Visual Studio](Project/E-shopping/WindowsFormsApp1.sln), build và chạy ứng dụng.
4. Kiểm tra theo [kịch bản TC01–TC08](Project/E-shopping/KiemThu/Ke_hoach_kiem_thu_va_truy_vet.md).

> **Lưu ý dữ liệu:** script SQL **xóa và tạo lại** bảy bảng của `EShoppingDb` trước khi thêm dữ liệu mẫu. Chỉ chạy trên CSDL thực hành; sao lưu nếu cần giữ dữ liệu cũ.

Tệp [`KiemThuTuDong.cs`](Project/E-shopping/KiemThu/KiemThuTuDong.cs) chứa ba kiểm tra console minh họa. Tệp này chưa được nối thành một project test riêng trong solution; tài liệu TC01–TC08 là kế hoạch và kết quả mong đợi, không phải báo cáo tám ca đã chạy đạt.

## 📁 Cấu trúc chính

```text
LAB04/
├── README.md
├── Word/
│   ├── Bai_9_GOC.docx
│   └── PhamMinhGiang-12DHCNPM1-Lab04.docx
├── ThietKe/
│   └── PhamMInhGiang-12DHCNPM1-ThietKeLab04.drawio
└── Project/E-shopping/
    ├── WindowsFormsApp1.sln
    ├── Form1.cs
    ├── Models/
    ├── Services/DichVuMuaHang.cs
    ├── Adapters/
    ├── Data/
    ├── Database/01_Tao_CSDL_eShopping.sql
    └── KiemThu/
```

## 👨‍🎓 Thông tin sinh viên

- **Họ và tên:** Phạm Minh Giang
- **MSSV:** 1250080044
- **Lớp:** 12DHCNPM1
- **Môn học:** Phương pháp phát triển phần mềm hướng đối tượng

---

<p align="center">✨ Yêu cầu → UML → WinForms → SQL Server → Kiểm thử truy vết ✨</p>
