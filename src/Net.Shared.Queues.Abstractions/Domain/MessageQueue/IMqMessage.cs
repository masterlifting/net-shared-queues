namespace Net.Shared.Queues.Abstractions.Domain.MessageQueue;

public interface IMqMessage<TPayload> where TPayload : class
{
    IMqQueue Queue { get; init; }
    TPayload Payload { get; init; }
    IDictionary<string, string> Headers { get; init; }
    DateTime DateTime { get; init; }
    string? Version { get; init; }
}