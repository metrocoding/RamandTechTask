using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebApi.Api.Controllers;
using WebApi.Api.JWT;
using WebApi.Api.Model;
using WebApi.Application.Application;
using WebApi.Core.Entities;

namespace WebApi.Test;

public class WebApiAuthenticationUnitTest
{
    private readonly User _user = new() { UserName = "Some", Password = "PassWord" };
    private readonly LoginDto _loginDto = new() { UserName = "Some", Password = "PassWord" };
    private readonly ILogger<AuthorizationController> _mockLogger = Substitute.For<ILogger<AuthorizationController>>();

    [Fact]
    public async Task Authenticate_ReturnsUnauthorized_WhenUserIsNotFound()
    {
        var mockUserRepository = Substitute.For<IUserRepository>();
        mockUserRepository.GetUserWithCredentialAsync(_user.UserName, _user.Password).Returns(Task.FromResult<User?>(null));

        var mockJwtManager = Substitute.For<IJwtManager>();
        mockJwtManager.GenerateToken(_user).Returns("sudo token value");

        var authController = new AuthorizationController(mockJwtManager, _mockLogger, mockUserRepository);

        var result = await authController.Authenticate(_loginDto);
        result.Should().BeEquivalentTo(new UnauthorizedResult());
    }

    [Fact]
    public async Task Authenticate_ReturnsUnauthorized_WhenTokenIsNotGenerated()
    {
        var mockUserRepository = Substitute.For<IUserRepository>();
        mockUserRepository.GetUserWithCredentialAsync(_user.UserName, _user.Password).Returns(_user);

        var mockJwtManager = Substitute.For<IJwtManager>();
        mockJwtManager.GenerateToken(_user).Returns("");

        var authController = new AuthorizationController(mockJwtManager, _mockLogger, mockUserRepository);

        var result = await authController.Authenticate(_loginDto);
        result.Should().BeEquivalentTo(new UnauthorizedResult());
    }

    [Fact]
    public async Task Authenticate_ReturnsOk_WhenEverythingIsOk()
    {
        var mockUserRepository = Substitute.For<IUserRepository>();
        mockUserRepository.GetUserWithCredentialAsync(_user.UserName, _user.Password).Returns(_user);

        var mockJwtManager = Substitute.For<IJwtManager>();
        mockJwtManager.GenerateToken(_user).Returns("sudo token");

        var authController = new AuthorizationController(mockJwtManager, _mockLogger, mockUserRepository);

        var result = await authController.Authenticate(_loginDto);
        result.Should().BeEquivalentTo(new OkObjectResult(new ApiResponse<string>
        {
            Success = true,
            Result = "sudo token",
        }));
    }
}