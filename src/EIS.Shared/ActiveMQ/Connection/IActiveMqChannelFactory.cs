using ActiveMQ.Artemis.Client;

namespace EIS.Shared.ActiveMQ.Connection;

public interface IActiveMqChannelFactory
{
    IConnection Create();
}