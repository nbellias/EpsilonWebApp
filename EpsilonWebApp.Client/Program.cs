using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EpsilonWebApp.Client.Services;

// Entry point for the Blazor WebAssembly client application.
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ICustomerServiceClient, CustomerServiceClient>();

await builder.Build().RunAsync();
