using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebApi.Api.Controllers;
using WebApi.Api.Model;
using WebApi.Application.Application;
using WebApi.Core.Entities;
using WebApi.Infrastructure.RabbitMQ;

namespace WebApi.Test;

public class WebApiUserControllerUnitTest
{
    private readonly ILogger<UserController> _mockLogger = Substitute.For<ILogger<UserController>>();
    private readonly IMessageBusClient _rabbit = Substitute.For<IMessageBusClient>();
    private readonly User _user = new() { Id = new Guid(), Password = "password", UserName = "username" };

    [Fact]
    public async Task GetAll_ShouldReturnWithExactSameGivenUser()
    {
        var mockUserRepository = Substitute.For<IUserRepository>();
        mockUserRepository.GetAllAsync().Returns(Task.FromResult(new List<User> { _user }));

        var userController = new UserController(_mockLogger, mockUserRepository, _rabbit);

        var result = await userController.GetAll();

        result.Should().BeEquivalentTo(new OkObjectResult(new ApiResponse<List<GetUserDto>>
        {
            Success = true,
            Result = new List<GetUserDto> { new() { Id = _user.Id, UserName = _user.UserName } }
        }));
    }

    [Fact]
    public async Task GetById_ShouldReturnWithExactSameGivenUser()
    {
        var mockUserRepository = Substitute.For<IUserRepository>();
        mockUserRepository.GetByIdAsync(_user.Id.ToString()).Returns(Task.FromResult<User?>(_user));

        var userController = new UserController(_mockLogger, mockUserRepository, _rabbit);

        var result = await userController.GetById(_user.Id.ToString());

        result.Should().BeEquivalentTo(new OkObjectResult(new ApiResponse<User?>
        {
            Success = true,
            Result = _user
        }));
    }
}