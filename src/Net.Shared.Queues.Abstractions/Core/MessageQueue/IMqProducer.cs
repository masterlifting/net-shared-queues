using Net.Shared.Models.Domain;
using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.Settings.MessageQueue;

namespace Net.Shared.Queues.Abstractions.Core.MessageQueue;

public interface IMqProducer : IDisposable
{
    Task Produce<TMessage, TPayload>(IEnumerable<TMessage> messages, MqProducerSettings settings, CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull;
    Task<Result<bool>> TryProduce<TMessage, TPayload>(IEnumerable<TMessage> messages, MqProducerSettings settings, CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull;
}