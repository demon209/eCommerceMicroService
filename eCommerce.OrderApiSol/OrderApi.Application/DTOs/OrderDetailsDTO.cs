
using System.ComponentModel.DataAnnotations;


namespace OrderApi.Application.DTOs
{
    // DTO chứa thông tin chi tiết hóa đơn
    public record OrderDetailsDTO
    (
       [Required] int OrderId,
       [Required] int ProductId,      
       [Required] string ProductName, 
       [Required] int ClientId,        
       [Required] string Name,
       [Required, EmailAddress] string Email,
       [Required ]string TelephoneNumber,
       [Required] string Address,
       [Required] int PurchaseQuantity,
       [Required, DataType(DataType.Currency)] decimal UnitPrice,
       [Required, DataType(DataType.Currency)] decimal TotalPrice,
       [Required] DateTime OrderDate
     );
}
