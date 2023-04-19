namespace Net.Shared.Queues.Models.Exceptions;

public sealed class QueuesException : Net.Shared.Exception
{
    public QueuesException(string message) : base(message)
    {
    }

    public QueuesException(System.Exception exception) : base(exception)
    {
    }
}