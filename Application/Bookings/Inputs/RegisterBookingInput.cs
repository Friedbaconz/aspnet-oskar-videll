using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Bookings.Inputs;

public record RegisterBookingInput
    (
    string userId, string workoutId
    );
