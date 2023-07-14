using ARSystem.Domain;
using ARSystem.Services;
using ARSystem.Services.Infrastructure;

var simpleFileAccessService = new SimpleFileAccessService("./data/store.json");
var dataAccessService = new SimpleDataAccessService(simpleFileAccessService);


UserService userService = new UserService(dataAccessService);

// userService.Add(new User()
// {
//     Id = Guid.NewGuid(),
//     Password = "234",
//     FirstName = "Abdurahim",
//     PhoneNumber = "+9989"
// });


// Console.WriteLine(userService.First().State);

// await dataAccessService.SaveStore();
await dataAccessService.LoadStore();


Console.WriteLine(userService.ElementAt(1).FirstName);