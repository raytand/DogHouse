using DogHouse.Api.Data;
using DogHouse.Api.Middleware;
using DogHouse.Api.Repositories;
using DogHouse.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration["AppSettings:Version"] = "Dogshouseservice.Version1.0.1";
builder.Configuration["RateLimit:RequestsPerSecond"] = "10";


builder.Services.AddDbContext<DogHouseContext>(options =>
    options.UseSqlite("Data Source=doghouse.db"));


builder.Services.AddScoped<IDogRepository, DogRepository>();
builder.Services.AddScoped<IDogService, DogService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers().AddJsonOptions(opts =>
{

});

builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DogHouseContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}


app.UseSwagger();
app.UseSwaggerUI();


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseMiddleware<RateLimitMiddleware>();

app.MapControllers();

app.Run();