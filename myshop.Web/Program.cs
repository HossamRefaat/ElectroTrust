using Microsoft.EntityFrameworkCore;
using myshop.DataAccess;
using myshop.DataAccess.Implementation;
using myshop.Entities.Repositories;
using Microsoft.AspNetCore.Identity;
using Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using myshop.Entities.Models;
using myshop.DataAccess.Services;
using Microsoft.AspNetCore.Diagnostics;

namespace myshop.Entities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                 options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4))
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();


            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IssueLogger>();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession();

            var app = builder.Build();

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

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

                        using (var scope = scopeFactory.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            db.Issues.Add(new Issue
                            {
                                Title = "Unhandled Exception",
                                Description = exceptionHandlerPathFeature.Error.ToString()
                            });
                            db.SaveChanges();
                        }
                    }

                    context.Response.Redirect("/Home/Error");
                });
            });


            StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Customer",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            //app.MapControllerRoute(
            //    name: "areas",
            //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

            //app.MapControllerRoute(
            //    name: "Customer",
            //    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
