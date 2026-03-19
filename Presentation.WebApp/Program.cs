using Application.Extensions;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication(builder.Configuration, builder.Environment);

builder.Services.AddDbContext<CoreFitnessDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("SqlConnection"), 
    sql => sql.MigrationsAssembly("Infrastructure")
));

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
