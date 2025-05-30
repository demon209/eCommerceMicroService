using System.Linq.Expressions;
using eCommerceSharedLibrary.Interface;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Interface
{
    public interface IOrder: IGenericInterface<Order> // Kế thừa các phương thức từ Interface chung
    {
        // Khai báo thêm phương thức mở rộng - lấy danh sách đơn hàng theo điều kiện tùy chọn
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
    }
}
