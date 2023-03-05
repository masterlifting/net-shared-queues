using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.Settings.Core;

namespace Net.Shared.Queues.Abstractions.Core.MessageQueue;

public interface IMqProducer : IDisposable
{
    Task Publish<TPayload>(
        Func<MqProducerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler,
        MqProducerSettings settings,
        CancellationToken cToken)
        where TPayload : class;
    Task<bool> TryPublish<TPayload>(
        Func<MqProducerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler,
        MqProducerSettings settings,
        CancellationToken cToken,
        out string error)
        where TPayload : class;
}