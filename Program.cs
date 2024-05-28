using Microsoft.AspNetCore.Identity;
using net_il_mio_fotoalbum.Data;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Areas.Identity.Data;

namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                        var connectionString = builder.Configuration.GetConnectionString("ProfileContextConnection") ?? throw new InvalidOperationException("Connection string 'ProfileContextConnection' not found.");

            builder.Services.AddDbContext<PhotoContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PhotoContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Photo}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Popolare il database con dati iniziali se � vuoto
            PhotoManager.SeedDatabase();

            app.Run();
        }
    }
}
