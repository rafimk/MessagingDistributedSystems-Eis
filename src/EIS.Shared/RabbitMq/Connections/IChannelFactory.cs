using RabbitMQ.Client;

namespace EIS.Shared.RabbitMQ.Connections;

public interface IChannelFactory
{
    IModel Create();
}