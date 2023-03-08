using Net.Shared.Queues.Models.Domain.MessageQueue.RabbitMq;

namespace Net.Shared.Queues.Models.Settings.MessageQueue.RabbitMq;

public sealed record RabbitMqModelBuilderSettings
{
    public RabbitMqExchange Exchange { get; init; } = null!;
    public RabbitMqQueue Queue { get; init; } = null!;
}