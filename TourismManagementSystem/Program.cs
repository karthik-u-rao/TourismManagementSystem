using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tourism.DataAccess;
using Tourism.DataAccess.Models;
using TourismManagementSystem.Data;

namespace TourismManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // TourismDbContext (packages, bookings, payments)
            builder.Services.AddDbContext<Tourism.DataAccess.TourismDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TourismDb")));

            // Identity ApplicationDbContext
            builder.Services.AddDbContext<TourismDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TourismDb")));

            // Identity with roles
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TourismDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // must come before UseAuthorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await RoleSeeder.SeedRolesAndAdminAsync(services);
            }

            app.Run();
        }
    }
}
