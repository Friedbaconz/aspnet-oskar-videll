

using Domain.Aggregates.Memberships;
using Domain.Aggregates.Workouts;

namespace Domain.Aggregates.Users;

public sealed class User
{
    public User(string id, Guid userid, string firstname, string lastname,string email, string passwordHash, string phonenumber, string status, string profileimage, int membershipid, Membership membership, List<Workout> workout)
    {
        Id = RequiredString(id, nameof(Id));
        UserId = RequiredGuid(userid, nameof(UserId));
        FirstName = RequiredString(firstname, nameof(FirstName));
        LastName = RequiredString(lastname, nameof(LastName));
        Email = RequiredString(email, nameof(Email));
        PasswordHash = RequiredString(passwordHash, nameof(PasswordHash));
        Phonenumber = RequiredString(phonenumber, nameof(Phonenumber));
        Status = RequiredString(status, nameof(Status));
        ProfileImageUri = RequiredString(profileimage, nameof(ProfileImageUri));
        MembershipId = membershipid;
        Membership = membership;
        Workouts = workout;
    }

    public string Id { get; }

    public Guid UserId { get; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public string? Phonenumber { get; private set; }

    public string Status { get; private set; }

    public string ProfileImageUri { get; private set; }

    public int MembershipId { get; private set; }

    public Membership? Membership { get; private set; }

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
    public static User Create(string id, Guid userid, string firstname, string lastname, string email, string passwordHash, string phonenumber, string status, string profileimage, int membershipid,Membership membership, List<Workout> workout)
    {
        return new User(id, Guid.NewGuid(), firstname, lastname, email, passwordHash, phonenumber, status, profileimage, membershipid ,membership, workout);
    }

    public static User Rehydrate(string id, Guid userid, string firstname, string lastname, string email, string passwordHash, string phonenumber, string status, string profileimage, int membershipid,Membership membership, List<Workout> workout)
    {
        return new User(id, userid, firstname, lastname, email, passwordHash, phonenumber, status, profileimage, membershipid, membership, workout);
    }

    public void UpdateProfile(string id, Guid userid, string firstname, string lastname, string email, string passwordHash, string phonenumber, string status, string profileimage, int membershipid, Membership membership, List<Workout> workout)
    {
        FirstName = RequiredString(firstname, nameof(FirstName));
        LastName = RequiredString(lastname, nameof(LastName));
        Email = RequiredString(email, nameof(Email));
        PasswordHash = RequiredString(passwordHash, nameof(PasswordHash));
        Phonenumber = RequiredString(phonenumber, nameof(Phonenumber));
        Status = RequiredString(status, nameof(Status));
        ProfileImageUri = RequiredString(profileimage, nameof(ProfileImageUri));
        MembershipId = membershipid;
        Membership = membership ?? throw new ArgumentNullException(nameof(membership));
        Workouts = workout ?? throw new ArgumentNullException(nameof(workout));
    }
}
