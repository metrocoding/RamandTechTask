namespace WebApi.Api.Model;

public class AddUserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginDto : AddUserDto
{
}

public class GetUserDto
{
    public string UserName { get; set; }
    public Guid Id { get; set; }
}