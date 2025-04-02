using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Yarp.ReverseProxy;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((context, services) =>
                {
                    services.AddReverseProxy()
                        .LoadFromConfig(context.Configuration.GetSection("ReverseProxy"));

                    // Настроим Swagger для отображения API
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo 
                        { 
                            Title = "YARP API", 
                            Version = "v1" 
                        });
                    });

                });

                webBuilder.Configure(app =>
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        // Swagger для Auth сервиса
                        c.SwaggerEndpoint("/swagger/auth/v1/swagger.json", "Auth Service API");
    
                        // Swagger для Friend сервиса
                        c.SwaggerEndpoint("/swagger/friend/v1/swagger.json", "Friend Service API");
    
                        // Добавьте RoutePrefix если нужно
                        c.RoutePrefix = "swagger";
                    });

                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapReverseProxy();
                    });
                });
            });
}