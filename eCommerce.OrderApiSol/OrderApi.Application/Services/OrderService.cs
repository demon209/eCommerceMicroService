using System.Net.Http.Json;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversion;
using OrderApi.Application.Interface;
using Polly.Registry;

namespace OrderApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrder _orderInterface;
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipelineProvider<string> _resiliencePipeline;
        // private readonly ILogger<OrderService> _logger; // nếu bạn dùng ILogger

        public OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline)
        {
            _orderInterface = orderInterface;
            _httpClient = httpClient;
            _resiliencePipeline = resiliencePipeline;
        }

        // Get Product by ID
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/products/{productId}"); // Gửi yêu cầu đến api của product
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to fetch product. Status: {response.StatusCode}. Content: {content}");
            }

            var product = await response.Content.ReadFromJsonAsync<ProductDTO>();
            if (product == null)
                return null!;

            return product;
        }

        // Get User by ID
        public async Task<AppUserDTO> GetUser(int userId)
        {
            var response = await _httpClient.GetAsync($"/api/authentications/{userId}"); // gửi yêu cầu đến api của Authentications
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to fetch user. Status: {response.StatusCode}. Content: {content}");
            }

            var user = await response.Content.ReadFromJsonAsync<AppUserDTO>();
            if (user == null)
                throw new Exception("User data is null or could not be deserialized.");

            return user;
        }

        // Get Order Details by Order ID
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            try
            {
                // Truy vấn thông tin hóa đơn
                var order = await _orderInterface.FindByIdAsync(orderId);
                if (order == null || order.Id <= 0)
                    return null!;

                // retry khi lỗi
                var retryPipeline = _resiliencePipeline.GetPipeline("my-retry-pipeline");
                var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));
                var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));


                if (productDTO == null || appUserDTO == null)
                    throw new Exception("Failed to retrieve product or user data.");

                // Trả về DTO chứa thông tin chi tiết - OrderDetailsDTO không chứa password
                return new OrderDetailsDTO(
                    order.Id,
                    productDTO.Id,
                    productDTO.Name,
                    appUserDTO.Id,
                    appUserDTO.Name,
                    appUserDTO.Email,
                    appUserDTO.Address,
                    appUserDTO.TelephoneNumber,
                    order.PurchaseQuantity,
                    productDTO.Price,
                    productDTO.Price * order.PurchaseQuantity,
                    order.OrderedDate
                );
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error occurred while getting order details."); // nếu dùng ILogger
                throw new Exception($"Internal error in GetOrderDetails: {ex.Message}", ex);
            }
        }

        // Get orders by client ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            var orders = await _orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (!orders.Any()) return null!;

            var (_, orderDTOs) = OrderConversion.FromEntity(null, orders);
            return orderDTOs!;
        }
    }
}
