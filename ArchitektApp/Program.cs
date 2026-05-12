using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ArchitektApp;
using ArchitektApp.Service;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<LoginService>();

// Registrerer FileService så den kan bruges i Blazor-komponenter via dependency injection
builder.Services.AddScoped<FileService>();

await builder.Build().RunAsync();
