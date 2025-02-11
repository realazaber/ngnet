using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Extensions
{
    public static class RegisterStartupMiddleware
    {        
        public static WebApplication RegisterMiddleware(this WebApplication app)
        {
            app.UseHttpsRedirection();
                       
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();
            
            return app;
        }

        public static WebApplication RunStartProcesses(this WebApplication app) {

            using (var scope = app.Services.CreateScope())
            {
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                UserManager<User> userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                
                dbContext.Database.Migrate();                              
            }
           
            return app;
        }
    }
}
