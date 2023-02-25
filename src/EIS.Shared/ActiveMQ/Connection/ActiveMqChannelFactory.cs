using ActiveMQ.Artemis.Client;

namespace EIS.Shared.ActiveMQ.Connection;

internal sealed class ActiveMqChannelFactory : IActiveMqChannelFactory
{
    private readonly IConnection _connection;
    private readonly ActiveMqChannelAccessor _activeMqChannelAccessor;

    public ActiveMqChannelFactory(IConnection connection, ActiveMqChannelAccessor activeMqChannelAccessor)
    {
        _connection = connection;
        _activeMqChannelAccessor = activeMqChannelAccessor;
    }

    public IConnection Create()
    {
        if (_activeMqChannelAccessor.Channel is not null)
        {
            return _activeMqChannelAccessor.Channel;
        }
        
        var connectionFactory = new ConnectionFactory();
        var endpoint = Endpoint.Create("localhost", 5672, "guest", "guest");
        _activeMqChannelAccessor.Channel = connectionFactory.CreateAsync(endpoint).Result;
        return _connection;
    }
}