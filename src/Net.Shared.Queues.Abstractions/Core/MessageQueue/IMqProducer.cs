using Net.Shared.Queues.Abstractions.Domain.MessageQueue;
using Net.Shared.Queues.Models.Settings.MessageQueue;

namespace Net.Shared.Queues.Abstractions.Core.MessageQueue;

public interface IMqProducer : IDisposable
{
    Task Produce<TPayload>(
        Func<MqProducerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler,
        MqProducerSettings settings,
        CancellationToken cToken)
        where TPayload : class;
    Task<bool> TryProduce<TPayload>(
        Func<MqProducerSettings, IReadOnlyCollection<IMqMessage<TPayload>>, CancellationToken, Task> handler,
        MqProducerSettings settings,
        CancellationToken cToken,
        out string error)
        where TPayload : class;
}