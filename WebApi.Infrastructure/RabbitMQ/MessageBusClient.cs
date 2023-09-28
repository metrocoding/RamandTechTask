using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using WebApi.Application.Application;
using WebApi.Infrastructure.Constants;

namespace WebApi.Infrastructure.RabbitMQ;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost", 
            Port = 5672
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: RabbitMqConfig.Exchange, type: ExchangeType.Fanout);
            _channel.QueueDeclare();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            Console.WriteLine("🐰 connected to message bus");
        }
        catch (Exception e)
        {
            Console.WriteLine($"🐰 error connecting to message bus {e.Message}");
        }
    }
    
    public void PublishUser(RabbitUserDto userDto)
    {
        var message = JsonSerializer.Serialize(userDto);
        if (_connection.IsOpen)
        {
            Console.WriteLine("🐰 rabbitmq is sending message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("🐰 rabbitmq connection is close");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: RabbitMqConfig.Exchange, routingKey: "", basicProperties: null, body: body);
        Console.WriteLine($"🐰 message sent ({message})");
    }

    private static void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("🐰 rabbitmq connection shutdown");
    }

    public void Dispose()
    {
        Console.WriteLine("🐰 message bus disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}