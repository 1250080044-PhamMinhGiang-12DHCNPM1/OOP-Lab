# 🌏 LAB05 — Quản lý Công ty Du lịch Văn Hóa Việt

> Bài thực hành Phân tích thiết kế hướng đối tượng, hiện thực bằng **C# WinForms .NET Framework 4.7.2** và **SQL Server**.

## ✅ Công việc đã hoàn thành hôm nay

| Hạng mục | Kết quả |
| --- | --- |
| 🧭 Phân tích | Hoàn thiện luồng tour, chuyến lẻ, khách lẻ/đoàn, HDV, thanh toán, khảo sát và lương. |
| 📐 UML | Có 7 sơ đồ: lớp phân tích, use case, hoạt động, tuần tự, lớp thiết kế và trạng thái. Nguồn `.drawio` có thể chỉnh sửa nằm trong `thietke/`. |
| 🗄️ CSDL | Script `Database/QuanLyCongTyDuLich.sql` tạo database, bảng, khóa, ràng buộc, chỉ mục và dữ liệu mẫu. |
| 🖥️ Ứng dụng | Đã đổi tên thành `QuanLyCongTyDuLich`; kiến trúc `Forms → Services → Data/Db.cs → SQL Server`. |
| 🧪 Kiểm thử | Bộ test Service chạy **17/17 đạt**, phủ tour, đăng ký lẻ/đoàn, cọc, bảo hiểm, HDV, thanh toán, khảo sát, lương và thống kê. |
| 📝 Báo cáo | Có báo cáo kiểm thử & truy vết tại `word/NhiemVu4_KiemThuTruyVet.docx`. |

## ✨ Điểm nổi bật so với Word mẫu giảng viên

- **Từ mô tả đến sản phẩm chạy được:** tài liệu mẫu là chuẩn trình bày và yêu cầu; bài làm này có thêm ứng dụng WinForms, CSDL SQL Server và dữ liệu mẫu thực thi được.
- **UML do nhóm thiết kế:** 7 sơ đồ được tạo mới và lưu cả ảnh lẫn file draw.io, không chỉ dừng ở ảnh minh họa tĩnh.
- **Quy tắc được hiện thực thật:** giới hạn 1–11 khách lẻ, đoàn trên 12 người, đặt cọc, bảo hiểm đủ danh sách, không chồng lịch HDV, thanh toán/khảo sát sau tour.
- **Có bằng chứng kiểm thử:** test tự động gọi trực tiếp tầng Service, kiểm tra trường hợp đúng và từ chối; dữ liệu tạm được dọn sau khi chạy.
- **Truy vết rõ ràng:** yêu cầu/quy tắc → UML → Form/Service → bảng CSDL → test case, thuận tiện khi báo cáo và demo.

## 🚀 Cách chạy nhanh

1. Chạy `project/WindowsFormsApp1/Database/QuanLyCongTyDuLich.sql` trên SQL Server.
2. Mở `project/WindowsFormsApp1/WindowsFormsApp1.csproj` bằng Visual Studio và Build.
3. Kiểm tra chuỗi kết nối trong `App.config`, sau đó chạy ứng dụng.
4. (Tùy chọn) Build và chạy `project/WindowsFormsApp1/Testing/Task4ServiceTests.csproj` để kiểm thử lại.

---

📌 **Tên hệ thống:** `QuanLyCongTyDuLich`  
🎯 **Mục tiêu:** nhất quán từ phân tích nghiệp vụ đến mã nguồn và kiểm thử thực tế.
