using System.Collections.Concurrent;
using Net.Shared.Queues.Abstractions.Core.WorkQueue;

namespace Net.Shared.Queues.WorkQueue;
/// <summary>
///  Asynchronously processing functions in the concurrent queue
/// </summary>
/// <remarks>Disposable</remarks>
public sealed class WorkQueue : IWorkQueue
{
    private record WorkQueueItem(Func<Task> Func, TaskCompletionSource TaskCompletionSource);
    private readonly BlockingCollection<WorkQueueItem> _queueItems;

    public WorkQueue()
    {
        _queueItems = new();
        Task.Run(ProcessQueueItems);
    }
    public WorkQueue(int itemsCount)
    {
        _queueItems = new(itemsCount);
        Task.Run(ProcessQueueItems);
    }

    public Task Process(Func<Task> func)
    {
        TaskCompletionSource tcs = new();
        while (!_queueItems.TryAdd(new(func, tcs))) ;
        return tcs.Task;
    }
    public Task Process(Func<Task>[] funcs)
    {
        List<Task> results = new(funcs.Length);

        foreach (var func in funcs)
        {
            TaskCompletionSource tcs = new();

            while (!_queueItems.TryAdd(new(func, tcs)))
                results.Add(tcs.Task);
        }

        return Task.WhenAll(results);
    }

    public void Dispose() => _queueItems.Dispose();

    private async Task ProcessQueueItems()
    {
        foreach (var item in _queueItems.GetConsumingEnumerable())
        {
            try
            {
                await item.Func.Invoke();
                item.TaskCompletionSource.SetResult();
            }
            catch (Exception exeption)
            {
                item.TaskCompletionSource.SetException(exeption);
            }
        }
    }
}
