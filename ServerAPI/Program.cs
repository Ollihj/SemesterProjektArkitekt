using ServerAPI.Services;
using ServerAPI.Repositories;
using ServerAPI.Repositories.FileRepo;

var builder = WebApplication.CreateBuilder(args);

// Registrerer MongoDbService så den kan bruges i alle repositories via dependency injection
builder.Services.AddSingleton<MongoDbService>();

// Registrerer repositories så de kan bruges i controllerne via dependency injection
builder.Services.AddSingleton<IBrugerRepository, BrugerRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();
builder.Services.AddSingleton<IProjektRepository, ProjektRepository>();
builder.Services.AddSingleton<IAftaleRepository, AftaleRepository>();
builder.Services.AddSingleton<IBlokeretRepository, BlokeretRepository>();

// Registrerer FileRepository så det kan bruges i FileController via dependency injection
builder.Services.AddSingleton<IFileRepository, MongoFileRepository>();

// Tilføjer support for controllere til applikationen
builder.Services.AddControllers();

// Tilføjer support for automatisk API dokumentation
builder.Services.AddOpenApi();

// Sætter CORS regler op så Blazor frontend må tale med API'et
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Tillader alle origins, metoder og headers
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Aktiverer API dokumentation når vi kører i udviklings-tilstand
    app.MapOpenApi();
}

// Aktiverer CORS reglen vi definerede ovenfor
app.UseCors("AllowAll");

// Aktiverer autorisation (kontrol af hvem der må hvad)
app.UseAuthorization();

// Fortæller applikationen at den skal bruge vores controllere til at håndtere requests
app.MapControllers();

// Starter serveren
app.Run();