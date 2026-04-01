using Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;
using Infrastructure.Persistence.Entities.Workouts;
using Infrastructure.Persistence.Entities.Users;

namespace Infrastructure.Persistence.Repositories.Workouts;

public sealed class WorkoutRepository(CoreFitnessDbContext context) : RepositoryBase<Workout, int, WorkoutEntity, CoreFitnessDbContext>(context), IWorkoutRepository
{

    public override int GetId(Workout model)
    {
        return model.Id;
    }

    public override WorkoutEntity ToEntity(Workout model)
    {
        var entity = new WorkoutEntity
        {
            WorkoutID = model.Id,
            WorkoutName = model.Name,
            Date = model.Date,
            Category = model.Category,
            Instructor = model.Instructions,
            Time = model.Time,
            Users = new List<UserEntity>()
        };

        return entity;
    }

    protected override void ApplyPropertyUpdates(WorkoutEntity entity, Workout model)
    {
        entity.Date = model.Date;
        entity.Category = model.Category;
        entity.Instructor = model.Instructions;
        entity.Time = model.Time;
        entity.Users = new List<UserEntity>();
        entity.WorkoutName = model.Name;
    }

    protected override Workout ToDomainModel(WorkoutEntity entity)
    {
        var model = Workout.Create
            (
                entity.WorkoutID,
                entity.WorkoutName,
                entity.Category,
                entity.Instructor,
                entity.Date,
                entity.Time,
                null
            );

        return model;
    }
}
