using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EpsilonWebApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<CustomerServiceClient>();

await builder.Build().RunAsync();
