using Net.Shared.Queues.Domain.RabbitMq.Connection;

namespace Net.Shared.Queues.Settings.RabbitMq;

public sealed class RabbitMqSection
{
    public const string Name = "RabbitMq";
    public RabbitMqClient Client { get; set; } = null!;
    public Dictionary<string, RabbitMqConsumerSettings>? Consumers { get; set; }
}