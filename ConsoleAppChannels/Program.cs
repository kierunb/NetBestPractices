using System.Threading.Channels;


// https://learn.microsoft.com/en-us/dotnet/core/extensions/channels
Console.WriteLine("Hello, Channels!");
Console.WriteLine();

await ClassBasedScenario(numberOfMessages: 30);

//await SimpleScenario(numberOfMessages: 100);


static async Task ClassBasedScenario(int numberOfMessages)
{
    Console.WriteLine("=== Class-Based Channel Example ===");
    Console.WriteLine();

    // Create an unbounded channel for WorkItem messages
    var channel = Channel.CreateUnbounded<WorkItem>();

    // Create producer and consumer instances
    var producer = new Producer(channel.Writer);
    var consumer = new Consumer(channel.Reader);

    // Start producer and consumer tasks
    var producerTask = producer.ProduceAsync(numberOfMessages);
    var consumerTask = consumer.ConsumeAsync();

    // Wait for both tasks to complete
    await Task.WhenAll(producerTask, consumerTask);

    Console.WriteLine();
    Console.WriteLine("Class-based scenario completed!");
    Console.WriteLine();
}

static async Task SimpleScenario(int numberOfMessages)
{
    // Create an unbounded channel
    var channel = Channel.CreateUnbounded<int>();

    // Producer task - writes items to the channel
    var producer = Task.Run(async () =>
    {
        Console.WriteLine("Producer: Starting...");
        for (int i = 1; i <= numberOfMessages; i++)
        {
            await channel.Writer.WriteAsync(i);
            Console.WriteLine($"Producer: Sent {i}");
            await Task.Delay(100); // Simulate work
        }
        channel.Writer.Complete(); // Signal no more items
        Console.WriteLine("Producer: Completed");
    });

    // Consumer task - reads items from the channel
    var consumer = Task.Run(async () =>
    {
        Console.WriteLine("Consumer: Starting...");
        await foreach (var item in channel.Reader.ReadAllAsync())
        {
            Console.WriteLine($"Consumer: Received {item}");
            await Task.Delay(200); // Simulate processing
        }
        Console.WriteLine("Consumer: Completed");
    });

    // Wait for both tasks to complete
    await Task.WhenAll(producer, consumer);

    Console.WriteLine();
    Console.WriteLine("All done!");
}

// Message class to represent data in the channel
class WorkItem
{
    public int Id { get; init; }
    public string Data { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }

    public override string ToString() =>
        $"WorkItem [Id={Id}, Data={Data}, Timestamp={Timestamp:HH:mm:ss.fff}]";
}

// Producer class - writes messages to the channel
class Producer
{
    private readonly ChannelWriter<WorkItem> _writer;

    public Producer(ChannelWriter<WorkItem> writer)
    {
        _writer = writer;
    }

    public async Task ProduceAsync(int count)
    {
        Console.WriteLine("Producer: Starting...");
        
        try
        {
            for (int i = 1; i <= count; i++)
            {
                var workItem = new WorkItem
                {
                    Id = i,
                    Data = $"Task_{i}",
                    Timestamp = DateTime.Now
                };

                await _writer.WriteAsync(workItem);
                Console.WriteLine($"Producer: Sent {workItem}");
                await Task.Delay(50); // Simulate work
            }
        }
        finally
        {
            _writer.Complete(); // Signal no more items
            Console.WriteLine("Producer: Completed");
        }
    }
}

// Consumer class - reads messages from the channel
class Consumer
{
    private readonly ChannelReader<WorkItem> _reader;

    public Consumer(ChannelReader<WorkItem> reader)
    {
        _reader = reader;
    }

    public async Task ConsumeAsync()
    {
        Console.WriteLine("Consumer: Starting...");
        
        await foreach (var workItem in _reader.ReadAllAsync())
        {
            Console.WriteLine($"Consumer: Received {workItem}");
            await Task.Delay(300); // Simulate processing
        }
        
        Console.WriteLine("Consumer: Completed");
    }
}