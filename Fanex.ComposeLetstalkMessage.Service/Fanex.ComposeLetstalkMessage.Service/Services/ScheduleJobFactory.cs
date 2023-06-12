using Quartz;
using Quartz.Spi;

namespace Fanex.ComposeLetstalkMessage.Service.Services
{
    public class ScheduleJobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ScheduleJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return serviceProvider.GetService<ScheduleServiceJob>();
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}