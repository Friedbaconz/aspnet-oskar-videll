using Domain.Abstractions.Repositories.Bookings;
using Domain.Aggregates.Bookings;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities.Booking;
using Infrastructure.Persistence.Entities.Users;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories.Bookings;

public sealed class BookingRepository(CoreFitnessDbContext context) : RepositoryBase<Booking, string, BookingEntity, CoreFitnessDbContext>(context), IBookingRepository
{
    public async Task<List<Booking>> GetAllByUserId(string userid, CancellationToken ct = default)
    {
        var List = new List<Booking>();
        var entities = await context.Bookings.Where(b => b.UserID == userid).ToListAsync(ct);

        foreach (var entity in entities)
        {
            var model = ToDomainModel(entity);
            List.Add(model);
        }
        return List;
    }

    public override string GetId(Booking model)
    {
        return model.Id;
    }

    public override BookingEntity ToEntity(Booking model)
    {
        var entity = new BookingEntity
        {
            BookingID = model.Id,
            UserID = model.UserId,
            WorkoutID = model.WorkoutId,
            User = context.UserEntites.FirstOrDefault(u => u.UserId == model.UserId),
            Workout = context.Workouts.FirstOrDefault(w => w.WorkoutID == model.WorkoutId)
        };

        return entity;
    }

    protected override void ApplyPropertyUpdates(BookingEntity entity, Booking model)
    {
        var existingMembership = context.UserEntites.FirstOrDefault(m => m.UserId == model.UserId);
        if (existingMembership != null)
        {
            entity.User = existingMembership;
        }
        var existingWorkout = context.Workouts.FirstOrDefault(w => w.WorkoutID == model.WorkoutId);
        if (existingWorkout != null)
        {
            entity.Workout = existingWorkout;
        }


        entity.BookingID = model.Id;
        entity.WorkoutID = model.WorkoutId;
        entity.UserID = model.UserId;
    }

    protected override Booking ToDomainModel(BookingEntity entity)
    {

        var model = Booking.Create
            (
                entity.BookingID,
                entity.UserID,
                entity.WorkoutID
            );

        return model;
    }


}
