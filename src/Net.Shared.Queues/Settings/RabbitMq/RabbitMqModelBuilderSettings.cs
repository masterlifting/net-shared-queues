using Net.Shared.Queues.Domain.RabbitMq;

namespace Net.Shared.Queues.Settings.RabbitMq;

public sealed class RabbitMqModelBuilderSettings
{
    public RabbitMqExchange Exchange { get; set; } = null!;
    public RabbitMqQueue Queue { get; set; } = null!;
}