<<<<<<< HEAD
using EKartMVC.Services;
=======
using E_Kart_MVC.Services;
>>>>>>> origin/feature/orders

namespace EKartMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var apiBaseUrl = "https://localhost:7000/";

            builder.Services.AddHttpClient<ProductApiService>(c => c.BaseAddress = new Uri(apiBaseUrl));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
            builder.Services.AddHttpClient<OrderApiService>(c =>
            {
                c.BaseAddress = new Uri(apiBaseUrl);
            });

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
