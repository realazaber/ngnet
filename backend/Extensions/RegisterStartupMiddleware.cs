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
            app.UseCors("AllowLocalhost4200");
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                var user = context.User;
                if (user.Identity.IsAuthenticated)
                {
                    Console.WriteLine($"✅ User authenticated: {user.Identity.Name}");
                    foreach (var claim in user.Claims)
                    {
                        Console.WriteLine($"📝 Claim Type: {claim.Type}, Value: {claim.Value}");
                    }
                }
                else
                {
                    Console.WriteLine($"❌ User NOT authenticated. Token: {context.Request.Headers["Authorization"]}");
                }

                await next();
            });


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

                if (userManager.Users.Count() == 0)
                {
                dbContext.Database.Migrate();
                    //Add admin user
                    User user = new User
                    {
                        Id = "Superuser",
                        FirstName = Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME"),
                        LastName = Environment.GetEnvironmentVariable("ADMIN_LASTNAME"),
                        Email = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
                        ProfileImg = Environment.GetEnvironmentVariable("ADMIN_PROFILEIMG") ?? "",
                    };
                    string password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
                    userManager.CreateAsync(user, password);
                }

            }
           
            return app;
        }
    }
}
