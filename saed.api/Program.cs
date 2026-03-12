using Microsoft.EntityFrameworkCore;
using saed.api.Data;
using saed.api.Data.Repositories;
using saed.api.Services;
using saed.api.Services.Universidad;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddHttpClient();
builder.Services.AddScoped<ProcesoRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient<UniversidadApiService>();
builder.Services.AddScoped<ProcesoService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
