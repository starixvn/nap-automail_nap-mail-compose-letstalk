using Fanex.Data.Repository;

namespace Fanex.ComposeLetstalkMessage.Business.Criterias
{
    public class NotifUpdateNotificationRepientStatusCriteria : NonQueryCommand
    {
        public int NotificationID { get; set; }

        public int RecipientID { get; set; }

        public int StatusID { get; set; }

        public override string GetSettingKey() => "Notif_UpdateNotificationRepientStatus";

        public override bool IsValid() => true;
    }
}