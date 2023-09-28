using WebApi.Core.Entities;

namespace WebApi.Api.JWT;

public interface IJwtManager
{
    // string? ValidateToken(string token);
    string GenerateToken(User user);
}