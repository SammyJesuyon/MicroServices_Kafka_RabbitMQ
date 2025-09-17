using Confluent.Kafka;

class Program
{
    static void Main(string[] args)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = "ServiceA"
        };
        
        using var producer = new ProducerBuilder<Null, string>(config).Build();

        try
        {
            string message = "Hello from ServiceA!";
            var result = producer.ProduceAsync("TestTopic", new Message<Null, string> { Value = message }).Result;
            Console.WriteLine($"Sent: {message} to partition {result.Partition} at offset {result.Offset}");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Failed to deliver message: {e.Message}");
        }
        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }
}