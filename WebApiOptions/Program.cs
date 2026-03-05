using Microsoft.Extensions.Options;
using WebApiOptions.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

AppSettings appSettings = 
    builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? new AppSettings();

// alternatively, you can register the AppSettings instance directly without using Configure<T>
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

// alternatively, you cahttps://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-10.0#default-app-configuration-sourcesn register the IOptionsMonitor<AppSettings> directly without using Configure<T>
builder.Services.AddSingleton<IOptionsMonitor<AppSettings>, OptionsMonitor<AppSettings>>();

// IOptionsSnapshot is yet another tool in your configuration toolbox.
// It provides a scoped view of configuration settings, ideal for short-lived operations or background tasks

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/options", () => TypedResults.Ok(appSettings));

// This endpoint will not reflect changes to the configuration at runtime, as it captures the AppSettings instance at startup
app.MapGet("/options2", (IOptions<AppSettings> options) => TypedResults.Ok(options.Value));

// This endpoint will reflect changes to the configuration at runtime, if supported by the configuration provider
app.MapGet("/options3", (IOptionsMonitor<AppSettings> optionsMonitor) => TypedResults.Ok(optionsMonitor.CurrentValue));

app.Run();

