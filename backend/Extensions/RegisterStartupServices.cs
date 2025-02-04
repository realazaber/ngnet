using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace backend.Extensions
{
    public static class RegisterStartupServices
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {                      
            builder.Services.AddHttpContextAccessor();

            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            //string connectionString = builder.Configuration.GetConnectionString("DbConnection");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddControllers();            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            builder.Services.AddScoped<UserManager<User>>();    
            builder.Services.AddScoped<SignInManager<User>>();
            builder.Services.AddScoped<RoleManager<IdentityRole>>();

            builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
           
            builder.Services.AddRateLimiter(r =>
            {
                r.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                r.AddFixedWindowLimiter(
                    "fixed",
                    f =>
                    {
                        f.Window = TimeSpan.FromSeconds(5);
                        f.PermitLimit = 3;
                    });
            });

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost4200",
                                       builder => builder.WithOrigins("http://localhost:4200") 
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            return builder;
        }
    }
}
