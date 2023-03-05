namespace Net.Shared.Queues.Models.RabbitMq.Settings;

public sealed record RabbitMqSection
{
    public const string Name = "RabbitMq";
    public RabbitMqClientSettings Client { get; init; } = null!;
    public Dictionary<string, RabbitMqConsumerSettings>? Consumers { get; init; }
}