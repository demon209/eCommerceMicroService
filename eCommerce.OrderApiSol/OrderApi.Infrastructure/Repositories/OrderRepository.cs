using System.Linq.Expressions;
using eCommerceSharedLibrary.Logs;
using eCommerceSharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interface;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;

// 3:52
namespace OrderApi.Infrastructure.Repositories
{
    // Xử lý nghiệp vụ - trả về Response(Flag, Message) hoặc Throw Exception
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try 
            {
                var order = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                return order.Id > 0 ? new Response(true, "Order placed successfully") :
                    new Response(false, "Error coccured while placing order");
            }
            catch(Exception ex)
            {
                // Log Original Exception
                LogException.LogExceptions(ex);

                // Display scary-free message to client
                return new Response(false, "Error occured while placing order");
            }

        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                var order = await FindByIdAsync(id);
                if (order == null)
                    return new Response(false, "Order not found");

                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true, "Order successfully deleted");

            }
            catch (Exception ex)
            {
                // Log Original Exception
                LogException.LogExceptions(ex);

                // Display scary-free message to client
                return new Response(false, "Error occured while placing order");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders!.FindAsync(id);
                return order != null ? order : null!;

            }
            catch (Exception ex)
            {
                // Log Original Exception
                LogException.LogExceptions(ex);

                // Display scary-free message to client
                throw new Exception("Error occured while retrieving order");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                return orders != null ? orders : null!;

            }
            catch (Exception ex)
            {
                // Log Original Exception
                LogException.LogExceptions(ex);

                // Display scary-free message to client
                throw new Exception("Error occured while retrieving order");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
                return order != null ? order : null!;

            }
            catch (Exception ex)
            {
                // Log Original Exception
                LogException.LogExceptions(ex);

                // Display scary-free message to client
                throw new Exception("Error occured while retrieving order");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();
                return orders != null ? orders : null!;
            }
            catch (Exception ex)
            {
                // Log Original Exception
                LogException.LogExceptions(ex);

                // Display scary-free message to client
                throw new Exception("Error occured while placing order");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if(order == null)
                    return new Response(false, $"Order have an {entity.Id} not found");
                context.Entry(order).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true,"Order Updated!");
            }
            catch (Exception ex)
            {
                // Log Original Exception
                LogException.LogExceptions(ex);

                // Display scary-free message to client
                return new Response(false, "Error occured while placing order");
            }
        }
    }
}
