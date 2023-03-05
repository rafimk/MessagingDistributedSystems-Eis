using EIS.Shared.ActiveMQ;
using EIS.Shared.Dispatchers;
using EIS.Shared.Messaging;
using EIS.Shared.Messaging.Inbox;
using EIS.Shared.Serialization;
using EIS.Shared.Time;
using Microsoft.EntityFrameworkCore;
using TestAPI.Persistence;
using TestAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var connectionString = builder.Configuration["ConnectionString"];

builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IClock, Clock>();

builder.Services
    .AddHttpContextAccessor()
    .AddSerialization()
    .AddMessaging()
    .AddActiveMQ()
    .AddHandlers("TestAPI")
    .AddDispatchers()
    .AddInbox<TestDbContext>(configuration)
    .AddHostedService<WeatherForecastMessagingBackgroundService>()
    .AddControllers();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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