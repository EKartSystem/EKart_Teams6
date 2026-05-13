
using System.Net.Http.Json;
namespace EKartMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Configure HttpClient for the API. In Development accept the local dev certificate
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddHttpClient<CustomerApiService>(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:7000/");
                })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    // Only for local development to accept the ASP.NET Core dev certificate
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                });
            }
            else
            {
                builder.Services.AddHttpClient<CustomerApiService>(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:7000/");
                });
            }
            // Session requires a backing cache
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
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
            app.UseSession();


            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
