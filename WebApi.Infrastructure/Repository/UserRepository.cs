using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using WebApi.Application.Application;
using WebApi.Core.Entities;

namespace WebApi.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionString") ??
                            throw new Exception("empty connection string");
    }

    public async Task<string> AddAsync(User entity)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync();

        const string addUserSp = "add_user_sp";
        var parameters = new { entity.UserName, entity.Password };
        var result = await con.ExecuteAsync(addUserSp, parameters, commandType: CommandType.StoredProcedure);
        return result.ToString();
    }

    public async Task<string> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAllAsync()
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync();

        const string getUserSp = "get_users_sp";
        var result = await con.QueryAsync<User>(getUserSp, commandType: CommandType.StoredProcedure);
        return result.AsList();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync();

        const string getUserSp = "get_user_sp";
        var parameters = new { Id = id };
        var result = await con.QueryAsync<User>(getUserSp, parameters, commandType: CommandType.StoredProcedure);
        return result.FirstOrDefault();
    }

    public async Task<string> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserWithCredentialAsync(string userName, string password)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync();

        const string getUserSp = "get_user_with_credentials_sp";
        var parameters = new { UserName = userName, Password = password };
        var result = await con.QueryAsync<User>(getUserSp, parameters, commandType: CommandType.StoredProcedure);
        return result.FirstOrDefault();
    }
}