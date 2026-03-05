using Microsoft.AspNetCore.Http.HttpResults;
using Scalar.AspNetCore;
using System.ComponentModel.Design;
using WebApiTypedResults.ResponseModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("docs");
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapGet("/hello", () => Results.Ok(new ResponseMessage("Hello World!")));
    //.Produces<ResponseMessage>();

app.MapGet("/hello2", () => TypedResults.Ok(new ResponseMessage("Hello World!")));

app.MapGet("/hello3", () =>
{
    var hello = new ResponseMessage("Hello World!");
    return hello != null ? Results.Ok(hello) : Results.NotFound();
});
//.Produces<ResponseMessage>(StatusCodes.Status200OK)
//.Produces(StatusCodes.Status404NotFound);


app.MapGet("/hello4", Results<Ok<ResponseMessage>, NotFound> () =>
{
    var hello = new ResponseMessage("Hello World!");
    return hello != null ? TypedResults.Ok(hello) : TypedResults.NotFound();
});

app.Run();


