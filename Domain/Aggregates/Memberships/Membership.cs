
using Domain.Aggregates.Users;
namespace Domain.Aggregates.Memberships;

public sealed class Membership
{

    public string Id { get; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public IEnumerable<MembershipBenefits> Benefits {  get; private set; }
    public string Status { get; private set; }
    public string Type {  get; private set; }
    public decimal Pricing { get; private set; }
    public int MonthlyDuration { get; private set; }
    public string Userid { get; private set; }
    public IEnumerable<string> Users { get; private set; }

    private Membership()
    {

    }

    private Membership(string id)
    {
        Id = id;
    }

    public static Membership Create ()
    {
        var membership = new Membership
        (
            Guid.NewGuid().ToString()
        );

        return membership;
    }

    public static Membership Create(string id, string name, string? description, IEnumerable<MembershipBenefits> benefits, string status, string type, decimal pricing, int monthlyDuration, IEnumerable<string> users)
    {
        var membership = new Membership(id)
        {
            Name = name,
            Description = description,
            Benefits = benefits,
            Status = status,
            Type = type,
            Pricing = pricing,
            MonthlyDuration = monthlyDuration,
            Users = users

        };

        return membership;
    }

    public void Update(string name, string? description, IEnumerable<MembershipBenefits> benefits,string status, string type, decimal pricing, int monthlyDuration, string userid,IEnumerable<string> users)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("First name is required");

        Name = name.Trim();
        Description = description;
        Benefits = benefits;
        Type = type;
        Pricing = pricing;
        MonthlyDuration = monthlyDuration;
        Userid = userid;
        Users = users;
    }
}
