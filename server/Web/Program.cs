using Data;

namespace Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDataServices(builder.Configuration);
        builder.Services.AddPresentationServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseCors("NgOrigins");

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
