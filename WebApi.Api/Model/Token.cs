namespace WebApi.Api.Model;

public class Token
{
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}