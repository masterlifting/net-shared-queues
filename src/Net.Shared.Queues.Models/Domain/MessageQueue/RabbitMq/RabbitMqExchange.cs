using static Net.Shared.Queues.Models.Constants.Enums.RabbitMq;

namespace Net.Shared.Queues.Models.Domain.MessageQueue.RabbitMq;

public sealed record RabbitMqExchange
{
    public ExchangeTypes Type { get; init; }
    public ExchangeNames Name { get; init; }
    public bool IsDurable { get; init; }
    public bool IsAutoDelete { get; init; }
    public IDictionary<string, object>? Arguments { get; init; }
}