using MongoDB.Bson.Serialization.Conventions;
using ServerAPI.Repositories;

var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
ConventionRegistry.Register("IgnoreExtraElements", conventionPack, _ => true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IBrugerRepository, BrugerRepositoryMongoDB>();
builder.Services.AddSingleton<IBookingRepository, BookingRepositoryMongoDB>();
builder.Services.AddSingleton<IAftaleRepository, AftaleRepositoryMongoDB>();
builder.Services.AddSingleton<IBlokeretRepository, BlokeretRepositoryMongoDB>();
builder.Services.AddSingleton<IProjektRepository, ProjektRepositoryMongoDB>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
