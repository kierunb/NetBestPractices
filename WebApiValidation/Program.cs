using Scalar.AspNetCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Adds services for using validation attributes
builder.Services.AddValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("docs");
}

app.UseHttpsRedirection();

app.MapPost("/products", (Product product) => TypedResults.Ok(product));

app.Run();

public record Product(
    [Required] string Name,
    [Range(1, 1000)] int Quantity);