using Lab4.CustomIdentity;
using Lab4.Data;
using Lab4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lab4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<CustomUserManager>()
                .AddRoleManager<CustomRoleManager>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            Seed.SeedData(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}