using Microsoft.AspNetCore.Mvc;

namespace WebApi.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
}