

namespace Domain.Aggregates.Memberships;

public sealed class Membership
{
    public Membership(Guid id, string name, string description, List<string> benefits, string status, string type, decimal pricing, int monthlyDuration)
    {
        Id = RequiredGuid(id, nameof(Id));
        Name = RequiredString(name, nameof(Name));
        Description = RequiredString(description, nameof(Description));
        Benefits = benefits;
        Status = RequiredString(status, nameof(Status));
        Type = RequiredString(type, nameof(Type));
        Pricing = RequiredValue(Pricing, nameof(Pricing));
        MonthlyDuration = monthlyDuration;
    }

    public Guid Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<string> Benefits {  get; private set; }
    public string Status { get; private set; }
    public string Type {  get; private set; }
    public decimal Pricing { get; private set; }
    public int MonthlyDuration { get; private set; }

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

    private static decimal RequiredValue(decimal value, string propertyName)
    {
        if (value < 0)
            throw new ArgumentException($"{propertyName} must be a non-negative value.", propertyName);
        return value;
    }

    public static Membership Create(Guid id, string name, string description, List<string> benefits, string status, string type, decimal pricing = 0, int monthlyDuration = 20)
    {
        return new Membership(Guid.NewGuid(), name, description, benefits, status, type, pricing, monthlyDuration);
    }

    public static Membership Rehydrate(Guid id, string name, string description, List<string> benefits, string status, string type, decimal pricing = 0, int monthlyDuration= 20)
    {
        return new Membership(id, name, description, benefits, status, type, pricing, monthlyDuration);
    }
    public void Update(string name, string description, List<string> benefits, string status, string type, decimal pricing, int monthlyDuration)
    {
        Name = RequiredString(name, nameof(Name));
        Description = RequiredString(description, nameof(Description));
        Benefits = benefits;
        Type = RequiredString(type, nameof(Type));
        Pricing = RequiredValue(pricing, nameof(Pricing));
        MonthlyDuration = monthlyDuration;
    }
}
