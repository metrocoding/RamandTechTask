using WebApi.Core.Entities;

namespace WebApi.Application.Application;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserWithCredentialAsync(string userName, string password);
}