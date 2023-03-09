using Net.Shared.Models.Domain;
using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.Settings.MessageQueue;

namespace Net.Shared.Queues.Abstractions.Core.MessageQueue;

public interface IMqConsumer : IDisposable
{
    Task Consume<TMessage, TPayload>(
        Func<MqConsumerSettings, IEnumerable<TMessage>, CancellationToken, Task> handler,
        MqConsumerSettings settings,
        CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull;
    Task<TryResult<bool>> TryConsume<TMessage, TPayload>(
        Func<MqConsumerSettings, IEnumerable<TMessage>, CancellationToken, Task> handler,
        MqConsumerSettings settings,
        CancellationToken cToken)
        where TMessage : class, IMqMessage<TPayload>
        where TPayload : notnull;
}