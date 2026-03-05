using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using WebApiProblemDetails.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adds services for using Problem Details format
builder.Services.AddProblemDetails();

//builder.Services.AddProblemDetails(options =>
//{
//    options.CustomizeProblemDetails = context =>
//    {
//        context.ProblemDetails.Instance =
//            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

//        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

//        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
//        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
//    };
//});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Converts unhandled exceptions into Problem Details responses
app.UseExceptionHandler();

//app.UseExceptionHandler(new ExceptionHandlerOptions
//{
//    StatusCodeSelector = ex => ex switch
//    {
//        ArgumentException => StatusCodes.Status400BadRequest,
//        _ => StatusCodes.Status500InternalServerError
//    }
//});

app.UseHttpsRedirection();

// Returns the Problem Details response for (empty) non-successful responses
app.UseStatusCodePages();


app.MapGet("/", () => "Hello World!");

app.MapGet("/throw", () =>
{
    throw new Exception("This is a test exception.");
});

app.Run();

