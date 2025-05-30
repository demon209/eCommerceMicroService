

using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    // DTO chứa thông tin sản phẩm
    public record ProductDTO
    (
     int Id,
     [Required] String Name,
     [Required, Range(1, int.MaxValue)] int Quantity,
     [Required, DataType(DataType.Currency)] decimal Price
    );
}
