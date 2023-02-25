using RabbitMQ.Client;

namespace EIS.Shared.RabbitMQ.Connections;

internal sealed class RabbitMqChannelFactory : IRabbitMqChannelFactory
{
    private readonly IConnection _connection;
    private readonly RabbitMqChannelAccessor _rabbitMqChannelAccessor;

    public RabbitMqChannelFactory(IConnection connection, RabbitMqChannelAccessor rabbitMqChannelAccessor)
    {
        _connection = connection;
        _rabbitMqChannelAccessor = rabbitMqChannelAccessor;
    }

    public IModel Create()
        => _rabbitMqChannelAccessor.Channel ?? (_rabbitMqChannelAccessor.Channel = _connection.CreateModel());
}