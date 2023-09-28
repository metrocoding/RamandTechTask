namespace WebApi.Infrastructure.Constants;

public static class RabbitMqConfig
{
    public static string Exchange => "trigger";
}

public class RabbitUserDto{
    public string UserName { get; set; }
    public Guid Id { get; set; }
}