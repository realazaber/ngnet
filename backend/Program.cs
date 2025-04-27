using backend.Extensions;
using backend.Models;
using backend.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.RegisterServices();
builder.AddCustomServices(); 
WebApplication app = builder.Build();

app.UseCors("DevHost");
app.UseRateLimiter();
app.UseStaticFiles();


app.RegisterMiddleware();
app.RunStartProcesses();

using (var scope = app.Services.CreateScope())
{    
    await SeedUserAndRoles.CreateUserAndRolesAsync(scope.ServiceProvider);
}

app.MapGroup("api/identity").MapCustomIdentityApi<User>();

string logMessage = "App started at " + DateTime.Now;
PrintLogger.PrintLog(logMessage, LogLevels.Information);

app.Run();