

using Domain.Aggregates.Memberships;
using Domain.Aggregates.Workouts;

namespace Domain.Aggregates.Users;

public sealed class User
{

    public string Id { get; private set; } = null!;

    public string UserId { get; private set; } = null!;

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public string? Phonenumber { get; private set; }

    public string? Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public string? ProfileImageUri { get; private set; }

    public int? MembershipId { get; private set; }

    public Membership? Membership { get; private set; }

    public List<Workout>? Workouts { get; private set; }

    private User()
    {

    }

    private User(string id, string userid, DateTimeOffset createdAt)
    {
        Id = id;
        UserId = userid;
        CreatedAt = createdAt;
    }
    
    public static User Create(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId)) 
            throw new ArgumentNullException("Application User id is required");

        var user = new User
            (
                Guid.NewGuid().ToString(),
                userId,
                DateTimeOffset.UtcNow
            );

        return user;
    }

    public static User Create(string id, string userid, string? firstname, string? lastname, string? phonenumber, string? status, DateTimeOffset createAt, string? profileimage, int? membershipid)
    {
        var user = new User(id, userid, createAt)
        {
            FirstName = firstname,
            LastName = lastname,
            Phonenumber = phonenumber,
            Status = status,
            ProfileImageUri = profileimage,
            MembershipId = membershipid
        };

        return user;
    }

    public void UpdateProfile(string firstname, string lastname, string? phonenumber, string? status, string? profileimage, int? membershipid)
    {
        if (!string.IsNullOrWhiteSpace(firstname))
            throw new ArgumentException("First name is required");

        if (!string.IsNullOrWhiteSpace(lastname))
            throw new ArgumentException("Last name is required");

        FirstName = firstname.Trim();
        LastName = lastname.Trim();
        Phonenumber = string.IsNullOrWhiteSpace(phonenumber) ? null : phonenumber;
        Status = string.IsNullOrWhiteSpace(status) ? null : status;
        ProfileImageUri = string.IsNullOrWhiteSpace(profileimage) ? null : profileimage;
        MembershipId = membershipid is null ? null : membershipid.Value;
    }
}
