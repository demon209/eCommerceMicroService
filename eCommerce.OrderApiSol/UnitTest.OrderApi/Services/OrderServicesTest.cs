using OrderApi.Application.Interface;
using OrderApi.Application.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using OrderApi.Application.DTOs;
using FluentAssertions;
using System.Net.Http.Json;
using OrderApi.Domain.Entities;
using System.Linq.Expressions;

namespace UnitTest.OrderApi.Services
{
    public class OrderServicesTest
    {
        private readonly IOrderService orderServiceInterface;
        private readonly IOrder orderInterface;
        public OrderServicesTest()
        {
            orderInterface = A.Fake<IOrder>();
            orderServiceInterface = A.Fake<IOrderService>();
        }


        // Create Fake HTTP Message Handler
        public class FakeHttpMessageHandler(HttpResponseMessage response): HttpMessageHandler
        {
            private readonly HttpResponseMessage _response = response;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
               => Task.FromResult(_response);
        } 

        //Create Fake client using fake http message handler
        private static HttpClient CreateFakeHttpClient(object o)
        {
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = JsonContent.Create(o)
            };
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(httpResponseMessage);
            var _httpClient = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };
            return _httpClient;
        }

        //Get product
        [Fact]
        public async Task GetProduct_ValidProductId_ReturnProduct()
        {
            //Arrange
            int productId = 1;
            var productDTO = new ProductDTO(1, "Product 1", 13, 56.78m);
            var _httpClient = CreateFakeHttpClient(productDTO);

            //System Under Test - SUT
            // We only the httpclient to make calls
            //specify only httpclient and null to the rest
            var _orderService = new OrderService(null!, _httpClient, null!);

            // Act
            var result = await _orderService.GetProduct(productId); 

            // assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be("Product 1");
        }
        [Fact]
        public async Task GetProduct_InValidProductId_ReturnNull()
        {
            //Arrange
            int productId = 1;
            var _httpClient = CreateFakeHttpClient(null!);

            //System Under Test - SUT
            // We only the httpclient to make calls
            //specify only httpclient and null to the rest
            var _orderService = new OrderService(null!, _httpClient, null!);

            // Act
            var result = await _orderService.GetProduct(productId);

            // assert
            result.Should().BeNull();
        }
        //Get client order by id
        [Fact]
        public async Task GetOrderByClientId_OrderExits_ReturnOrderDetails()
        {
            //Arrange
            int clientId = 1;
            var orders = new List<Order>()
            {
                new(){Id = 1, ProductId = 1, ClientId = clientId, PurchaseQuantity = 2, OrderedDate = DateTime.UtcNow },
                new(){Id = 1, ProductId = 2, ClientId = clientId, PurchaseQuantity = 1, OrderedDate = DateTime.UtcNow }
            };

            //Mock the GetOrderBy method
            A.CallTo(()=> orderInterface.GetOrdersAsync(A<Expression<Func<Order, bool>>>.Ignored)).Returns(orders);
            var _orderService = new OrderService(orderInterface!, null!, null!);

            //Act
            var result = await _orderService.GetOrdersByClientId(clientId);

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(orders.Count());
            result.Should().HaveCountGreaterThanOrEqualTo(2);
        }
    }
}
