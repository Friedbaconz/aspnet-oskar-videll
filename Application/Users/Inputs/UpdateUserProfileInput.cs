

using Domain.Aggregates.Memberships;
using Domain.Aggregates.Workouts;

namespace Application.Users.Inputs;

public record UpdateUserProfileInput 
(
    string UserId, string Firstname, string Lastname, string Phonenumber, string ProfileImageUri
);
