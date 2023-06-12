namespace Fanex.ComposeLetstalkMessage.Business.Models
{
    public class Attachment
    {
        public int NotificationID { get; set; }

        public int AttachementID { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}