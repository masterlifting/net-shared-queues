using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using static Net.Shared.Queues.Models.Constants.Enums.RabbitMq;

namespace Net.Shared.Queues.Models.RabbitMq.Domain;

public class RabbitMqProducerMessage<TPayload> : IMqMessage<TPayload> where TPayload : class
{
    public string Id { get; init; } = null!;
    public IMqQueue Queue { get; init; } = null!;
    public TPayload Payload { get; init; } = null!;
    public IDictionary<string, string> Headers { get; init; } = null!;
    public DateTime DateTime { get; init; } = DateTime.UtcNow;

    public ExchangeNames Exchange { get; set; }

    public string? Version { get; init; }
}