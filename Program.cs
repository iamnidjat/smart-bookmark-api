using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.Repositories;
using SmartBookmarkApi.Services.Implementations;
using SmartBookmarkApi.Services.Interfaces;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
//builder.Services.AddControllers().AddJsonOptions(x =>
//                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(connectionString);
});

// Specific Services
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Generic Services
builder.Services.AddScoped(typeof(ICRUDBaseService<>), typeof(CRUDBaseService<>));

// Repositories
builder.Services.AddScoped<IBookmarkRepository, BookmarkRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
