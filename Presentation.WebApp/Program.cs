using Application.Abstractions.Identity;
using Application.Extensions;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Memberships.Services;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Application.Workouts.Abstractions;
using Application.Workouts.Inputs;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Users;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Presentation.WebApp.Controllers;
using Presentation.WebApp.Models.Memberships;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddSession();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication(builder.Configuration, builder.Environment);


var app = builder.Build();
app.MapControllers();
await PersistenceDatabaseInitializer.InitializeAsync(app.Services, app.Environment);

StaticWebAssetsLoader.UseStaticWebAssets(app.Environment, app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CoreFitnessDbContext>();

        if (!context.Memberships.Any())
            using (var sscope = app.Services.CreateScope())
            {
                var ServiceMembership = scope.ServiceProvider.GetRequiredService<IRegisterMembershipService>();

                var workoutService = scope.ServiceProvider.GetRequiredService<IRegisterWorkoutService>();

                if (!context.Memberships.Any())
                {
                    //Basic
                    var basicmember = new RegisterMemebershipInput
                    (
                        name: "Basic",
                        description: "Access to gym during staffed hours.",
                        benefits: new List<RegisterBenfitsInput>(),
                        status: "Active",
                        type: "monthly",
                        pricing: 19.99m,
                        monthlyDuration: 1
                    );

                    basicmember.benefits.AddRange(
                    new RegisterBenfitsInput
                    (
                        benefit: "new benefit 1"
                    ),
                    new RegisterBenfitsInput
                    (
                        benefit: "new benefit 2"
                    ),
                    new RegisterBenfitsInput
                    (
                        benefit: "new benefit 3"
                    ));

                    await ServiceMembership.ExecuteAsync(basicmember);

                    //Premium
                    var Premiummember = new RegisterMemebershipInput
                    (
                        name: "Premium",
                        description: "Access to gym during staffed hours.",
                        benefits: new List<RegisterBenfitsInput>(),
                        status: "Active",
                        type: "monthly",
                        pricing: 39.99m,
                        monthlyDuration: 1
                    );

                    Premiummember.benefits.AddRange(
                    new RegisterBenfitsInput
                    (
                        benefit: "new benefit 4"
                    ),
                    new RegisterBenfitsInput
                    (
                        benefit: "new benefit 5"
                    ),
                    new RegisterBenfitsInput
                    (
                        benefit: "new benefit 6"
                    ));

                    await ServiceMembership.ExecuteAsync(Premiummember);
                }

                if (!context.Workouts.Any())
                {
                    var workout1 = new RegisterWorkoutInput
                    (
                        Name: "Morning Yoga",
                        Category: "Yoga",
                        Instructions: "Start your",
                        Date: DateTime.Today,
                        Time: TimeSpan.FromHours(7)
                    );

                    await workoutService.ExecuteAsync(workout1);
                }

                await context.SaveChangesAsync();
            }
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
