

using Domain.Aggregates.Memberships;
using Domain.Aggregates.Workouts;

namespace Domain.Aggregates.Users;

public sealed class User
{
    public User(Guid id, string name, string email, string passwordHash, string phonenumber, Membership membership, List<Workout> workout)
    {
        Id = RequiredGuid(id, nameof(Id));
        Name = RequiredString(name, nameof(Name));
        Email = RequiredString(email, nameof(Email));
        PasswordHash = RequiredString(passwordHash, nameof(PasswordHash));
        Phonenumber = RequiredString(phonenumber, nameof(Phonenumber));
        Membership = membership ?? throw new ArgumentNullException(nameof(membership));
        Workouts = workout ?? throw new ArgumentNullException(nameof(workout));
    }

    public Guid Id { get; }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public string Phonenumber { get; private set; }

    public Membership Membership { get; private set; }

    public List<Workout> Workouts { get; private set; }

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
    public static User Create(Guid id, string name, string email, string passwordHash, string phonenumber, Membership membership, List<Workout> workout)
    {
        return new User(Guid.NewGuid(), name, email, phonenumber, passwordHash, membership, workout);
    }

    public static User Rehydrate(Guid id, string name, string email, string passwordHash, string phonenumber, Membership membership, List<Workout> workout)
    {
        return new User(id, name, email, phonenumber, passwordHash, membership, workout);
    }

    public void Update(string name, string email, string passwordHash, string phonenumber, Membership membership, List<Workout> workout)
    {
        Name = RequiredString(name, nameof(Name));
        Email = RequiredString(email, nameof(Email));
        PasswordHash = RequiredString(passwordHash, nameof(PasswordHash));
        Phonenumber = RequiredString(phonenumber, nameof(Phonenumber));
        Membership = membership ?? throw new ArgumentNullException(nameof(membership));
        Workouts = workout ?? throw new ArgumentNullException(nameof(workout));
    }
}
