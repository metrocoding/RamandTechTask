using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.Model;
using WebApi.Application.Application;
using WebApi.Core.Entities;
using WebApi.Infrastructure.Constants;
using WebApi.Infrastructure.RabbitMQ;

namespace WebApi.Api.Controllers;

[ApiVersion("1.0")]
public class UserController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;
    private readonly IMessageBusClient _messageBusClient;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, IMessageBusClient messageBusClient)
    {
        _userRepository = userRepository;
        _messageBusClient = messageBusClient;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var apiResponse = new ApiResponse<List<GetUserDto>>();

        try
        {
            var users = await _userRepository.GetAllAsync();
            apiResponse.Success = true;
            apiResponse.Result = users.Select(u => new GetUserDto { Id = u.Id, UserName = u.UserName }).ToList();
        }
        catch (SqlException ex)
        {
            apiResponse.Success = false;
            apiResponse.Message = ex.Message;
            _logger.LogError("SQL Exception: {Exception}", ex.Message);
        }
        catch (Exception ex)
        {
            apiResponse.Success = false;
            apiResponse.Message = ex.Message;
            _logger.LogError("Exception: {Exception}", ex.Message);
        }

        return Ok(apiResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var apiResponse = new ApiResponse<User?>();

        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            apiResponse.Success = true;
            apiResponse.Result = user;

            if (user != null)
            {
                _messageBusClient.PublishUser(new RabbitUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName
                });
            }
        }
        catch (SqlException ex)
        {
            apiResponse.Success = false;
            apiResponse.Message = ex.Message;
            _logger.LogError("SQL Exception: {Exception}", ex);
        }
        catch (Exception ex)
        {
            apiResponse.Success = false;
            apiResponse.Message = ex.Message;
            _logger.LogError("Exception: {Exception}", ex);
        }

        return Ok(apiResponse);
    }

    // [HttpPost]
    // public async Task<ApiResponse<string>> Add(AddUserDto userDto)
    // {
    //     var apiResponse = new ApiResponse<string>();
    //
    //     try
    //     {
    //
    //         var data = await _unitOfWork.Users.AddAsync(userDto.ToUserEntity());
    //         apiResponse.Success = true;
    //         apiResponse.Result = data;
    //     }
    //     catch (SqlException ex)
    //     {
    //         apiResponse.Success = false;
    //         apiResponse.Message = ex.Message;
    //         _logger.LogError("SQL Exception:", ex);
    //     }
    //     catch (Exception ex)
    //     {
    //         apiResponse.Success = false;
    //         apiResponse.Message = ex.Message;
    //         _logger.LogError("Exception:", ex);
    //     }
    //
    //     return apiResponse;
    // }
    //
    // [HttpPut]
    // public async Task<ApiResponse<string>> Update(AddUserDto userDto)
    // {
    //     var apiResponse = new ApiResponse<string>();
    //
    //     try
    //     {
    //         var data = await _unitOfWork.Users.UpdateAsync(userDto.ToUserEntity());
    //         apiResponse.Success = true;
    //         apiResponse.Result = data;
    //     }
    //     catch (SqlException ex)
    //     {
    //         apiResponse.Success = false;
    //         apiResponse.Message = ex.Message;
    //         _logger.LogError("SQL Exception:", ex);
    //     }
    //     catch (Exception ex)
    //     {
    //         apiResponse.Success = false;
    //         apiResponse.Message = ex.Message;
    //         _logger.LogError("Exception:", ex);
    //     }
    //
    //     return apiResponse;
    // }
    //
    // [HttpDelete]
    // public async Task<ApiResponse<string>> Delete(string id)
    // {
    //     var apiResponse = new ApiResponse<string>();
    //
    //     try
    //     {
    //         var data = await _unitOfWork.Users.DeleteAsync(id);
    //         apiResponse.Success = true;
    //         apiResponse.Result = data;
    //     }
    //     catch (SqlException ex)
    //     {
    //         apiResponse.Success = false;
    //         apiResponse.Message = ex.Message;
    //         _logger.LogError("SQL Exception:", ex);
    //     }
    //     catch (Exception ex)
    //     {
    //         apiResponse.Success = false;
    //         apiResponse.Message = ex.Message;
    //         _logger.LogError("Exception:", ex);
    //     }
    //
    //     return apiResponse;
    // }
}