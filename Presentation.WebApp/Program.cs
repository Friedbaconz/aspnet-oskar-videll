using Application.Extensions;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Memberships.Services;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddSession();

var Infrastructure = builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
var Application = builder.Services.AddApplication(builder.Configuration, builder.Environment);

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
            var ServiceMembership = scope.ServiceProvider.GetRequiredService<IRegisterMembershipService>();

            if (!context.Memberships.Any())
            {
                    //Basic
                    var basicmember = new RegisterMemebershipInput
                    (
                        name: "Basic",
                        description: "Access to gym during staffed hours.",
                        benefits: new List<RegisterBenfitsInput>(),
                        status: "Active",
                        type: "Monthly",
                        pricing: 19.99m,
                        monthlyDuration: 1
                    );

                    basicmember.benefits.AddRange(
                        new RegisterBenfitsInput
                        (
                        id: 1,
                        benefit: "MorningYoga"
                        ),
                        new RegisterBenfitsInput
                        (
                        id: 2,
                        benefit: "MorningYoga"
                        ),
                        new RegisterBenfitsInput
                        (
                        id: 3,
                        benefit: "MorningYoga"
                        ));
                    
                    ServiceMembership?.ExecuteAsync(basicmember);
                    
                    //Premium
                    var Premiummember = new RegisterMemebershipInput
                    (
                        name: "Premium",
                        description: "Access to gym during staffed hours.",
                        benefits: new List<RegisterBenfitsInput>(),
                        status: "Active",
                        type: "Monthly",
                        pricing: 39.99m,
                        monthlyDuration: 1
                    );
                    
                    Premiummember.benefits.AddRange(
                        new RegisterBenfitsInput
                        (
                        id: 4,
                        benefit: "MorningYoga"
                        ),
                        new RegisterBenfitsInput
                        (
                        id: 5,
                        benefit: "MorningYoga"
                        ),
                        new RegisterBenfitsInput
                        (
                        id: 6,
                        benefit: "MorningYoga"
                        ));

                    ServiceMembership?.ExecuteAsync(Premiummember);
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
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" };

    foreach(var roleName in roleNames)
    {
        var exsist = await roleManager.RoleExistsAsync(roleName);
        if (!exsist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
