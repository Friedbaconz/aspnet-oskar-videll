using Domain.Aggregates.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions.Repositories.Bookings;

public interface IBookingRepository : IRepositoryBase<Booking, int>
{
}
