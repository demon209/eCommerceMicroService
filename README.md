

````markdown
# eCommerce Microservice Project

## Mô tả
Đây là dự án microservices cho ứng dụng thương mại điện tử bao gồm các dịch vụ:
- Product API
- Order API
- Authentication API
- API Gateway

Ứng dụng được xây dựng bằng .NET, sử dụng Docker để container hóa các service và kết nối với Oracle database.

## Yêu cầu
- .NET 7.0 SDK hoặc mới hơn
- Docker & Docker Compose
- Oracle Database (hoặc kết nối đến Oracle DB)

## Cấu trúc thư mục
- `eCommerce.ProductApiSol/` - Dịch vụ quản lý sản phẩm
- `eCommerce.OrderApiSol/` - Dịch vụ quản lý đơn hàng
- `eCommerce.AuthenticationApiSol/` - Dịch vụ xác thực người dùng
- `eCommerce.ApiGatewaySol/` - API Gateway tổng hợp các dịch vụ

## Hướng dẫn cài đặt và chạy

1. Clone project về:
   ```bash
   git clone https://github.com/demon209/eCommerceMicroService.git
   cd eCommerceMicroService
````

2. Build và chạy các container bằng Docker Compose:

   ```bash
   docker-compose up --build
   ```

## Cấu hình

Các biến cấu hình nằm trong file `appsettings.json` của từng dịch vụ.

Bao gồm:

* Cấu hình kết nối database
* Cấu hình API Gateway
* Cấu hình JWT authentication

## API - GET POST UPDATE DELETE

### Product API:

* `GET /api/products/{id}` - Lấy thông tin sản phẩm theo ID

### Order API:

* `GET /api/orders/{id}` - Lấy thông tin đơn hàng chi tiết

### Authentication API:

* `GET /api/authentications/{id}` - Lấy thông tin user theo ID

### API Gateway:

* Tổng hợp và định tuyến các yêu cầu tới các dịch vụ phía sau

## Công cụ hỗ trợ

* Polly: Retry và resilience khi gọi các API nội bộ
* Serilog: Logging

## Liên hệ

Nếu có thắc mắc hoặc góp ý, vui lòng liên hệ qua email: **[vuquan150904@gmail.com](mailto:vuquan150904@gmail.com)**

---

README này có thể được chỉnh sửa và bổ sung theo tiến độ phát triển dự án.

```

