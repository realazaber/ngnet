using backend.Data;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace backend.Extensions
{
    public static class RegisterCustomServices
    {
        
        public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
        {

            string logsPath = Environment.GetEnvironmentVariable("LOGS_PATH");

            logsPath += "/logs-.txt";
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .WriteTo.Console()
                            .WriteTo.File(logsPath, rollingInterval: RollingInterval.Day)
                            .CreateLogger();
                      
            builder.Services.AddScoped<CustomAuthService>();
            //All your custom dependency injection services go here.
            return builder;
        }
    }
}
