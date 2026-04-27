
using Domain.Aggregates.Users;
namespace Domain.Aggregates.Workouts;

public sealed class Workout
{

    public string Id { get; private set; } = null!;

    public string? Name { get; private set; }

    public string? Category { get; private set; }

    public string? Instructions { get; private set; } 

    public DateTime Date { get; private set; }

    public TimeSpan Time { get; private set; }

    public IEnumerable<string> Users { get; private set; }

    private Workout()
    {

    }

    private Workout(string id)
    {
        Id = id;
    }

    public static Workout Create()
    {
        var workout = new Workout
        (
            Guid.NewGuid().ToString().ToString()
        );

        return workout;
    }


    public static Workout Create(string id,string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        var workout = new Workout(id)
        {
            Name = RequiredString(name, nameof(Name)),
            Category = RequiredString(category, nameof(Category)),
            Instructions = RequiredString(instructions, nameof(Instructions)),
            Date = date.Date,
            Time = RequiredTime(time, nameof(Time)),
            Users = users
        };
        
        return workout;
    }

    private static TimeSpan RequiredTime(TimeSpan value, string propertyname)
    {
        if(value < TimeSpan.Zero)
            throw new ArgumentException($"{propertyname} Can't be a negative time.");
        return value;
    }

    private static string RequiredString (string value, string propertyname)
    {
        if(string.IsNullOrEmpty(value))
            throw new ArgumentNullException($"{propertyname} is required.");

        return value;
    }

    public void Update(string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        Name = string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
        Category = string.IsNullOrWhiteSpace(category) ? string.Empty : category.Trim();
        Instructions = string.IsNullOrWhiteSpace(instructions) ? string.Empty : instructions.Trim();
        Date = date.Date;
        Time = time.Add(TimeSpan.FromSeconds(-time.Seconds));
        Users = users;
    }
}
