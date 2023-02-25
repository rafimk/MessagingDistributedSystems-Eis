using ActiveMQ.Artemis.Client;

namespace EIS.Shared.ActiveMQ.Connection;

internal sealed class ActiveMqChannelAccessor
{
    private static readonly ThreadLocal<ConnectionHolder> Holder = new() ;

    public IConnection? Channel
    {
        get => Holder.Value?.Context;
        set
        {
            var holder = Holder.Value;
            if (holder is not null)
            {
                holder.Context = null;
            }

            if (value is not null)
            {
                Holder.Value = new ConnectionHolder { Context = value };
            }
        }
    }

    private class ConnectionHolder
    {
        public IConnection? Context;
    }
}