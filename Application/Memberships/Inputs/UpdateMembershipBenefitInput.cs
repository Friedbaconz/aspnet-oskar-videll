using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Inputs;

public record UpdateMembershipBenefitInput
    (
        int id,
        string benefit,
        string membershipId
    );
