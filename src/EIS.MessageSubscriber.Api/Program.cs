using EIS.MessageSubscriber.Api.Services;
using EIS.Shared.ActiveMQ;
using EIS.Shared.Messaging;
using EIS.Shared.Observability;
using EIS.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddHttpContextAccessor()
    .AddSerialization()
    .AddMessaging()
    .AddActiveMQ()
    .AddHostedService<WeatherForecastMessagingBackgroundService>()
    .AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCorrelationId();

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