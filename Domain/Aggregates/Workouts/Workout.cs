
using Domain.Aggregates.Users;
namespace Domain.Aggregates.Workouts;

public sealed class Workout
{
    public Workout(string id, string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        Id = RequiredString(id.ToString(), nameof(Id));
        Name = RequiredString(name, nameof(Name));
        Category = RequiredString(category, nameof(Category));
        Instructions = RequiredString(instructions, nameof(Instructions));
        Date = RequiredDateTime(date, nameof(Date));
        Time = RequiredTimeSpan(time, nameof(Time));
        Users = users;
    }

    public string Id { get; }

    public string Name { get; private set; }

    public string Category { get; private set; }

    public string Instructions { get; private set; } 

    public DateTime Date { get; private set; }

    public TimeSpan Time { get; private set; }

    public IEnumerable<string> Users { get; private set; }

    private static int RequiredInt(int value, string propertyName)
    {
        if (value <= 0)
            throw new ArgumentException($"{propertyName} must be a positive integer.", propertyName);
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

    public static Workout Create(string id, string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        return new Workout(id = Guid.NewGuid().ToString(), name, category, instructions, date, time, users);
    }

    public static Workout Rehydrate(string id, string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        return new Workout(id = Guid.NewGuid().ToString(), name, category, instructions, date, time, users);
    }

    public void Update(string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        Name = RequiredString(name, nameof(Name));
        Category = RequiredString(category, nameof(Category));
        Instructions = RequiredString(instructions, nameof(Instructions));
        Date = RequiredDateTime(date, nameof(Date));
        Time = RequiredTimeSpan(time, nameof(Time));
        Users = users;
    }
}
