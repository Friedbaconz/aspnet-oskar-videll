using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Bookings.Inputs;

public record UpdateBookingInput
    (
    string Id, string UserId, string WorkoutId
    );
