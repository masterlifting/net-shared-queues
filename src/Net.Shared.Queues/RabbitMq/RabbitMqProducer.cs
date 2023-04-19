﻿using Microsoft.Extensions.Logging;

using Net.Shared.Models.Domain;
using Net.Shared.Queues.Abstractions.Core.MessageQueue;
using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.Exceptions;
using Net.Shared.Queues.Models.RabbitMq.Domain;
using Net.Shared.Queues.Models.Settings.MessageQueue;
using Net.Shared.Queues.Models.Settings.MessageQueue.RabbitMq;

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
            return new(exception);
        }
    }
    public void Dispose()
    {
        _client.Dispose();
        _logger.LogDebug($"{_producerInfo} was disconnected.");
    }
}