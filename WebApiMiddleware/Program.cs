using System.Globalization;
using WebApiMiddleware.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Custom Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine("Work that can write to the response. (1)");
    await next.Invoke(context);
    Console.WriteLine("Work that doesn't write to the response. (1)");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Work that can write to the response. (2)");
    await next.Invoke(context);
    Console.WriteLine("Work that doesn't write to the response. (2)");
});

app.UseRequestCulture();

// /?branch=main	Branch used = 'main'
app.MapWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch);

app.MapGet("/", () => "Hello World!");

app.Run();


static void HandleBranch(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        var branchVer = context.Request.Query["branch"];
        Console.WriteLine($"Branch used = '{branchVer}'");
        await context.Response.WriteAsync($"Branch used = '{branchVer}'");
    });
}