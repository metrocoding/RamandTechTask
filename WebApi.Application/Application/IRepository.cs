using WebApi.Core.Entities;

namespace WebApi.Application.Application;

public interface IRepository<T> where T : class
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<string> AddAsync(T entity);
    Task<string> UpdateAsync(T entity);
    Task<string> DeleteAsync(string id);
}