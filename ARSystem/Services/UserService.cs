using ARSystem.Domain;
using ARSystem.Services.Infrastructure;

namespace ARSystem.Services;

public class UserService : DataServiceBase<User>
{
    public UserService(DataAccessServiceBase dataAccessService) : base(dataAccessService)
    {
    }
}