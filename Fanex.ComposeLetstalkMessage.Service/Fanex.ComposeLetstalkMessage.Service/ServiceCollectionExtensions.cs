using Fanex.ComposeLetstalkMessage.Business.Models;
using Fanex.ComposeLetstalkMessage.Business.Services;
using Fanex.ComposeLetstalkMessage.Service.Services;
using Fanex.Data.Repository;
using Fanex.LetstalkService.Client;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using LetstalkConfig = Fanex.LetstalkService.Client.AppConfigService;

namespace Fanex.ComposeLetstalkMessage.Service;

using AppConfigService = Business.Services.AppConfigService;

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

    public static IServiceCollection AddInternalServices(this IServiceCollection services)
    {
        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<ScheduleServiceJob>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<LetstalkClientService, LetstalkClientServiceImp>();
        services.AddSingleton<MessageService, MessageServiceImp>();
        services.AddSingleton<MainService, MainServiceImp>();

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