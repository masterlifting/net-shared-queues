using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.Settings.MessageQueue;

namespace Net.Shared.Queues.Abstractions.Core.MessageQueue;

public interface IMqConsumer : IDisposable
{
    Task Consume<TPayload>(
        Func<MqConsumerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler,
        MqConsumerSettings settings,
        CancellationToken cToken)
        where TPayload : class;
    Task<bool> TryConsume<TPayload>(
        Func<MqConsumerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler,
        MqConsumerSettings settings,
        CancellationToken cToken,
        out string error)
        where TPayload : class;
}