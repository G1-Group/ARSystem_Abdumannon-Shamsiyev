using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ARSystem.Domain;
using ARSystem.Domain.Enums;

namespace ARSystem.Services.Infrastructure;

public abstract class DataAccessServiceBase
{
    [JsonIgnore]
    public IFileAccessService FileAccessService { get; set; }

    [JsonIgnore]
    private Dictionary<Type, List<ModelBase>> DataModelsStore { get; set; }

    public DataAccessServiceBase(IFileAccessService fileAccessService)
    {
        FileAccessService = fileAccessService;
        this.DataModelsStore = new Dictionary<Type, List<ModelBase>>();
    }

    public void RegisterDataModels<T>() where T: ModelBase
    {
        this.DataModelsStore.Add(typeof(T), new List<ModelBase>(new List<T>()));
    }

    public List<T> Set<T>() where T: ModelBase
    {
        var type = this.GetType();
        var storePropertyInfo = type.GetProperties().FirstOrDefault(x => x.PropertyType == typeof(List<T>));

        if (storePropertyInfo is null)
            throw new Exception("Store not found");
        
        var store = (List<T>)storePropertyInfo.GetValue(this);
        
        return store;
    }

    public async Task SaveStore()
    {
        var jsonStoreContent = JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        });

        await this.FileAccessService.SaveData(jsonStoreContent);
    }

    public async Task LoadStore()
    {
        var storeJsonContent = await this.FileAccessService.LoadData();
        if (string.IsNullOrEmpty(storeJsonContent))
            return;
        
        var loadedDataAccessService = JsonSerializer.Deserialize(storeJsonContent, this.GetType());

        if (loadedDataAccessService is null)
            return;

        
        var storeDatas = loadedDataAccessService.GetType().GetProperties().Where(x =>
            x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(List<>));
        
        foreach (var propertyInfo in storeDatas)
        {
            this.GetType().GetProperty(propertyInfo.Name)?.SetValue(this, propertyInfo.GetValue(loadedDataAccessService));    
        }

    }
    
    
    
}