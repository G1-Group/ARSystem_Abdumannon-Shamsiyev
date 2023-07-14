using ARSystem.Domain;

namespace ARSystem.Services.Infrastructure;

public class QueuedFileAccessService : IFileAccessService
{
    private readonly string _filePath;
    private readonly Queue<WorkItem?> _workItemsQueue;

    public delegate string OnLoadDataHandler(object sender, EventArgs args);

    public event OnLoadDataHandler OnLoad;
    
    public QueuedFileAccessService(string filePath)
    {
        _filePath = filePath;
        this._workItemsQueue = new Queue<WorkItem?>();
    }
    public Task SaveData(string data)
    {
        // this._workItemsQueue.Enqueue(new WorkItem()
        // {
        //     IsWriteAction = false,
        //     Action = async () => await File.WriteAllTextAsync(this._filePath, data)
        // });
        return Task.CompletedTask;
    }

    public Task<string> LoadData()
    {
        // this._workItemsQueue.Enqueue(new WorkItem()
        // {
        //     IsWriteAction = true,
        //     Action = async () => await File.ReadAllTextAsync(this._filePath)
        // });
        return Task.FromResult(String.Empty);
    }

    public Task Initialize()
    {
        new Thread(async () =>
        {
            while (true)
            {
                await ActionPerformer();
            }
        }).Start();
        return Task.CompletedTask;
    }

    private async Task ActionPerformer()
    {
        if (_workItemsQueue.Count == 0)
            return;
        
        if (_workItemsQueue.TryPeek(out WorkItem? workItem))
        {
            if (workItem is not null)
            {
                try
                {
                    var loadedData = await workItem.Action();
                    if (workItem.IsWriteAction)
                        OnLoad(this, new OnLoadEventArgs() { LoadedData = loadedData });
                    _workItemsQueue.Dequeue();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}

class WorkItem : ModelBase
{
    public Func<Task<string>> Action { get; set; }
    public bool IsWriteAction { get; set; }
}

class OnLoadEventArgs : EventArgs
{
    public string LoadedData { get; set; }
}