using Net.Shared.Queues.Models.Settings.Core;

namespace Net.Shared.Queues.Models.RabbitMq.Settings;

public sealed record RabbitMqConsumerSettings : MqConsumerSettings
{
    public bool IsExclusiveQueue { get; set; }
    public bool IsAutoAck { get; set; }
    public string? ConsumerTag { get; set; }
    public bool IsNoLocal { get; set; }

    public IDictionary<string, object>? Arguments { get; set; }
}