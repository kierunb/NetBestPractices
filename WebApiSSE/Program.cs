using WebApiSSE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<FoodService>();

var app = builder.Build();

app.UseDefaultFiles().UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/orders", (FoodService foods, CancellationToken token) =>
    TypedResults.ServerSentEvents(
        foods.GetCurrent(token),
        eventType: "order")
);

app.Run();


