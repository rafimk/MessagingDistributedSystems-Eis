using RabbitMQ.Client;

namespace EIS.Shared.RabbitMQ.Connections;

public interface IRabbitMqChannelFactory
{
    IModel Create();
}