using Microsoft.EntityFrameworkCore;
using RedisApp.WebApi.Infrastructure.CrossCutting;
using RedisApp.WebApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoListContext>(options =>
{
    options.UseInMemoryDatabase("ToDoListDB");
});

// IoC
builder.Services.RegisterServices();

// Redis
builder.Services.AddStackExchangeRedisCache(o =>
{
    o.InstanceName = "instance";
    o.Configuration = "localhost:6379";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seeding data for tests
app.Services.SeedData();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
