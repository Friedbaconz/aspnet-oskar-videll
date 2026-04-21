
using Domain.Aggregates.Users;
namespace Domain.Aggregates.Workouts;

public sealed class Workout
{

    public string Id { get; private set; } = null!;

    public string Name { get; private set; }

    public string Category { get; private set; }

    public string Instructions { get; private set; } 

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
        var workout = new Workout()
        {
            Id = Guid.NewGuid().ToString().ToString()
        };

        return workout;
    }


    public static Workout Create(string id, string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        var workout = new Workout(id)
        {
            Name = name,
            Category = category,
            Instructions = instructions,
            Date = date,
            Time = time,
            Users = users
        };
        
        return workout;
    }

    public void Update(string name, string category, string instructions, DateTime date, TimeSpan time, IEnumerable<string> users)
    {
        Name = name.Trim();
        Category = category.Trim();
        Instructions = instructions.Trim();
        Date = date.Date;
        Time = time.Add(TimeSpan.FromSeconds(-time.Seconds));
        Users = users;
    }
}
