using eCommerceSharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    // Thiết kế Api cho Product
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            // get all product from repo
            var products = await productInterface.GetAllAsync();
            if(!products.Any())
                return NotFound("No products detected in the database");

            // convert data from entity to DTO
            var (_, list) = ProductConversion.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No product found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
                // get singed product from repo
            var product = await productInterface.FindByIdAsync(id);
            if(product == null)
            {
                return NotFound("Product requested not found");
            }
            // convert data from entity to DTO  
            var (_product, _) = ProductConversion.FromEntity(product, null!);
            return _product != null ? Ok(_product) : NotFound("Product not found");
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            // check model state is all data annotations are passed
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            // convert to entity 
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);

        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(int id)
        {
            var response = await productInterface.DeleteAsync(id);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
