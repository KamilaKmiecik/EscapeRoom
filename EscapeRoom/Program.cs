using EscapeRoom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EscapeRoom.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsetting.Development.json"); 

builder.Services.AddDbContext<EscapeRoomContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EscapeRoomContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EscapeRoomContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseAuthentication();;

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] {  "Admin", "RoomWorker", "DeskWorker", "Client" };

    foreach (var role in roles)
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string email = "admin@mail.com";
    string password = "Test@1234";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser() { UserName = email, Email = email, EmailConfirmed = true };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }

    email = "RoomWorker@mail.com";
    password = "Test@1234";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser() { UserName = email, Email = email, EmailConfirmed = true };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "RoomWorker");
    }

    email = "DeskWorker@mail.com";
    password = "Test@1234";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser() { UserName = email, Email = email, EmailConfirmed = true };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "DeskWorker");
    }

    email = "Client@mail.com";
    password = "Test@1234";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser() { UserName = email, Email = email, EmailConfirmed = true };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Client");
    }

}

app.Run();

