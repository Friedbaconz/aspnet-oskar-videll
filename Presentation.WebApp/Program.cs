using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddSession();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication(builder.Configuration, builder.Environment);

var app = builder.Build();

await PersistenceDatabaseInitializer.InitializeAsync(app.Services, app.Environment);

StaticWebAssetsLoader.UseStaticWebAssets(app.Environment, app.Configuration);

//make a couple of memberships and workouts for testing
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CoreFitnessDbContext>();

    if (!context.Memberships.Any())
        using (var sscope = app.Services.CreateScope())
        {
            var ccontext = sscope.ServiceProvider.GetRequiredService<CoreFitnessDbContext>();

            if (!ccontext.Memberships.Any())
            {
                var basicMembership = new MembershipEntity
                {
                    MembershipID = Guid.NewGuid().ToString(),
                    Name = "Basic",
                    Description = "Access to gym during staffed hours.",
                    Type = "Monthly",
                    Status = "Active",
                    Pricing = 19.99m,
                    DurationInMonths = 1,
                    Benefits = new List<MembershipBenefitEntity>()
                };

                basicMembership.Benefits.Add(new MembershipBenefitEntity
                {
                    MembershipBenefitID = 1,
                    Benefit = "Gym floor access"
                });
                basicMembership.Benefits.Add(new MembershipBenefitEntity
                {
                    MembershipBenefitID = 2,
                    Benefit = "Locker room access"
                });

                var premiumMembership = new MembershipEntity
                {
                    MembershipID = Guid.NewGuid().ToString(),
                    Name = "Premium",
                    Description = "24/7 access, all classes included.",
                    Type = "Monthly",
                    Status = "Active",
                    Pricing = 39.99m,
                    DurationInMonths = 1,
                    Benefits = new List<MembershipBenefitEntity>()
                };

                premiumMembership.Benefits.Add(new MembershipBenefitEntity
                {
                    MembershipBenefitID = 3,
                    Benefit = "All classes included"
                });
                premiumMembership.Benefits.Add(new MembershipBenefitEntity
                {
                    MembershipBenefitID = 4,
                    Benefit = "Priority booking for classes"
                });
                premiumMembership.Benefits.Add(new MembershipBenefitEntity
                {
                    MembershipBenefitID = 5,
                    Benefit = "Access to VIP lounge"
                });

                context.Memberships.AddRange(basicMembership, premiumMembership);
            }

            if (!context.Workouts.Any())
            {
                context.Workouts.AddRange(
                    new WorkoutEntity
                    {
                        WorkoutID = Guid.NewGuid().ToString(),
                        WorkoutName = "Morning Yoga",
                        Category = "Yoga",
                        Instructor = "Alice",
                        Date = DateTime.SpecifyKind(DateTime.Today.AddDays(1), DateTimeKind.Utc)
                    },
                    new WorkoutEntity
                    {
                        WorkoutID = Guid.NewGuid().ToString(),
                        WorkoutName = "Evening HIIT",
                        Category = "HIIT",
                        Instructor = "Bob",
                        Date = DateTime.SpecifyKind(DateTime.Today.AddDays(1), DateTimeKind.Utc)
                    }
                );
            }

            await context.SaveChangesAsync();
        }

    if (!context.Workouts.Any())
    {
        context.Workouts.AddRange(
            new WorkoutEntity
            {
                WorkoutID = Guid.NewGuid().ToString(),
                WorkoutName = "Morning Yoga",
                Category = "Yoga",
                Instructor = "Alice",
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(7)
            },
            new WorkoutEntity
            {
                WorkoutID = Guid.NewGuid().ToString(),
                WorkoutName = "Evening HIIT",
                Category = "HIIT",
                Instructor = "Bob",
                Date = DateTime.Today.AddDays(2),
                Time = TimeSpan.FromHours(18)
            }
        );
    }

    await context.SaveChangesAsync();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
