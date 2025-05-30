
using System.Linq.Expressions;
using eCommerceSharedLibrary.Logs;
using eCommerceSharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Infrastructure.Repositories
{
    // Xử lý logic các nghiệp vụ
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try 
            {
                // check if product already exist
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
                if (getProduct != null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} already added");


                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity != null && currentEntity.Id > 0)
                {
                    return new Response(true, $"{entity.Name} added to database successfully");
                }
                else return new Response(false, $"error occurred while adding {entity.Name}");

            }
            catch(Exception ex)
            {
                // Log the Global exception
                LogException.LogExceptions(ex);

                    //display scary-free message to the client
                    return new Response(false, "Error occurrec adding new product");
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                var product = await FindByIdAsync(id);
                if(product == null)
                {
                    return new Response(false, $"{product!.Name} not found");
                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response(true, $"{product!.Name} is deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the Global exception
                LogException.LogExceptions(ex);

                //display scary-free message to the client
                return new Response(false, "Error occurrec deleted product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product != null ? product : null!;
            }
            catch (Exception ex)
            {
                // Log the Global exception
                LogException.LogExceptions(ex);

                //display scary-free message to the client
                throw new Exception("Error occurred retrieving product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products != null ? products : null!;
            }
            catch (Exception ex)
            {
                // Log the Global exception
                LogException.LogExceptions(ex);

                //display scary-free message to the client
                throw new InvalidOperationException("Error occurrec get all products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return product != null ? product : null!;
            }
            catch (Exception ex)
            {
                // Log the Global exception
                LogException.LogExceptions(ex);

                //display scary-free message to the client
                throw new InvalidOperationException("Error occurrec get product");
            }

        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if(product == null)
                {
                    return new Response(false, $"{entity.Name} not found");
                }
                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true,$"{entity.Name} is updated successfully");
            }
            catch (Exception ex)
            {
                // Log the Global exception
                LogException.LogExceptions(ex);

                //display scary-free message to the client
               return new Response(false, "Error occurrec adding new product");
            }
        }
    }
}
