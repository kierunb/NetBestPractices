using System.Collections.Concurrent;

Console.WriteLine("=== Queue Examples ===\n");
QueueExamples();

Console.WriteLine("\n=== ConcurrentQueue Examples ===\n");
ConcurrentQueueExamples();

Console.WriteLine("\n=== PriorityQueue Examples ===\n");
PriorityQueueExamples();

Console.WriteLine("\n=== Async Single Chain (Regular Queue OK) ===\n");
await AsyncSingleChainExamples();

Console.WriteLine("\n=== Async Concurrent Access (ConcurrentQueue Needed) ===\n");
await AsyncConcurrentExamples();

static void QueueExamples()
{
    // Typical Usage: Print job queue, breadth-first search, task processing in FIFO order
    // Use when you need simple First-In-First-Out behavior in single-threaded scenarios
    
    var printQueue = new Queue<string>();
    
    // Enqueue items (add to the end)
    printQueue.Enqueue("Document1.pdf");
    printQueue.Enqueue("Image.png");
    printQueue.Enqueue("Report.docx");
    
    Console.WriteLine($"Items in queue: {printQueue.Count}");
    
    // Peek at next item without removing
    Console.WriteLine($"Next to print: {printQueue.Peek()}");
    
    // Dequeue items (remove from the front)
    while (printQueue.Count > 0)
    {
        string document = printQueue.Dequeue();
        Console.WriteLine($"Printing: {document}");
    }
}

static void ConcurrentQueueExamples()
{
    // Typical Usage: Multi-threaded producer-consumer pattern, thread-safe message queues
    // Use when multiple threads need to add/remove items safely without manual locking
    
    var messageQueue = new ConcurrentQueue<string>();
    
    // Producer thread simulation
    var producerTask = Task.Run(() =>
    {
        for (int i = 1; i <= 5; i++)
        {
            messageQueue.Enqueue($"Message {i}");
            Console.WriteLine($"Producer: Added Message {i}");
            Thread.Sleep(100);
        }
    });
    
    // Consumer thread simulation
    var consumerTask = Task.Run(() =>
    {
        Thread.Sleep(50); // Start slightly after producer
        for (int i = 1; i <= 5; i++)
        {
            string? message = null;
            while (!messageQueue.TryDequeue(out message))
            {
                Thread.Sleep(10); // Wait for messages
            }
            Console.WriteLine($"Consumer: Processed {message}");
            Thread.Sleep(150);
        }
    });
    
    Task.WaitAll(producerTask, consumerTask);
    Console.WriteLine($"Remaining messages: {messageQueue.Count}");
}

static void PriorityQueueExamples()
{
    // Typical Usage: Task scheduling, emergency room triage, Dijkstra's algorithm, event handling by importance
    // Use when items should be processed by priority rather than order of arrival
    
    var taskQueue = new PriorityQueue<string, int>();
    
    // Enqueue with priority (lower number = higher priority)
    taskQueue.Enqueue("Write documentation", 3);
    taskQueue.Enqueue("Fix critical bug", 1);
    taskQueue.Enqueue("Code review", 2);
    taskQueue.Enqueue("Update README", 4);
    taskQueue.Enqueue("Security patch", 1);
    
    Console.WriteLine("Processing tasks by priority:");
    while (taskQueue.Count > 0)
    {
        string task = taskQueue.Dequeue();
        Console.WriteLine($"- {task}");
    }
    
    Console.WriteLine("\nEmergency Room Triage Example:");
    var erQueue = new PriorityQueue<string, int>();
    
    erQueue.Enqueue("Patient A - Minor cut", 3);
    erQueue.Enqueue("Patient B - Heart attack", 1);
    erQueue.Enqueue("Patient C - Broken arm", 2);
    erQueue.Enqueue("Patient D - Flu symptoms", 4);
    
    while (erQueue.Count > 0)
    {
        Console.WriteLine($"Treating: {erQueue.Dequeue()}");
    }
}

static async Task AsyncSingleChainExamples()
{
    // Typical Usage: Sequential async processing, API calls in order, file processing pipeline
    // Use regular Queue<T> when: Only ONE async operation accesses the queue at a time
    // No concurrent access = No need for thread-safe collections
    
    var apiCallQueue = new Queue<string>(); // Regular Queue is sufficient
    
    // Fill queue with API endpoints to call
    apiCallQueue.Enqueue("https://api.example.com/users");
    apiCallQueue.Enqueue("https://api.example.com/products");
    apiCallQueue.Enqueue("https://api.example.com/orders");
    
    Console.WriteLine("Processing API calls sequentially (single async chain):");
    
    // Process items one at a time in async chain
    // Even though we use 'await', only ONE operation accesses the queue at any moment
    while (apiCallQueue.Count > 0)
    {
        string endpoint = apiCallQueue.Dequeue(); // Safe - no concurrent access
        await SimulateApiCallAsync(endpoint);
        Console.WriteLine($"✓ Completed: {endpoint}");
    }
    
    Console.WriteLine("All API calls completed sequentially.\n");
}

static async Task AsyncConcurrentExamples()
{
    // Typical Usage: Multiple async producers/consumers, parallel processing, real-time data feeds
    // Use ConcurrentQueue<T> when: MULTIPLE async operations might access the queue simultaneously
    // Concurrent access = MUST use thread-safe collections to prevent race conditions
    
    var dataQueue = new ConcurrentQueue<string>(); // ConcurrentQueue required!
    bool producerFinished = false;
    
    Console.WriteLine("Processing with multiple concurrent async operations:");
    
    // Producer: Async operation adding items
    var producer = Task.Run(async () =>
    {
        for (int i = 1; i <= 10; i++)
        {
            dataQueue.Enqueue($"Data-{i}");
            Console.WriteLine($"  [Producer] Added Data-{i}");
            await Task.Delay(80); // Simulate async work (API call, file I/O, etc.)
        }
        producerFinished = true;
        Console.WriteLine("  [Producer] Finished");
    });
    
    // Consumer 1: Async operation processing items
    var consumer1 = Task.Run(async () =>
    {
        int processed = 0;
        while (!producerFinished || !dataQueue.IsEmpty)
        {
            if (dataQueue.TryDequeue(out var item)) // Thread-safe dequeue
            {
                await Task.Delay(120); // Simulate async processing
                Console.WriteLine($"  [Consumer1] Processed {item}");
                processed++;
            }
            else
            {
                await Task.Delay(10); // Wait for more items
            }
        }
        Console.WriteLine($"  [Consumer1] Finished ({processed} items)");
    });
    
    // Consumer 2: Another async operation competing for items
    var consumer2 = Task.Run(async () =>
    {
        int processed = 0;
        while (!producerFinished || !dataQueue.IsEmpty)
        {
            if (dataQueue.TryDequeue(out var item)) // Thread-safe dequeue
            {
                await Task.Delay(150); // Simulate async processing
                Console.WriteLine($"  [Consumer2] Processed {item}");
                processed++;
            }
            else
            {
                await Task.Delay(10); // Wait for more items
            }
        }
        Console.WriteLine($"  [Consumer2] Finished ({processed} items)");
    });
    
    // Wait for all operations to complete
    await Task.WhenAll(producer, consumer1, consumer2);
    Console.WriteLine($"\nAll concurrent operations completed. Remaining: {dataQueue.Count} items");
}

static async Task SimulateApiCallAsync(string endpoint)
{
    // Simulate async API call with delay
    await Task.Delay(100);
}

