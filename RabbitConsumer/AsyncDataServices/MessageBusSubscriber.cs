using System.Text;
using RabbitConsumer.Constants;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IConnection = RabbitMQ.Client.IConnection;

namespace RabbitConsumer.AsyncDataServices;

public class MessageBusSubscriber
{
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber()
    {
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: RabbitMqConfig.Exchange, type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: RabbitMqConfig.Exchange, routingKey: "");
        Console.WriteLine("🐰 Consumer service listening to message bus");
        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("🐰 Consumer service rabbitmq connection shutdown");
    }

    public void Listen()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, eventArgs) =>
        {
            // receive from main publisher
            var body = eventArgs.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine($"event received {notificationMessage}");
            
            // send to dead letter queue
            var args = new Dictionary<string, object> { { "x-message-ttl", 10000 } };
            _channel.ExchangeDeclare(exchange: "dead-letter", type: ExchangeType.Fanout, arguments: args);
            _channel.QueueDeclare();
            body = Encoding.UTF8.GetBytes(notificationMessage);
            _channel.BasicPublish(exchange: "dead-letter", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"🐰 message sent to dead letter queue ({notificationMessage})");
        };
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
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