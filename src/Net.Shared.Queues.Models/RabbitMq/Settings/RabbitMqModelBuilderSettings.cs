using Net.Shared.Queues.Models.RabbitMq.Domain;

namespace Net.Shared.Queues.Models.RabbitMq.Settings;

public sealed record RabbitMqModelBuilderSettings
{
    public RabbitMqExchange Exchange { get; init; } = null!;
    public RabbitMqQueue Queue { get; init; } = null!;
}