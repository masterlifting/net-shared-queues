namespace Net.Shared.Queues.Models.RabbitMq.Settings;

public sealed record RabbitMqClientSettings
{
    public RabbitMqConnectionSettings Connection { get; init; } = null!;
    public RabbitMqModelBuilderSettings[] ModelBuilders { get; init; } = Array.Empty<RabbitMqModelBuilderSettings>();
}