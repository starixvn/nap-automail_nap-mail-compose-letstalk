using Fanex.Data.Repository;

namespace Fanex.ComposeLetstalkMessage.Business.Criterias
{
    public class NotifScanSendNewNotificationCriteria : CriteriaBase
    {
        public bool IsTest { get; set; }

        public override string GetSettingKey() => "Notif_ScanSendNewNotification";

        public override bool IsValid() => true;
    }
}