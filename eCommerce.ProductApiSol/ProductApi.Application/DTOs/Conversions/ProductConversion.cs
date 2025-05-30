using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        //Trans DTO --> Entity
        // Chuyển DTO Thành thực thể để lưu vào CSDL
        // trả về product được khởi tạo từ DTO
        public static Product ToEntity(ProductDTO product) => new()
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                Price = product.Price
            };

        // Trans List --> DTO
        // Chuyển thực thể/danh sách thành DTO để gửi về client
        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            // return single
            // chỉ có product
            if (product != null || products == null)
            {
                var singleProduct = new ProductDTO
                    (product!.Id,
                    product.Name!,
                    product.Quantity,
                    product.Price
                    );
                return (singleProduct, null);
            }

            // return list
            // chỉ có danh sách
            if (product == null || products != null) {
                var _products = products!.Select(p =>
                    new ProductDTO(p.Id, p.Name!, p.Quantity, p.Price)).ToList();

                return (null, _products);
            }

            return (null, null);
        }
    }
}
