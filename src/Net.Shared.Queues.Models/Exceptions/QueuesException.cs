namespace Net.Shared.Queues.Models.Exceptions;

public sealed class QueuesException : NetSharedException
{
    public QueuesException(string message) : base(message)
    {
    }

    public QueuesException(Exception exception) : base(exception)
    {
    }
}