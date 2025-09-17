using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "fanout-exchange", durable: false, autoDelete: false, type: ExchangeType.Fanout);
        
        
        for (int i = 0; i < 10; i++)
        {
             var message = $"Hello from ServiceA! {i}";
             var body = Encoding.UTF8.GetBytes(message);
             channel.BasicPublish(
                 exchange: "fanout-exchange",
                 routingKey: "",
                 mandatory: true,
                 basicProperties: null,
                 body: body
             );
        
             Console.WriteLine($"Sent: {message}");
             
        }
    }
}