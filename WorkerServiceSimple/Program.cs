using WorkerServiceSimple;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = ".NET Super Service";
});

builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<Worker2>();

var host = builder.Build();
host.Run();
