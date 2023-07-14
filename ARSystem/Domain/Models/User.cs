namespace ARSystem.Domain;

public class User : ModelBase
{
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}