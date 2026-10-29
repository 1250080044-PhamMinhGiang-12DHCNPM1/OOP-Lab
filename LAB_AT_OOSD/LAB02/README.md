# 📚 LAB02 — Hệ thống quản lý thư viện

> Bài thực hành phân tích nghiệp vụ thư viện và thiết kế giao diện C# WinForms: từ danh mục sách, thẻ độc giả đến mượn, trả, phạt và thống kê.

## 🌟 Tổng quan bài làm

- Đọc và hệ thống hóa yêu cầu trong [đề bài quản lý thư viện](Word/LAB2_He_thong_quan_ly_thu_vien.docx).
- Thiết kế **mục 6 — Hệ thống giao diện** trong [báo cáo LAB02](Word/PhamMinhGiang-12DHCNPM1-LLab02.docx): sơ đồ điều hướng, phác thảo màn hình, bảng Control và quy tắc kiểm tra dữ liệu.
- Xác định **6 Form** theo nghiệp vụ, trong đó màn hình mượn và trả có hai Tab riêng.
- Có sẵn solution **WinForms .NET Framework 4.7.2** để tiếp tục hiện thực theo hướng `Form → Service → Data → SQL Server`.

## 🧭 Các màn hình được thiết kế

| # | Màn hình | Nghiệp vụ chính |
|---:|---|---|
| 1 | 🏠 `FrmMain` | Điều hướng đến các chức năng của hệ thống |
| 2 | 🗂️ `FrmDanhMuc` | Quản lý nhân viên, thể loại và nhà xuất bản |
| 3 | 📖 `FrmSach` | Quản lý đầu sách, tìm kiếm và theo dõi số lượng hiện có |
| 4 | 🪪 `FrmDocGia` | Lưu hồ sơ độc giả, cấp hoặc gia hạn thẻ thư viện |
| 5 | 🔄 `FrmMuonTra` | Lập phiếu mượn; ghi nhận trả sách và phí phạt |
| 6 | 📊 `FrmThongKe` | Tổng hợp mượn, quá hạn, mất, hư hỏng và tiền phạt theo kỳ |

## 📌 Quy tắc nghiệp vụ trọng tâm

| Nhóm | Điều kiện cần bảo đảm |
|---|---|
| **Thẻ thư viện** | Độc giả chỉ có một thẻ còn giá trị tại một thời điểm; thẻ hết hạn hoặc chưa đóng lệ phí năm không được dùng để mượn. |
| **Mượn sách** | Mỗi độc giả mượn tối đa **3 đầu sách khác nhau**; không mượn thêm khi còn sách quá hạn; chỉ chọn sách còn trong kho. |
| **Thời gian** | Ngày hẹn trả không trước ngày mượn; ngày trả thực tế không trước ngày mượn. |
| **Trả và phạt** | Trả trễ, mất hoặc hư hỏng phải ghi nhận phí phạt cụ thể; sách mất hoặc hư hỏng không được cộng lại vào tồn kho sẵn sàng. |
| **Dữ liệu** | Khóa định danh không sửa khi cập nhật; các thao tác mượn/trả cần cập nhật phiếu, chi tiết và tồn kho nhất quán. |

## 🏗️ Hướng triển khai

```text
┌─────────────────────┐
│  Forms · WinForms UI│  Nhập liệu, danh sách, thông báo
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│      Services       │  Kiểm tra thẻ, mượn, trả và phạt
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│       Data          │  ADO.NET và câu lệnh SQL có tham số
└──────────┬──────────┘
           ▼
┌─────────────────────┐
│     SQL Server      │  Lưu trữ và ràng buộc dữ liệu
└─────────────────────┘
```

> **Trạng thái project:** thư mục `Project/OuanLyThuVien` hiện là **khung thực hành**; các Form nghiệp vụ và một số lớp Service/Data chưa được hoàn thiện. Các hình trong báo cáo là **phác thảo thiết kế**, không phải ảnh chụp ứng dụng đã chạy.

## 🛠️ Công nghệ

![C#](https://img.shields.io/badge/C%23-WinForms-512BD4?logo=csharp&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.7.2-5C2D91)
![SQL Server](https://img.shields.io/badge/SQL_Server-ADO.NET-CC2927?logo=microsoftsqlserver&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-2022-5C2D91?logo=visualstudio&logoColor=white)

## 📁 Cấu trúc thư mục

```text
LAB02/
├── README.md
├── Word/
│   ├── LAB2_He_thong_quan_ly_thu_vien.docx
│   └── PhamMinhGiang-12DHCNPM1-LLab02.docx
└── Project/OuanLyThuVien/
    ├── OuanLyThuVien.sln
    ├── Form1.cs
    ├── Models.cs
    ├── App.config
    ├── Data/Db.cs
    └── Services/
        ├── DocGiaService.cs
        └── MuonTraService.cs
```

## 👨‍🎓 Thông tin sinh viên

- **Họ và tên:** Phạm Minh Giang
- **MSSV:** 1250080044
- **Lớp:** 12DHCNPM1
- **Môn học:** Phương pháp phát triển phần mềm hướng đối tượng

---

<p align="center">✨ Từ yêu cầu thư viện → thiết kế màn hình → nền tảng hiện thực WinForms ✨</p>
