using Microsoft.Extensions.Logging;

using Net.Shared.Abstractions.Models.Domain;
using Net.Shared.Extensions;
using Net.Shared.Queues.Abstractions.Interfaces.Core.MessageQueue;
using Net.Shared.Queues.Abstractions.Interfaces.Domain.MessageQueue;
using Net.Shared.Queues.Abstractions.Models.Exceptions;
using Net.Shared.Queues.Abstractions.Models.Settings.MessageQueue;
using Net.Shared.Queues.Abstractions.Models.Settings.MessageQueue.RabbitMq;
using Net.Shared.Queues.RabbitMq.Domain;

namespace Net.Shared.Queues.RabbitMq;

public sealed class RabbitMqProducer : IMqProducer
{
    private readonly string _producerInfo;
    private readonly RabbitMqClient _client;
    private readonly ILogger<RabbitMqProducer> _logger;

    public RabbitMqProducer(ILogger<RabbitMqProducer> logger, RabbitMqClient client)
    {
        _logger = logger;
        _client = client;

        var objectId = GetHashCode();
        _producerInfo = $"RabbitMq producer {objectId}";
    }

    public Task Produce<TMessage, TPayload>(IEnumerable<TMessage> messages, MqProducerSettings settings, CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull
    {
        var producerSettings =
            settings as RabbitMqProducerSettings
            ?? throw new QueuesException($"Configuration '{nameof(RabbitMqProducerSettings)}' was not found.");

        var _messages = messages is IEnumerable<RabbitMqProducerMessage<TPayload>>
            ? (messages as IEnumerable<RabbitMqProducerMessage<TPayload>>)!
            : throw new QueuesException("Messages have incorrected format.");

        _client.PublishMessagesSync(producerSettings, _messages);

        return Task.CompletedTask;
    }
    public async Task<Result<bool>> TryProduce<TMessage, TPayload>(IEnumerable<TMessage> messages, MqProducerSettings settings, CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull
    {
        try
        {
            await Produce<TMessage, TPayload>(messages, settings, cToken);

            return new(true);
        }
        catch (Exception exception)
        {
            return new(new QueuesException(exception));
        }
    }
    public void Dispose()
    {
        _client.Dispose();
        _logger.Debug($"{_producerInfo} was disconnected.");
    }
}
