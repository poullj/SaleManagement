using CommunityToolkit.Mvvm.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using SaleManagementWpfClient.Helper;
using Polly;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using SaleManagementWpfClient.Service;
using WebApi.Client;
using SaleManagementWpfClient.ViewModels;
using Shared.Types;

namespace SaleManagementWpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string SessionId;
        private IHost host;
        public App()
        {
            SessionId = Guid.NewGuid().ToString();

            ExceptionOutlet.ExceptionOutletEvent += ExceptionOutlet_ExceptionOutletEvent;

            IConfigurationRoot config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

            var logConnection = config.GetConnectionString("LogConnection");

            host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(config);
                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config)
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.With(new AppDetailsEnricher())
                        .Enrich.WithProperty(HttpHeaderNames.SessionID, SessionId)
                        .Enrich.WithProperty(HttpHeaderNames.Workstation, Environment.MachineName)
                        .Enrich.WithProperty(HttpHeaderNames.ProcessID, Environment.ProcessId)
                        .CreateLogger();
                    loggingBuilder.AddSerilog(logger, dispose: true);
                    Log.Logger = logger;
                })
                .Build();
            Ioc.Default.ConfigureServices(host.Services);
        }

        private void ExceptionOutlet_ExceptionOutletEvent(bool expectedException, ApiException apiException)
        {
           
           if (expectedException)
           {
                // TODO: Show user friendly message of expected exception
           }
           else
           {
                // TODO: Show message of system error - contact the system responsible
            }
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy<HttpResponseMessage>
              .Handle<HttpRequestException>()
              .Or<SocketException>()
              .OrResult(msg => msg.StatusCode > HttpStatusCode.InternalServerError || msg.StatusCode == HttpStatusCode.RequestTimeout)
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), OnRetry);
        }

        private static void OnRetry(DelegateResult<HttpResponseMessage> delegateResult, TimeSpan timespan)
        {
            Log.Warning(exception: delegateResult.Exception, "API call failed - waiting {@delay} before retrying", timespan);
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            // Configurations
            services.AddHttpClient(configuration["APISettings:Name"], c => { c.BaseAddress = new Uri(configuration["APISettings:BaseUrl"]); })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddHttpMessageHandler<AddCorrelationIdToHeader>()
                .AddPolicyHandler(GetRetryPolicy());

            services.AddSingleton<ContextCorrelator>();
            services.AddSingleton<IDistrictClient, DistrictService>();
            services.AddSingleton<ISalesPersonClient, SalesPersonService>();
            services.AddTransient<AddCorrelationIdToHeader>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>();
            services.AddSingleton<IApplicationContext, ApplicationContext>();

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var applicationContext = host.Services.GetService<IApplicationContext>();
            BuildApplicationContext(applicationContext);

            await host.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
                
        void BuildApplicationContext(IApplicationContext applicationContext)
        {
            applicationContext.UserName = Environment.UserName;
            applicationContext.WorkStation = Environment.MachineName;
            applicationContext.SessionID = SessionId;
            applicationContext.EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
    }
}
