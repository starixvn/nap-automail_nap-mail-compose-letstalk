using Fanex.ComposeLetstalkMessage.Business.Models;

namespace Fanex.ComposeLetstalkMessage.Business.Services
{
    public static class AppConfigService
    {
        public static ConfigSettingModel AppConfig
        {
            get; set;
        }

        public static bool IsProcessing { get; set; }
    }
}