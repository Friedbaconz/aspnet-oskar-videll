
using Domain.Aggregates.Users;
namespace Domain.Aggregates.Memberships;

public sealed class Membership
{
    public Membership(int id, string name, string description, List<MembershipBenefits> benefits, string status, string type, decimal pricing, int monthlyDuration, List<User> users)
    {
        Id = RequiredInt(id, nameof(Id));
        Name = RequiredString(name, nameof(Name));
        Description = RequiredString(description, nameof(Description));
        Benefits = benefits ?? throw new ArgumentNullException(nameof(benefits));
        Status = RequiredString(status, nameof(Status));
        Type = RequiredString(type, nameof(Type));
        Pricing = RequiredValue(Pricing, nameof(Pricing));
        MonthlyDuration = monthlyDuration;
        Users = users ?? throw new ArgumentNullException(nameof(users));
    }

    public int Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<MembershipBenefits> Benefits {  get; private set; }
    public string Status { get; private set; }
    public string Type {  get; private set; }
    public decimal Pricing { get; private set; }
    public int MonthlyDuration { get; private set; }
    public List<User> Users { get; private set; }

    private static Guid RequiredGuid(Guid value, string propertyName)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"{propertyName} is required.", propertyName);
        return value;
    }
    private static string RequiredString(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{propertyName} is required.", propertyName);

        return value.Trim();
    }

    private static int RequiredInt(int value, string propertyName)
    {
        if (value <= 0)
            throw new ArgumentException($"{propertyName} must be a positive integer.", propertyName);
        return value;
    }

    private static decimal RequiredValue(decimal value, string propertyName)
    {
        if (value < 0)
            throw new ArgumentException($"{propertyName} must be a non-negative value.", propertyName);
        return value;
    }

    public static Membership Create(int id, string name, string description, List<MembershipBenefits> benefits, string status, string type, decimal pricing, int monthlyDuration, List<User> users)
    {
        return new Membership(id, name, description, benefits, status, type, pricing, monthlyDuration, users);
    }

    public static Membership Rehydrate(int id, string name, string description, List<MembershipBenefits> benefits, string status, string type, decimal pricing, int monthlyDuration, List<User> users)
    {
        return new Membership(id, name, description, benefits, status, type, pricing, monthlyDuration, users);
    }
    public void Update(string name, string description, List<MembershipBenefits> benefits, string status, string type, decimal pricing, int monthlyDuration, List<User> users)
    {
        Name = RequiredString(name, nameof(Name));
        Description = RequiredString(description, nameof(Description));
        Benefits = benefits;
        Type = RequiredString(type, nameof(Type));
        Pricing = RequiredValue(pricing, nameof(Pricing));
        MonthlyDuration = monthlyDuration;
        Users = Users;
    }
}
