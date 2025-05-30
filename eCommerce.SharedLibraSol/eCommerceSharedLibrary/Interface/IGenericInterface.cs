﻿using System.Linq.Expressions;
using eCommerceSharedLibrary.Responses;
namespace eCommerceSharedLibrary.Interface
{
    // Khai báo các phương thức để dùng chung
    public interface IGenericInterface<T>where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIdAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);

    }
}
