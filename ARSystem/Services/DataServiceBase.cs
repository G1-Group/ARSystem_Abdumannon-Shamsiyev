using System.Collections;
using ARSystem.Domain;
using ARSystem.Domain.Enums;

namespace ARSystem.Services.Infrastructure;

public class DataServiceBase<T> : IEnumerable<T> where T: ModelBase
{
    private readonly DataAccessServiceBase _dataAccessService;
    
    public List<T> Store => this._dataAccessService.Set<T>();

    public DataServiceBase(DataAccessServiceBase dataAccessService)
    {
        _dataAccessService = dataAccessService;
    }
    public IEnumerator<T> GetEnumerator()
    {
        return this._dataAccessService.Set<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    public T Add(T source)
    {
        this.Store.Add(source);
        return source;
    }

    public T Update(T source)
    {
        var model = this.FirstOrDefault(x => x.Id == source.Id);
        
        if (model is null)
        {
            return this.Add(source);
        }

        this.Store.Remove(model);
        this.Store.Add(source);

        return source;
    }

    public T Delete(T source)
    {
        var model = this.FirstOrDefault(x => x.Id == source.Id);
        this.Store.Remove(model);

        return model;
    }
}