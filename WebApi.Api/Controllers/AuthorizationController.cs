using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.JWT;
using WebApi.Api.Model;
using WebApi.Application.Application;

namespace WebApi.Api.Controllers;

public class AuthorizationController : BaseApiController
{
    private readonly IJwtManager _jWtManager;
    private readonly ILogger<AuthorizationController> _logger;
    private readonly IUserRepository _userRepository;

    public AuthorizationController(IJwtManager jWtManager, ILogger<AuthorizationController> logger,
        IUserRepository userRepository)
    {
        _jWtManager = jWtManager;
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpPost]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public async Task<IActionResult> Authenticate(LoginDto loginDto)
    {
        var apiResponse = new ApiResponse<string>();
        try
        {
            var user = await _userRepository.GetUserWithCredentialAsync(loginDto.UserName, loginDto.Password);
            if (user is null) return Unauthorized();
            
            var token = _jWtManager.GenerateToken(user);
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            apiResponse.Success = true;
            apiResponse.Result = token;
        }
        catch (Exception ex)
        {
            apiResponse.Success = false;
            _logger.LogError("Failed to authorize {Exception}", ex.Message);
        }

        return Ok(apiResponse);
    }
}