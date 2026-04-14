using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Inputs;

public record RegisterMemebershipInput
    (
        string name, 
        string? description,
        List<RegisterBenfitsInput> benefits,
        string status, 
        string type, 
        decimal pricing, 
        int monthlyDuration
    );
