using ActiveMQ.Artemis.Client;
using EIS.Shared.Accessors;
using EIS.Shared.ActiveMQ.Connection;
using EIS.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace EIS.Shared.ActiveMQ;

public static class Extensions
{
    public static IServiceCollection AddActiveMQ(this IServiceCollection services, Action<IMessagingConfiguration> configure = default)
    {
        var connectionFactory = new ConnectionFactory();
        var endpoint = Endpoint.Create("localhost", 5672, "guest", "guest");
        var connection = connectionFactory.CreateAsync(endpoint).Result;

        services.AddSingleton(connection);
        services.AddSingleton<ActiveMqChannelAccessor>();
        services.AddSingleton<IActiveMqChannelFactory, ActiveMqChannelFactory>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IMessageSubscriber, MessageSubscriber>();
        services.AddSingleton<IMessageIdAccessor, MessageIdAccessor>();
        
        configure?.Invoke(new MessagingConfiguration(services));
        return services;
    }
}