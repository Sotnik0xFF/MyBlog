using Microsoft.AspNetCore.Authentication.Cookies;
using MyBlog.Application;

namespace MyBlog.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddApplicationServices();
        builder.Services.AddControllersWithViews();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
            options => options.LoginPath = "/Home/AccessDenied");

        var app = builder.Build();

        app.UseExceptionHandler("/Home/ExceptionHandler");

        app.Use(async (context, next) =>
        {
            await next();
            if (context.Response.StatusCode == 404)
            {
                context.Request.Path = "/Home/PageNotFound";
                await next();
            }
        });

        app.UseAuthentication();

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
