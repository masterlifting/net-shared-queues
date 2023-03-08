using Microsoft.Extensions.Logging;

using Net.Shared.Queues.Abstractions.Core.MessageQueue;
using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.RabbitMq.Domain;
using Net.Shared.Queues.Models.Settings.MessageQueue;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Net.Shared.Queues.RabbitMq;

public sealed class RabbitMqConsumer : IMqConsumer
{
    private readonly string _consumerInfo;
    private readonly RabbitMqClient _client;
    private readonly ILogger<RabbitMqConsumer_Old> _logger;
    private readonly List<RabbitMqConsumerMessage> _messages;

    public RabbitMqConsumer(ILogger<RabbitMqConsumer_Old> logger, RabbitMqClient client)
    {
        _client = client;
        _logger = logger;

        _messages = new(2000);

        var objectId = GetHashCode();
        _consumerInfo = $"RabbitMq consumer {objectId}";
    }
    public Task Consume<TPayload>(Func<MqConsumerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler, MqConsumerSettings settings, CancellationToken cToken) where TPayload : class
    {
        throw new NotImplementedException();
    }
    public Task<bool> TryConsume<TPayload>(Func<MqConsumerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler, MqConsumerSettings settings, CancellationToken cToken, out string error) where TPayload : class
    {
        throw new NotImplementedException();
    }
    public void Dispose()
    {
        _logger.LogDebug($"{_consumerInfo} was disconnected.");
    }


    private static Dictionary<string, string> GetMessageHeaders(IBasicProperties properties)
    {
        var headers = new Dictionary<string, string>(properties.Headers.Count, StringComparer.OrdinalIgnoreCase);

        foreach (var key in properties.Headers.Keys)
        {
            var value = properties.Headers[key]?.ToString();

            if (string.IsNullOrEmpty(value))
                continue;

            headers.Add(key, value);
        }

        return headers;
    }
    private static RabbitMqConsumerMessage GetMessage(BasicDeliverEventArgs args) => new()
    {
        Payload = Encoding.UTF8.GetString(args.Body.ToArray()),
        Headers = GetMessageHeaders(args.BasicProperties),
        Exchange = args.Exchange,
        RoutingKey = args.RoutingKey,
        ConsumerTag = args.ConsumerTag,
        DeliveryTag = args.DeliveryTag
    };
}
