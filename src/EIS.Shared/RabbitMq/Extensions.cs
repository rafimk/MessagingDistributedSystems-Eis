using EIS.Shared.Accessors;
using EIS.Shared.Messaging;
using EIS.Shared.RabbitMQ.Connections;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace EIS.Shared.RabbitMQ;

public static class Extensions
{
    public static IServiceCollection AddRabbit(this IServiceCollection services, Action<IMessagingConfiguration> configure = default)
    {
        var factory = new ConnectionFactory {HostName = "localhost"};
        var connection = factory.CreateConnection();

        services.AddSingleton(connection);
        services.AddSingleton<RabbitMqChannelAccessor>();
        services.AddSingleton<IRabbitMqChannelFactory, RabbitMqChannelFactory>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IMessageSubscriber, MessageSubscriber>();
        services.AddSingleton<IMessageIdAccessor, MessageIdAccessor>();
        
        configure?.Invoke(new MessagingConfiguration(services));
        return services;
    }
}