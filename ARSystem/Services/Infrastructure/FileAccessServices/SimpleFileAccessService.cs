namespace ARSystem.Services.Infrastructure;

public class SimpleFileAccessService : IFileAccessService
{
    private readonly string _filePath;

    public SimpleFileAccessService(string filePath)
    {
        _filePath = filePath;
    }
    
    public async Task SaveData(string data)
    {
        await File.WriteAllTextAsync(this._filePath, data);
    }

    public async Task<string> LoadData()
    {
        return await File.ReadAllTextAsync(this._filePath);
    }

    public async Task Initialize()
    {
        var directoryPath = Path.GetDirectoryName(this._filePath);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        if (!File.Exists(this._filePath))
        {
            File.Create(this._filePath).Close();
        }
    }
}