
using WebApi.Infrastructure.Constants;

namespace WebApi.Infrastructure.RabbitMQ;

public interface IMessageBusClient
{
    void PublishUser(RabbitUserDto userDto);
}