using Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;
using Infrastructure.Persistence.Entities.Workouts;
using Infrastructure.Persistence.Entities.Users;

namespace Infrastructure.Persistence.Repositories.Workouts;

public sealed class WorkoutRepository(CoreFitnessDbContext context) : RepositoryBase<Workout, string, WorkoutEntity, CoreFitnessDbContext>(context), IWorkoutRepository
{

    public override string GetId(Workout model)
    {
        return model.Id;
    }

    public override WorkoutEntity ToEntity(Workout model)
    {
        var Users = new List<UserEntity>();

        if (model.Users != null)
        {
            foreach (var Userid in model.Users)
            {
                var existing = context.UserEntites.FirstOrDefault(e => e.workoutId == Userid);

                if (existing != null)
                {
                    Users.Add(existing);
                }
            }
        }

        var entity = new WorkoutEntity
        {
            WorkoutID = model.Id,
            WorkoutName = model.Name,
            Date = model.Date,
            Category = model.Category,
            Instructor = model.Instructions,
            Time = model.Time,
            Users = Users
        };

        return entity;
    }

    protected override void ApplyPropertyUpdates(WorkoutEntity entity, Workout model)
    {
        var NewEntity = context.Workouts.FirstOrDefault(m => m.WorkoutID == entity.WorkoutID);

        if (NewEntity != null)
        {
            entity.Users = NewEntity.Users;
        }
        else
        {
            entity.Users = new List<UserEntity>();
        }

        entity.Date = model.Date;
        entity.Category = model.Category;
        entity.Instructor = model.Instructions;
        entity.Time = model.Time;
        entity.WorkoutName = model.Name;
    }

    protected override Workout ToDomainModel(WorkoutEntity entity)
    {
        var users = context.UserEntites
            .Where(b => b.workoutId == entity.WorkoutID)
            .Select(b => b.workoutId)
            .ToList();

        var model = Workout.Create
            (
                entity.WorkoutName,
                entity.Category,
                entity.Instructor,
                entity.Date,
                entity.Time,
                users
            );

        return model;
    }
}
