using Net.Shared.Queues.Abstractions.Settings;

namespace Net.Shared.Queues.Abstractions.Domain;

public interface IMqProducerMessage<TPayload> : IMqMessage<TPayload> where TPayload : class
{
    IMqQueue Queue { get; set; }
    IMqProducerMessageSettings? Settings { get; set; }
    string Version { get; set; }
}