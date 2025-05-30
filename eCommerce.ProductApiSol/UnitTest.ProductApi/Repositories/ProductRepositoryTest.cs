using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;



namespace UnitTest.ProductApi.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly ProductDbContext productDbContext;
        private readonly ProductRepository productRepository;


        public ProductRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductDb").Options;

            productDbContext = new ProductDbContext(options);

            // XÓA TOÀN BỘ DỮ LIỆU TRONG CƠ SỞ DỮ LIỆU IN-MEMORY
            productDbContext.Database.EnsureDeleted();
            productDbContext.Database.EnsureCreated();


            productRepository = new ProductRepository(productDbContext);    

        }

        // Create product
        [Fact]
        public async Task CreateAsync_WhenProductAlreadyExists_ReturnErrorResponse()
        {
            // arrange
            var existingProduct = new Product() { Name = "ExistingProduct" };
            productDbContext.Products.Add(existingProduct);
            await productDbContext.SaveChangesAsync();

            //Act
            var result = await productRepository.CreateAsync(existingProduct);

            // assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("ExistingProduct already added");
        }

        [Fact]
        public async Task CreateAsync_WhenProductDoesNotExist_ReturnOkResponse()
        {
            // arrange
            var newProduct = new Product() { Name = "newProduct" };
            //productDbContext.Products.Add(newProduct);
            //await productDbContext.SaveChangesAsync();

            //Act
            var result = await productRepository.CreateAsync(newProduct);

            // assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("newProduct added to database successfully");
        }

        //Deleted
        [Fact]
        public async Task DeleteAsync_WhenProductIsFound_ReturnsOkResponse()
        {
            var product = new Product() {Id = 1, Name = "Existing product", Price = 13, Quantity = 5};

            productDbContext.Products.Add(product);

            var result = await productRepository.DeleteAsync(product);

            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Existing product is deleted successfully");

        }

        [Fact]
        public async Task DeleteAsync_WhenProductIsNotFound_ReturnsNotFoundResponse()
        {
            var product = new Product() { Id = 16, Name = "NonExisting product", Price = 13, Quantity = 5 };
            var result = await productRepository.DeleteAsync(product);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("NonExisting product not found");

        }
        // Get product by id
        [Fact]
        public async Task FindByIdAsync_WhenProductIsFound_ReturnsProduct()
        {
            var product = new Product() { Id = 1, Name = "Existing product", Price = 13, Quantity = 5 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();


            var result = await productRepository.FindByIdAsync(product.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Existing product");
        }

        [Fact]
        public async Task FindByIdAsync_WhenProductIsNotFound_ReturnsNotFound()
        {

            var result = await productRepository.FindByIdAsync(99);

            result.Should().BeNull();
        }

        //Get all products
        [Fact]
        public async Task GetAllProduct_WhenProductsAreFound_ReturnProducts()
        {
            var products = new List<Product> 
            {

                new(){Id = 34, Name = "Product 1"},
                new(){Id = 40, Name = "Product 2"},

            };
            productDbContext.Products.AddRange(products);
            await productDbContext.SaveChangesAsync();

            var result =  await productRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
        }



        [Fact]
        public async Task GetAllProduct_WhenProductsAreNotFound_ReturnNotFound()
        {

            var result = await productRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        //Update Product
        [Fact]
        public async Task UpdatePruduct_WhenProductIsUpdateSuccess_ReturnSuccess()
        {
            var products = new Product()
            {
                Id = 3, Name = "Product 1",
            };
            productDbContext.Products.Add(products);
            await productDbContext.SaveChangesAsync();
            

            var result = await productRepository.UpdateAsync(products);


            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product 1 is updated successfully");
        }
        [Fact]
        public async Task UpdatePruduct_WhenProductIsUpdateFails_ReturnFails()
        {
            var products = new Product()
            {
                Id = 3,
                Name = "Product 11",
            };

            var result = await productRepository.UpdateAsync(products);


            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product 11 not found");
        }
    }
}
