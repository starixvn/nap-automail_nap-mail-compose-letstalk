using Fanex.ComposeLetstalkMessage.Business.Services;
using ONEbook.UM.Services;
using Quartz;

namespace Fanex.ComposeLetstalkMessage.Service.Services
{
    public class ScheduleServiceJob : IJob
    {
        private readonly Log log;
        private readonly IMaintenanceService um;
        private readonly MainService mainService;

        public ScheduleServiceJob(
            Log log,
            IMaintenanceService um,
            MainService mainService)
        {
            this.log = log;
            this.um = um;
            this.mainService = mainService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var isUM = um.IsUnderMaintenance();
                if (isUM)
                {
                    return Task.CompletedTask;
                }

                mainService.Execute();
            }
            catch (Exception ex)
            {
                log.Log(ex);
            }

            return Task.CompletedTask;
        }
    }
}