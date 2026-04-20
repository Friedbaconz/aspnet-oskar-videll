using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Workouts.Inputs;

public record RegisterWorkoutInput
    (
        string Name,
        string Category,
        string Instructions,
        DateTime Date,
        TimeSpan Time
    );
