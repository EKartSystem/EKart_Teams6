using System.Text;
using E_Kart_Application.DBContext;
using E_Kart_Application.Exceptions;
using E_Kart_Application.Mappings;
using E_Kart_Application.Repositories;
using E_Kart_Application.Services;
using E_Kart_Application.Validators;
using E_Kart_Application.Validators.Orders;
using E_Kart_Application.Validators.OrderDetails;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using E_Kart_Application.Filters;

namespace E_Kart_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Database Configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<EKARTContext>(options =>
                options.UseSqlServer(connectionString));

            // 2. Service Registrations (Products, Locations, Customers)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            // 3. Service Registrations (Orders & OrderDetails - from previous merge)
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

            // 4. Service Registrations (Employees & Categories - from current merge)
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // 5. Tools, Auth Helpers & Controllers
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<LogActionFilter>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddControllers();

            // 6. Validation Configuration
            builder.Services.AddValidatorsFromAssemblyContaining<AddProductValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            // 7. JWT Authentication Setup
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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // 8. Middleware Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Global Exception Middleware must be near the top
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}