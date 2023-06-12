using ONEbook.UM.Configurations;

namespace Fanex.ComposeLetstalkMessage.Business.Models
{
    public class ConfigSettingModel
    {
        public string SentryClientKey { get; set; }

        public string CategorySentryLog { get; set; }

        public string LetstalkServiceApi { get; set; }

        public string CronSchedule { get; set; }

        public string Environment { get; set; }

        public bool IsTest { get; set; }

        public UMClientConfiguration UMConfig { get; set; }
    }
}