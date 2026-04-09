using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Inputs;

public record IConnectMembershipWithUserInput
(
    string UserId, string MembershipId
);
