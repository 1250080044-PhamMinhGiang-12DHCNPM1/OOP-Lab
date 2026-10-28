# 🌏 LAB05 — Quản lý Công ty Du lịch Văn Hóa Việt

> Bài thực hành Phân tích thiết kế hướng đối tượng, hiện thực bằng **C# WinForms .NET Framework 4.7.2** và **SQL Server**.

## ✅ Công việc đã hoàn thành hôm nay

| Hạng mục | Kết quả |
| --- | --- |
| 🧭 Phân tích | Hoàn thiện luồng tour, chuyến lẻ, khách lẻ/đoàn, HDV, thanh toán, khảo sát và lương. |
| 📐 UML | Có 7 sơ đồ: lớp phân tích, use case, hoạt động, tuần tự, lớp thiết kế và trạng thái. Nguồn `.drawio` có thể chỉnh sửa nằm trong `thietke/`. |
| 🗄️ CSDL | Script `Database/QuanLyCongTyDuLich.sql` tạo database, bảng, khóa, ràng buộc, chỉ mục và dữ liệu mẫu. |
| 🖥️ Ứng dụng | tạo thành công `QuanLyCongTyDuLich`; kiến trúc `Forms → Services → Data/Db.cs → SQL Server`. |
| 🧪 Kiểm thử | Bộ test Service chạy **17/17 đạt**, phủ tour, đăng ký lẻ/đoàn, cọc, bảo hiểm, HDV, thanh toán, khảo sát, lương và thống kê. |
| 📝 Báo cáo | Có báo cáo kiểm thử & truy vết !

## ✨ 1 Số điểm nổi bật 

- **Từ mô tả đến sản phẩm chạy được:** tài liệu mẫu là chuẩn trình bày và yêu cầu; bài làm này có thêm ứng dụng WinForms, CSDL SQL Server và dữ liệu mẫu thực thi được.
- **UML do nhóm thiết kế:** 7 sơ đồ được tạo mới và lưu cả ảnh lẫn file draw.io, không chỉ dừng ở ảnh minh họa tĩnh.
- **Quy tắc được hiện thực thật:** giới hạn 1–11 khách lẻ, đoàn trên 12 người, đặt cọc, bảo hiểm đủ danh sách, không chồng lịch HDV, thanh toán/khảo sát sau tour.
- **Có bằng chứng kiểm thử:** test tự động gọi trực tiếp tầng Service, kiểm tra trường hợp đúng và từ chối; dữ liệu tạm được dọn sau khi chạy.
- **Truy vết rõ ràng:** yêu cầu/quy tắc → UML → Form/Service → bảng CSDL → test case, thuận tiện khi báo cáo và demo.


## 🛠️ Công nghệ sử dụng

![C#](https://img.shields.io/badge/C%23-WinForms-512BD4?logo=csharp&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.7.2-5C2D91)
![SQL Server](https://img.shields.io/badge/SQL_Server-Windows_Authentication-CC2927?logo=microsoftsqlserver&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-2022-5C2D91?logo=visualstudio&logoColor=white)
![draw.io](https://img.shields.io/badge/draw.io-UML-F08705?logo=diagramsdotnet&logoColor=white)

## 📁 Cấu trúc chính

```text
LAB06/
├── README.md
├── word/
│   ├── Bai_6_Quan_ly_cong_ty_du_lich.docx
│   └── PhamMinhGiang-12DHCNPM1-1250080044.docx
├── thietke/
│   └── PhamMinhGiang-12DHCNPM1-LAB06.drawio
└── project/Quan_ly_cong_ty_du_lich/
    ├── Forms/FrmMain.cs
    ├── Services/
    ├── Data/Db.cs
    ├── ....


```
## 👨‍🎓 Thông tin sinh viên

- **Họ và tên:** Phạm Minh Giang
- **MSSV:** 1250080044
- **Lớp:** 12DHCNPM1
- **Môn học:** Phân tích thiết kế hướng đối tượng

---

<p align="center">✨ UML nhất quán · CSDL chặt chẽ · Code phân tầng · Kiểm thử truy vết ✨</p>
