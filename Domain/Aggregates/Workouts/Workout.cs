
using Domain.Aggregates.Users;
namespace Domain.Aggregates.Workouts;

public sealed class Workout
{
    public Workout(Guid id, string name, string category, string instructions, DateTime date, TimeSpan time, List<User> users)
    {
        Id = RequiredGuid(id, nameof(Id));
        Name = RequiredString(name, nameof(Name));
        Category = RequiredString(category, nameof(Category));
        Instructions = RequiredString(instructions, nameof(Instructions));
        Date = RequiredDateTime(date, nameof(Date));
        Time = RequiredTimeSpan(time, nameof(Time));
        Users = users;
    }

    public Guid Id { get; }

    public string Name { get; private set; }

    public string Category { get; private set; }

    public string Instructions { get; private set; } 

    public DateTime Date { get; private set; }

    public TimeSpan Time { get; private set; }

    public List<User> Users { get; private set; }

    private static Guid RequiredGuid(Guid value, string propertyName)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"{propertyName} is required.", propertyName);
        return value;
    }
    private static string RequiredString(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{propertyName} is required.", propertyName);

        return value.Trim();
    }
    private static DateTime RequiredDateTime(DateTime value, string propertyName)
    {
        if (value == default)
            throw new ArgumentException($"{propertyName} is required.", propertyName);
        return value;
    }
    private static TimeSpan RequiredTimeSpan(TimeSpan value, string propertyName)
    {
        if (value == default)
            throw new ArgumentException($"{propertyName} is required.", propertyName);
        return value;
    }

    public static Workout Create(Guid id, string name, string category, string instructions, DateTime date, TimeSpan time, List<User> users)
    {
        return new Workout(Guid.NewGuid(), name, category, instructions, date, time, users);
    }

    public static Workout Rehydrate(Guid id, string name, string category, string instructions, DateTime date, TimeSpan time, List<User> users)
    {
        return new Workout(id, name, category, instructions, date, time, users);
    }

    public void Update(string name, string category, string instructions, DateTime date, TimeSpan time, List<User> users)
    {
        Name = RequiredString(name, nameof(Name));
        Category = RequiredString(category, nameof(Category));
        Instructions = RequiredString(instructions, nameof(Instructions));
        Date = RequiredDateTime(date, nameof(Date));
        Time = RequiredTimeSpan(time, nameof(Time));
        Users = users;
    }
}
