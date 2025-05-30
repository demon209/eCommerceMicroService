using eCommerceSharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversion;
using OrderApi.Application.Interface;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            await Task.Delay(4000);
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
                return NotFound("No order detected in the database");
            var (_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ? NotFound() : Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order == null)
            {
                return NotFound("No order detected in the database");
            }
            var (_order, _) = OrderConversion.FromEntity(order, null!);
            return Ok(_order);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId)
        {
            if (clientId <= 0)
                return BadRequest("Invalid data provided");
            var orders = await orderService.GetOrdersByClientId(clientId);
            return !orders.Any() ? NotFound(null) : Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid data provided");
            try
            {
                var orderDetail = await orderService.GetOrderDetails(orderId);

                if (orderDetail == null || orderDetail.OrderId <= 0)
                    return NotFound("No order found.");

                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                // Trả về lỗi chi tiết từ service, ví dụ: không lấy được product
                return BadRequest($"Error fetching order details: {ex.Message}");
            }

        }



        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }

            // convert to entity
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("No order detected in the database");

            // convert from to entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Response>> DeletedOrder(int id)
        {
            // convert from dto to entity
            var response = await orderInterface.DeleteAsync(id);
            return response.Flag? Ok(response) : BadRequest(response);
        }
    }
}
