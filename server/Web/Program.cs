using Core.ExceptionHandling;
using Data;
using Services;

namespace Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDataServices(builder.Configuration);
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddPresentationServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseCors("NgOrigins");

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseAuthentication();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
