
using eCommerceSharedLibrary.Interface;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    // Kế thừa các Interface của SharedLibrary
    public interface IProduct : IGenericInterface<Product> { }

}
