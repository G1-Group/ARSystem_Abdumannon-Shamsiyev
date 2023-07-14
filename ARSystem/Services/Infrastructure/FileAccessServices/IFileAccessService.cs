namespace ARSystem.Services.Infrastructure;

public interface IFileAccessService
{
    public Task SaveData(string data);
    public Task<string> LoadData();
    public Task Initialize();
}