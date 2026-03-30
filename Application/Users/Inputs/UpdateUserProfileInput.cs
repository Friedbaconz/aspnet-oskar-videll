

using Domain.Aggregates.Memberships;
using Domain.Aggregates.Workouts;

namespace Application.Users.Inputs;

public record UpdateUserProfileInput 
(
    string Id, string UserId, string Firstname, string Lastname, string Email, string Password, string Phonenumber, string Status, DateTimeOffset CreatedAt, string ProfileImageUri, int MembershipId
);
