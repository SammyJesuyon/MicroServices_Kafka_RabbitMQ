using Confluent.Kafka;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "ServiceB",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("TestTopic");

        try
        {
            while (true)
            {
                var result = consumer.Consume(TimeSpan.FromSeconds(1));
                if (result?.Message != null)
                {
                    Console.WriteLine(
                        $"Received: {result.Message.Value} at partition {result.Partition} at offset {result.Offset}");
                }
            }
        }
        catch (ConsumeException e)
        {
            Console.WriteLine($"Error occured: {e.Error.Reason}");
        }
        finally
        {
            consumer.Close();
        }
    }
}