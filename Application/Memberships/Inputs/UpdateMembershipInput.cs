using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Inputs
{
    public record UpdateMembershipInput
    (
        string id, string name, string? description, IEnumerable<UpdateMembershipBenefitInput> benefits, string status, string type, decimal pricing, int monthlyDuration, string userid, IEnumerable<string> users
    );
}
