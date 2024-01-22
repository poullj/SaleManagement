
using DataAccessLayer;
using Serilog;
using Serilog.Events;
using Shared.Types;

using Microsoft.Extensions.Logging;
using WebApi.MiddleWare;



namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")            
                   .Build();

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();
            builder.Logging.SetMinimumLevel(LogLevel.Information);
            var logger = LoggerFactory.Create(loggingBuilder =>
            {
                var logger = new LoggerConfiguration()
                  .ReadFrom.Configuration(config)
                   // Uncomment to reduce logging from MS
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                   .MinimumLevel.Override("System", LogEventLevel.Warning)
                   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                  .Enrich.WithRequestHeader(headerName: HttpHeaderNames.Workstation)
                  .Enrich.WithRequestHeader(headerName: HttpHeaderNames.ProcessID)
                  .Enrich.WithRequestHeader(headerName: HttpHeaderNames.UserName)
                  .Enrich.WithRequestHeader(headerName: HttpHeaderNames.SessionID)
                  .Enrich.With(new AppDetailsEnricher())
                  .CreateLogger();
                loggingBuilder.AddSerilog(logger, dispose: true);
                Log.Logger = logger;

                var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Log.Debug($"Program started with environment {envName}");
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<ContextCorrelator>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocument();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseOpenApi();

            app.UseSwaggerUi();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
