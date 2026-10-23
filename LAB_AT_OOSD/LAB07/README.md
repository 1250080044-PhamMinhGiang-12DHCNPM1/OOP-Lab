# 🎨 LAB07 — Quản lý nhân sự Công ty Sơn Đông Á

> Bài thực hành phân tích, thiết kế và hiện thực hệ thống quản lý nhân sự bằng UML, SQL Server và C# WinForms theo kiến trúc ba tầng.

## 🌟 Kết quả nổi bật

- Phân tích trọn vẹn quy trình từ **tuyển dụng, thử việc** đến **tính và chốt lương**.
- Hoàn thành **8 trang sơ đồ draw.io** có thể tiếp tục chỉnh sửa.
- Xây dựng ứng dụng **WinForms .NET Framework 4.7.2** với **7 phân hệ nghiệp vụ**.
- Thiết kế CSDL SQL Server gồm **23 bảng** và **14 stored procedure**.
- Hoàn thành **10/10 kịch bản kiểm thử**, chạy trực tiếp trên SQL Server thật.
- Lập ma trận truy vết xuyên suốt: `Yêu cầu → UML → Form → Service → CSDL → Test`.

## 🧭 Bảy phân hệ nghiệp vụ

| # | Phân hệ | Chức năng chính |
|---:|---|---|
| 1 | 👤 Tuyển dụng – Thử việc | Tiếp nhận nhân viên, lập và đánh giá quyết định thử việc |
| 2 | 📄 Hợp đồng – Bảo hiểm | Ký hợp đồng chính thức, tạo BHXH và BHYT cho hợp đồng dài hạn |
| 3 | 💼 Lương – Chức vụ | Quản lý quyết định lương và chức vụ theo ngày hiệu lực |
| 4 | 🗓️ Kế hoạch tuần | Lập lịch làm việc theo phòng ban, nhân viên và ca |
| 5 | ⏱️ Chấm công – Phụ trội | Ghi nhận ngày công, giờ phụ trội và chốt bảng công |
| 6 | 🏆 Khen thưởng – Kỷ luật | Lập quyết định và phiếu thu/chi cho cá nhân hoặc phòng ban |
| 7 | 💰 Bảng lương | Tính lương, kiểm tra trùng và chốt bảng lương tháng |

## 🧩 Bộ mô hình UML

File draw.io gồm đúng tám trang:

1. Biểu đồ lớp phân tích.
2. Association và multiplicity.
3. Use Case tổng quát.
4. Use Case từ thử việc đến hợp đồng.
5. Activity thử việc và ký hợp đồng.
6. Sequence lập quyết định khen thưởng.
7. Kiến trúc `UI → Service → Data → SQL Server`.
8. Thiết kế cơ sở dữ liệu.

Các sơ đồ sử dụng phong cách học thuật thống nhất: nền vàng kem, đường nối hồng đỏ, bố cục rõ ràng và được chèn vào báo cáo Word.

## 🏗️ Kiến trúc ứng dụng

```text
┌──────────────────────┐
│  Forms / WinForms UI │  Nhập liệu, tra cứu, hiển thị danh sách
└──────────┬───────────┘
           ▼
┌──────────────────────┐
│       Services       │  Kiểm tra BR01–BR09 và điều phối nghiệp vụ
└──────────┬───────────┘
           ▼
┌──────────────────────┐
│         Data         │  ADO.NET, parameterized SQL, stored procedure
└──────────┬───────────┘
           ▼
┌──────────────────────┐
│      SQL Server      │  Ràng buộc, transaction và lưu trữ dữ liệu
└──────────────────────┘
```

## 🛠️ Công nghệ sử dụng

![C#](https://img.shields.io/badge/C%23-WinForms-512BD4?logo=csharp&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.7.2-5C2D91)
![SQL Server](https://img.shields.io/badge/SQL_Server-Windows_Authentication-CC2927?logo=microsoftsqlserver&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-2022-5C2D91?logo=visualstudio&logoColor=white)
![draw.io](https://img.shields.io/badge/draw.io-UML-F08705?logo=diagramsdotnet&logoColor=white)

## 📁 Cấu trúc chính

```text
LAB07/
├── README.md
├── word/
│   ├── Bai_7_Quan_ly_nhan_su.docx
│   └── PhamMinhGiang-12DHCNPM1-LAB07.docx
├── thietke/
│   └── PhamMinhGiang-12DHCNPM1-LAB07.drawio
└── project/QuanLyNhanXu/
    ├── Forms/FrmMain.cs
    ├── Services/
    ├── Data/Db.cs
    ├── Models/Dtos.cs
    ├── Database/QuanLyNhanSuDongA.sql
    ├── Testing/
    └── QuanLyNhanXu.sln
```
## 👨‍🎓 Thông tin sinh viên

- **Họ và tên:** Phạm Minh Giang
- **MSSV:** 1250080044
- **Lớp:** 12DHCNPM1
- **Môn học:** Phân tích thiết kế hướng đối tượng

---

<p align="center">✨ UML nhất quán · CSDL chặt chẽ · Code phân tầng · Kiểm thử truy vết ✨</p>
