namespace Fanex.ComposeLetstalkMessage.Business.Models
{
    public class Recipient
    {
        public int NotificationID { get; set; }

        public int RecipientID { get; set; }

        public int ConversationID { get; set; }

        public string Target { get; set; }

        public string Tag { get; set; }
    }
}