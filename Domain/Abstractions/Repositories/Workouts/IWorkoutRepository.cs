using Domain.Aggregates.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions.Repositories.Workouts;

public interface IWorkoutRepository : IRepositoryBase<Workout, int>
{

}
