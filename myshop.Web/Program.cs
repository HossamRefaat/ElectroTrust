    using Microsoft.EntityFrameworkCore;
using myshop.DataAccess;
using myshop.DataAccess.Implementation;
using myshop.Entities.Repositories;
using Microsoft.AspNetCore.Identity;
using Utilities;
using Stripe;
using myshop.Entities.Models;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace myshop.Entities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/myshop_log.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Error()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddExceptionHandler(options =>
            {
                options.ExceptionHandler = async context =>
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    var errorId = Guid.NewGuid().ToString();

                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    logger.LogError(exception, $"[{errorId}] Unhandled Exception: {exception?.Message}");

                    var log = new Models.Log
                    {
                        Id = errorId,
                        Message = exception?.Message,
                        Exception = exception?.StackTrace.ToString(),
                        TimeStamp = DateTime.Now
                    };

                    var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();
                    unitOfWork.Log.Add(log);
                    unitOfWork.Complete();

                    context.Response.Redirect($"/Error?errorId={errorId}");
                };
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                 options => {
                     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4);
                     options.SignIn.RequireConfirmedAccount = true;

                 })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();


            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler();

            //app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

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


            app.MapControllerRoute(
                name: "error",
                pattern: "Error/{action=Index}/{errorId?}",
                defaults: new { controller = "Error" });

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
