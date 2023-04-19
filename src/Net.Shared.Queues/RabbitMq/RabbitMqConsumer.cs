using Microsoft.Extensions.Logging;

using Net.Shared.Models.Domain;
using Net.Shared.Queues.Abstractions.Core.MessageQueue;
using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.Exceptions;
using Net.Shared.Queues.Models.RabbitMq.Domain;
using Net.Shared.Queues.Models.Settings.MessageQueue;
using Net.Shared.Queues.Models.Settings.MessageQueue.RabbitMq;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;

namespace Net.Shared.Queues.RabbitMq;

public sealed class RabbitMqConsumer : IMqConsumer
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly string _consumerInfo;
    private readonly RabbitMqClient _client;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly List<RabbitMqConsumerMessage> _messages;

    public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, RabbitMqClient client)
    {
        _client = client;
        _logger = logger;

        _messages = new(2000);

        var objectId = GetHashCode();
        _consumerInfo = $"RabbitMq consumer {objectId}";
    }

    public Task Consume<TMessage, TPayload>(Func<MqConsumerSettings, IEnumerable<TMessage>, CancellationToken, Task> handler, MqConsumerSettings settings, CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull
    {
        var consumerSettings =
            settings as RabbitMqConsumerSettings
            ?? throw new QueuesException($"Configuration '{nameof(RabbitMqConsumerSettings)}' was not found.");

        _client.RegisterConsumerSync(consumerSettings, OnReceived);

        async Task OnReceived(object? _, BasicDeliverEventArgs args)
        {
            var message = GetMessage(args);

            await _semaphore.WaitAsync(cToken);
            _messages.Add(message);

            if (_messages.Count == settings.Limit)
                await Invoke<TMessage, TPayload>(handler, settings, cToken);

            _semaphore.Release();
        }

        return Task.CompletedTask;
    }
    public async Task<Result<bool>> TryConsume<TMessage, TPayload>(Func<MqConsumerSettings, IEnumerable<TMessage>, CancellationToken, Task> handler, MqConsumerSettings settings, CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull
    {
        try
        {
            await Consume<TMessage, TPayload>(handler, settings, cToken);

            return new(true);
        }
        catch (Exception exception)
        {
            return new(exception);
        }
    }
    public void Dispose()
    {
        _client.Dispose();
        _logger.LogDebug($"{_consumerInfo} was disconnected.");
    }

    private Task Invoke<TMessage, TPayload>(Func<MqConsumerSettings, IEnumerable<TMessage>, CancellationToken, Task> handler, MqConsumerSettings settings, CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull
    {
        try
        {
            return handler.Invoke(settings, (IEnumerable<TMessage>)_messages, cToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(new QueuesException(exception));
        }
        finally
        {
            _messages.Clear();
        }

        return Task.CompletedTask;
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
