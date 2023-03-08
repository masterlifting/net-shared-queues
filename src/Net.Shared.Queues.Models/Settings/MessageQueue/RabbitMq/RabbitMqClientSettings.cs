namespace Net.Shared.Queues.Models.Settings.MessageQueue.RabbitMq;

public sealed record RabbitMqClientSettings
{
    public RabbitMqConnectionSettings Connection { get; init; } = null!;
    public RabbitMqModelBuilderSettings[] ModelBuilders { get; init; } = Array.Empty<RabbitMqModelBuilderSettings>();
}