namespace Net.Shared.Queues.Abstractions.Domain.MessageQueue;

public interface IMqQueue
{
    string Name { get; init; }
}