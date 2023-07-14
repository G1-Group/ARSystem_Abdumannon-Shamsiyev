using ARSystem.Domain;

namespace ARSystem.Services.Infrastructure;

public class SimpleDataAccessService : DataAccessServiceBase
{
    public List<User> Users { get; set; } = new List<User>();
    public SimpleDataAccessService(IFileAccessService fileAccessService) : base(fileAccessService)
    {
    }
}