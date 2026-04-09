

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

    public string? ProfileImageUri { get; private set; }

    public string MembershipId { get; private set; }

    public IEnumerable<string> WorkoutsId { get; private set; }

    private User()
    {

    }

    private User(string id, string userid)
    {
        Id = id;
        UserId = userid;
    }
    
    public static User Create(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId)) 
            throw new ArgumentNullException("Application User id is required");

        var user = new User
            (
                Guid.NewGuid().ToString(),
                userId
            );

        return user;
    }

    public static User Create(string id, string userid, string? firstname, string? lastname, string? phonenumber, string? status, string? profileimage, string membershipid, IEnumerable<string> workoutsId)
    {
        var user = new User(id, userid)
        {
            FirstName = firstname,
            LastName = lastname,
            Phonenumber = phonenumber,
            Status = status,
            ProfileImageUri = profileimage,
            MembershipId = string.IsNullOrWhiteSpace(membershipid) ? string.Empty : membershipid,
            WorkoutsId = workoutsId,
        };

        return user;
    }

    public void UpdateProfile(string firstname, string lastname, string? phonenumber, string? profileimage, string membershipid, IEnumerable<string> workoutsId)
    {
        if (string.IsNullOrWhiteSpace(firstname))
            throw new ArgumentException("First name is required");

        if (string.IsNullOrWhiteSpace(lastname))
            throw new ArgumentException("Last name is required");

        FirstName = firstname.Trim();
        LastName = lastname.Trim();
        Phonenumber = string.IsNullOrWhiteSpace(phonenumber) ? null : phonenumber;
        ProfileImageUri = string.IsNullOrWhiteSpace(profileimage) ? null : profileimage;
        MembershipId = string.IsNullOrWhiteSpace(membershipid) ? string.Empty : membershipid;
        WorkoutsId = workoutsId;
    }
}
