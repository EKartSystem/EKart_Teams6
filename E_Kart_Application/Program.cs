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
using System.Text.Json.Serialization;

namespace E_Kart_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Database Configuration
            builder.Services.AddDbContext<EKARTContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Repository & Service Registrations (All Modules)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // Shivansh's Modules
            builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
            builder.Services.AddScoped<IShipperService, ShipperService>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();

            // 3. Infrastructure (AutoMapper, Filters, Auth Helpers)
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<LogActionFilter>();
            builder.Services.AddScoped<TokenService>();

            // 4. Controllers & JSON Configuration
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // 5. Validation Configuration
            builder.Services.AddValidatorsFromAssemblyContaining<AddProductValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            // 6. JWT Authentication Setup
            var jwtKey = builder.Configuration["Jwt:Key"];
            if (!string.IsNullOrEmpty(jwtKey))
            {
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
            }

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // 7. Middleware Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Custom Global Exception Middleware (Must be high in the pipeline)
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}