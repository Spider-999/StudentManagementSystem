using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

#region Service configuration
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add database connection
builder.Services.AddDbContext<AppDatabaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));

// Add Identity setup
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 0;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDatabaseContext>()
    .AddDefaultTokenProviders();
#endregion

#region Services
// Register LeaderboardService
builder.Services.AddScoped<LeaderboardService>();
#endregion

#region App configuration
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

#endregion

#region Seed user roles in the database
// Access the services we configured
using (var scope = app.Services.CreateScope())
{
    // Create user roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    string[] roles = { "Admin", "Student", "Professor" };

    // Add user roles to the database
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            // Add role if it doesnt exist yet
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create the admin user
    var adminEmail = "admin@mail.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if(admin == null)
    {
        var newAdmin = new Admin
        {
            Name = "admin",
            UserName = adminEmail,
            Email = adminEmail,
        };
        var adminCreate = await userManager.CreateAsync(newAdmin, "admin");
        if(adminCreate.Succeeded)
        {
            // Assign the admin user every role
            foreach(var role in roles)
            {
                var roleAssignResult = await userManager.AddToRoleAsync(newAdmin, role);
            }
        }
    }
}
#endregion

app.Run();
