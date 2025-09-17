using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(exchange: "fanout-exchange", durable: false, autoDelete: false, type: ExchangeType.Fanout);

        channel.QueueDeclare(queue: "serviceB", durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind("serviceB", "fanout-exchange", "");
        
        Console.WriteLine("Waiting for messages. Press [enter] to exit.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received: {message}");
            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        
        channel.BasicConsume("serviceB", autoAck: false, consumer);
        
        
        Console.ReadLine();
    }
}