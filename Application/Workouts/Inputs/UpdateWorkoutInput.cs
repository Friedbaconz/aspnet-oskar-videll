using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Workouts.Inputs;

public record UpdateWorkoutInput
(
    string Id,
    string Name,
    string Category,
    string Instructions,
    DateTime Date,
    TimeSpan Time,
    List<string> Users
);
