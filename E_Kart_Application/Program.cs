<<<<<<< HEAD
using System.Text;
using E_Kart_Application.DBContext;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Mappings;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using E_Kart_Application.Validators.Orders;
using E_Kart_Application.Validators.OrderDetails;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using E_Kart_Application.Validators;
using E_Kart_Application.Filters;
=======
using E_Kart_Application.DBContext;
using E_Kart_Application.Mappings;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using E_Kart_Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
>>>>>>> origin/feature/employee-categories-module

namespace E_Kart_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(
                    "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
            // 1. Database Configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<EKARTContext>(options =>
                options.UseSqlServer(connectionString));
=======
            builder.Host.UseSerilog();

            builder.Services.AddDbContext<EkartContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));
>>>>>>> origin/feature/employee-categories-module

            // 2. Repository & Service Registrations (Combined)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            // New Order Services from the Merge
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

            // 3. Tools & Mapping
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<LogActionFilter>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddControllers();

<<<<<<< HEAD
            // 4. Validation (Combined)
            builder.Services.AddValidatorsFromAssemblyContaining<AddProductValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            // 5. JWT Authentication
            var jwtKey = builder.Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT Key not found in appsettings.json");
            }

            var key = Encoding.UTF8.GetBytes(jwtKey);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

=======
>>>>>>> origin/feature/employee-categories-module
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            var app = builder.Build();

<<<<<<< HEAD
            // 6. Middleware Pipeline (Correct Order)
=======
>>>>>>> origin/feature/employee-categories-module
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Middleware must be in this order: Exceptions -> Auth -> Auth -> Controllers
            app.UseMiddleware<GlobalExceptionMiddleware>();

<<<<<<< HEAD
            app.UseAuthentication();
            app.UseAuthorization();

=======
>>>>>>> origin/feature/employee-categories-module
            app.MapControllers();

            app.Run();
        }
    }
}