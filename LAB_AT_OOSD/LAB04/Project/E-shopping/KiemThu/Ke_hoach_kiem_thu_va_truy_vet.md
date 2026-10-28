# IV. KIỂM THỬ VÀ TRUY VẾT

## 4.1. Mục tiêu kiểm thử

Kiểm thử prototype e-SHOPPING nhằm xác nhận các chức năng mua hàng cơ bản hoạt động đúng: hiển thị sản phẩm, quản lý giỏ hàng, nhập thông tin nhận hàng, thanh toán mô phỏng, lưu đơn hàng và gửi thư xác nhận mô phỏng.

## 4.2. Điều kiện chuẩn bị

1. SQL Server đang chạy và đã có cơ sở dữ liệu `EShoppingDb`.
2. Đã chạy tệp `Database/01_Tao_CSDL_eShopping.sql`.
3. Mở `WindowsFormsApp1.sln` bằng Visual Studio, biên dịch và chạy chương trình.
4. Dùng dữ liệu mẫu: sản phẩm SP01, SP02, SP03; khách hàng mẫu có mã 1.

## 4.3. Kịch bản kiểm thử

| Mã | Kịch bản | Các bước thực hiện | Kết quả mong đợi |
|---|---|---|---|
| TC01 | Xem sản phẩm | Mở chương trình. | Lưới sản phẩm hiển thị 3 sản phẩm mẫu cùng mã, giá và tồn kho. |
| TC02 | Thêm sản phẩm vào giỏ | Chọn SP01, số lượng 2, nhấn **Thêm vào giỏ**. | Giỏ hàng có SP01 x 2; tổng tiền bằng 300.000 đ. |
| TC03 | Cộng dồn số lượng | Chọn tiếp SP01, số lượng 1, nhấn **Thêm vào giỏ**. | Giỏ hàng chỉ có một dòng SP01 x 3; tổng tiền bằng 450.000 đ. |
| TC04 | Đặt hàng thiếu thông tin người nhận | Có sản phẩm trong giỏ; để trống họ tên hoặc địa chỉ; nhập số thẻ hợp lệ; nhấn **Xác nhận đặt hàng**. | Hệ thống báo không thể đặt hàng và không tạo đơn hàng. |
| TC05 | Thanh toán thất bại | Có sản phẩm trong giỏ; nhập đủ người nhận; nhập số thẻ ít hơn 6 chữ số; nhấn **Xác nhận đặt hàng**. | Hệ thống báo thanh toán không thành công; không lưu đơn hàng. |
| TC06 | Đặt hàng thành công, không gửi thư | Có sản phẩm trong giỏ; nhập đủ người nhận; để trống thư điện tử; nhập số thẻ có ít nhất 6 chữ số. | Hệ thống thông báo mã đơn hàng; thêm dữ liệu vào Đơn hàng, Chi tiết đơn hàng, Giao dịch thanh toán; giỏ hàng trên giao diện được làm trống. |
| TC07 | Đặt hàng thành công và gửi thư | Thực hiện như TC06 nhưng nhập thư điện tử hợp lệ. | Đơn hàng được lưu; bộ điều hợp thư điện tử trả về thành công (mô phỏng). |
| TC08 | Kiểm tra dữ liệu đơn hàng | Sau TC06 hoặc TC07, mở SQL Server Management Studio và truy vấn các bảng đơn hàng. | Tổng tiền đơn hàng bằng tổng các dòng chi tiết; giao dịch có kết quả `Thành công`. |

## 4.4. Truy vết yêu cầu đến hiện thực và kiểm thử

| Mã yêu cầu | Yêu cầu | UML liên quan | Form / dịch vụ | Bảng CSDL | Test case |
|---|---|---|---|---|---|
| Y01 | Xem danh sách sản phẩm | Trường hợp sử dụng Xem sản phẩm; tuần tự chọn sản phẩm | Form1; DichVuMuaHang.LaySanPham(); BoDieuHopSanPham | SanPham | TC01 |
| Y02 | Thêm và cập nhật giỏ hàng | Trường hợp sử dụng Quản lý giỏ hàng; lớp Giỏ hàng, Mục giỏ hàng | Form1.NutThem_Click(); MucGioHang | GioHang, MucGioHang | TC02, TC03 |
| Y03 | Nhập người nhận và giao hàng | Trạng thái Chờ thông tin đặt hàng; lớp Người nhận | Form1.NutDatHang_Click(); NguoiNhan | DonHang | TC04, TC06 |
| Y04 | Kiểm tra và thanh toán đơn hàng | Tuần tự đặt hàng/thanh toán; hoạt động đặt hàng | DichVuMuaHang.DatHang(); BoDieuHopThanhToan | DonHang, GiaoDichThanhToan | TC05, TC06 |
| Y05 | Lưu đơn hàng và chi tiết | Lớp Đơn hàng, Chi tiết đơn hàng; trạng thái Đã ghi nhận đơn hàng | KhoDonHang.Luu() | DonHang, ChiTietDonHang | TC06, TC08 |
| Y06 | Gửi thư xác nhận nếu có thư điện tử | Trạng thái Đang gửi thư xác nhận; tuần tự đặt hàng/gửi thư | BoDieuHopThuDienTu.GuiXacNhan() | KhachHang, DonHang | TC06, TC07 |

## 4.5. Câu lệnh kiểm tra dữ liệu sau khi đặt hàng

```sql
USE EShoppingDb;

SELECT TOP 10 MaDonHang, TenNguoiNhan, TongTien, TrangThai, NgayDat
FROM DonHang
ORDER BY MaDonHang DESC;

SELECT ct.MaDonHang, sp.TenSanPham, ct.SoLuong, ct.DonGia,
       ct.SoLuong * ct.DonGia AS ThanhTien
FROM ChiTietDonHang ct
JOIN SanPham sp ON sp.MaSanPham = ct.MaSanPham
ORDER BY ct.MaDonHang DESC;

SELECT MaDonHang, SoTien, KetQua, ThoiGian
FROM GiaoDichThanhToan
ORDER BY MaGiaoDich DESC;
```

## 4.6. Kết luận kiểm thử

Các test case TC01 đến TC08 bao phủ luồng chính và hai tình huống lỗi quan trọng: thiếu thông tin người nhận, thanh toán không thành công. Việc lưu đơn hàng được kiểm tra qua ba bảng DonHang, ChiTietDonHang và GiaoDichThanhToan. Gửi thư điện tử trong prototype là mô phỏng nên không gửi thư ra bên ngoài.
