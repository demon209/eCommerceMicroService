

````markdown
# eCommerce Microservice Project #

## Mô tả ##
Đây là dự án microservices cơ bản cho ứng dụng thương mại điện tử, 
nhằm giúp mọi người nắm được bản chất cũng như cách để xây dựng một ứng dụng microservices, 
bao gồm các dịch vụ:
- Product API: Quản lý sản phẩm
- Order API: Quản lý hóa đơn
- Authentication API: Quản lý khách hàng và token login - Quản lý role
- API Gateway: Thông qua cổng api gateway chung

Ứng dụng được xây dựng bằng .NET, sử dụng Docker để container hóa các service và kết nối với Oracle database.

## Yêu cầu ##
- .NET 7.0 SDK hoặc mới hơn
- Docker & Docker Compose
- Oracle Database (hoặc kết nối đến Oracle DB)

## Cấu trúc thư mục ##
eCommerceMicroservice
|- `eCommerce.ProductApiSol/` - Dịch vụ quản lý sản phẩm
|- `eCommerce.OrderApiSol/` - Dịch vụ quản lý đơn hàng
|- `eCommerce.AuthenticationApiSol/` - Dịch vụ xác thực người dùng
|- `eCommerce.ApiGatewaySol/` - API Gateway tổng hợp các dịch vụ
|- `eCommerce.SharedLibraSol/` - Thư viện dùng chung cho các microservices
````


## Hướng dẫn cài đặt và chạy

1. Clone project về:
   ```bash
   git clone https://github.com/demon209/eCommerceMicroService.git
   cd eCommerceMicroService
	```


2. Build và chạy các container bằng Docker Compose:

   ```bash
   docker compose build
   docker compose up -d
   ```

## DIAGRAM
![image](https://github.com/user-attachments/assets/5b19cb7c-4691-41ec-9344-e84c8ca3f544)

## Cấu hình

Các biến cấu hình nằm trong file `appsettings.json` của từng dịch vụ.

Bao gồm:

* Cấu hình kết nối database
* Cấu hình API Gateway
* Cấu hình JWT authentication

***LƯU Ý***: Dự án đã được cấu hình để chạy trên docker, nếu muốn chạy trên localhost, cần thay đổi host thành localhost trong file cấu hình ocelot của ApiGateway và thay đổi host.docker.internal thành localhost của connect string mỗi dự án.

## API - GET POST UPDATE DELETE

### Product API:

* `GET /api/products/{id}` - Lấy thông tin sản phẩm theo ID
* `GET /api/products` - Lấy thông tin danh sách sản phẩm 
* `DELETE /api/products/{id}` - Xóa sản phẩm theo ID
* `PUSH /api/products` - Thêm mới sản phẩm
* `UPDATE /api/products/{id}` - cập nhật sản phẩm
* Cần header Authorization với Value là Bearer + token để sử dụng DELETE/UPDATE/PUSH với role là Admin
### Order API:

* `GET /api/orders/{id}` - Lấy thông tin đơn hàng chi tiết
* `GET /api/orders` - Lấy thông tin danh sách đơn hàng 
* `DELETE /api/orders/{id}` - Xóa đơn hàng theo ID
* `PUSH /api/orders` - Thêm mới đơn hàng
* `UPDATE /api/orders/{id}` - cập nhật đơn hàng
* Cần header Authorization với Value là Bearer + token để sử dụng với role là Admin
### Authentication API:

* `GET /api/authentications/{id}` - Lấy thông tin user theo ID
* `PUSH /api/authentications/login` - Login với body là email + password để lấy token
* `PUSH /api/authentications/register` - Register tài khoản mới

### API Gateway:

* Tổng hợp và định tuyến các yêu cầu tới các dịch vụ phía sau
* Kí tay chứng chỉ https

## Công cụ hỗ trợ

* Polly: Retry và resilience khi gọi các API nội bộ
* Serilog: Logging
* JWT
* DOCKER

## Liên hệ

Nếu có thắc mắc hoặc góp ý, vui lòng liên hệ qua email: **[vuquan150904@gmail.com](mailto:vuquan150904@gmail.com)**

---

README này có thể được chỉnh sửa và bổ sung theo tiến độ phát triển dự án.

```

