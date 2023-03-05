using Net.Shared.Exceptions;

namespace Net.Shared.Queues.Models.Exceptions;

public sealed class NetSharedQueueException : NetSharedException
{
    public NetSharedQueueException(string message) : base(message)
    {
    }

    public NetSharedQueueException(Exception exception) : base(exception)
    {
    }
}
