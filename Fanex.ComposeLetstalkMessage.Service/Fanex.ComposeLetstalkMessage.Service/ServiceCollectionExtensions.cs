using Fanex.ComposeLetstalkMessage.Business.Models;
using Fanex.ComposeLetstalkMessage.Business.Services;
using Fanex.ComposeLetstalkMessage.Service.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.LetstalkService.Client;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using SharpRaven;
using LetstalkConfig = Fanex.LetstalkService.Core.Services.AppConfigService;

namespace Fanex.ComposeLetstalkMessage.Service;

using Logging.Extensions;
using Logging.Extensions.AspNetCore;
using Logging.Extensions.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection LoadConfig(this IServiceCollection services)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        var text = File.ReadAllText("appsettings.json");
        AppConfigService.AppConfig = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
        LetstalkConfig.Config.LetstalkServiceApi = AppConfigService.AppConfig.LetstalkServiceApi;

        return services;
    }

    public static IServiceCollection AddFanexServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connections = configuration.GetSection("ConnectionStrings")
                .GetChildren()
                .AsEnumerable()
                .ToDictionary
                (
                     x => x.Key,
                     y => y.Value
                );

        DbSettingProviderManager
         .StartNewSession()
         .UseConnectionStrings(connections)
         .UseDefaultDbSettingProvider(configuration.GetSection("StoredProcedurePath").Value)
         .Run();

        return services;
    }

    public static IServiceCollection AddSentryService(this IServiceCollection services)
    {
        LogManager
        .SetDefaultLogCategory(AppConfigService.AppConfig.CategorySentryLog)
        .Use(new List<ILogEngine>
        {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(AppConfigService.AppConfig.SentryClientKey)
                })
        });
        ExceptionHandler
            .UseConfig(new LoggingConfiguration())
            .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<ScheduleServiceJob>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<LetstalkClientService, LetstalkClientServiceImp>();
        services.AddSingleton<MessageService, MessageServiceImp>();
        services.AddSingleton<MainService, MainServiceImp>();

        OnebookUMInitializer.InitializeUMClientManager();
        services.AddUMCoreService();
        return services;
    }

    public static void StartScheduler(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        scheduler.JobFactory = new ScheduleJobFactory(serviceProvider);

        var job = JobBuilder.Create<ScheduleServiceJob>().Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity("AutoScheduleService", "AutoScheduleService")
            .StartNow()
            .WithCronSchedule(AppConfigService.AppConfig.CronSchedule)
            .Build();

        scheduler.ScheduleJob(job, trigger).Wait();
        scheduler.Start().Wait();
    }
}