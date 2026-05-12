using EKartMVC.Services;
namespace EKartMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiBaseUrl = "https://localhost:7000/";

            builder.Services.AddHttpClient<ProductApiService>(c => c.BaseAddress = new Uri(apiBaseUrl));

            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient<OrderApiService>(c =>
            {
                c.BaseAddress = new Uri(apiBaseUrl);
            });

            builder.Services.AddHttpClient<CustomerApiService>(x => x.BaseAddress = new Uri(apiBaseUrl));
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 minutes ka session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
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
            app.UseSession();
            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
