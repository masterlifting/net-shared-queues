using Net.Shared.Background.Models.Settings;

namespace Net.Shared.Queues.Models.Settings.Core;

public abstract record MqSettings
{
    public string Queue { get; init; } = null!;
    public uint Limit { get; init; } = 1_000;
    public TaskSchedulerSettings Scheduler { get; init; } = new();
}