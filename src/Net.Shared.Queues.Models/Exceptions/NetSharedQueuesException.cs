using Net.Shared.Exceptions;

namespace Net.Shared.Queues.Models.Exceptions;

public sealed class NetSharedQueuesException : NetSharedException
{
    public NetSharedQueuesException(string message) : base(message)
    {
    }

    public NetSharedQueuesException(Exception exception) : base(exception)
    {
    }
}