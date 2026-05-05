using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ArchitektApp;
using ArchitektApp.Pages;
using ArchitektApp.Service;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Config.ServerUrl) });
builder.Services.AddScoped<IBrugerService, BrugerServiceHttp>();
builder.Services.AddScoped<IBookingService, BookingServiceHttp>();
builder.Services.AddScoped<IAftaleService, AftaleServiceHttp>();
builder.Services.AddScoped<IBlokeretService, BlokeretServiceHttp>();
builder.Services.AddScoped<IProjektService, ProjektServiceHttp>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
