using Fanex.ComposeLetstalkMessage.Business.Models;
using Fanex.LetstalkService.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.ComposeLetstalkMessage.Business.Services
{
    public interface MainService
    {
        void Execute();
    }

    public class MainServiceImp : MainService
    {
        private readonly Log log;
        private readonly DataService dataService;
        private readonly LetstalkClientService letstalkService;
        private readonly MessageService messageService;

        public MainServiceImp(
            Log log,
            DataService dataService,
            LetstalkClientService letstalkService,
            MessageService messageService)
        {
            this.log = log;
            this.dataService = dataService;
            this.letstalkService = letstalkService;
            this.messageService = messageService;
        }

        public void Execute()
        {
            try
            {
                if (!AppConfigService.IsProcessing)
                {
                    AppConfigService.IsProcessing = true;
                    var data = dataService.QueryMessage();
                    var recipients = data.Item1;
                    var messages = data.Item2;
                    var attachments = data.Item3;

                    foreach (var recipient in recipients)
                    {
                        SendMessage(messages, attachments, recipient);
                    }

                    AppConfigService.IsProcessing = false;
                }
            }
            catch (Exception ex)
            {
                log.Log(ex);
            }
            finally
            {
                AppConfigService.IsProcessing = false;
            }
        }

        private void SendMessage(List<Message> messages, List<Attachment> attachments, Recipient recipient)
        {
            var result = new List<bool>();
            var message = messages.FirstOrDefault(s => s.NotificationID == recipient.NotificationID);
            var letstalkMessageModel = messageService.CreateLetstalkMessageModel(recipient, message);
            var statusMessage = letstalkService.SendMessage(letstalkMessageModel);
            result.Add(statusMessage);

            var attachmentsByNotificationId = attachments.Where(s => s.NotificationID == message.NotificationID).ToList();
            foreach (var attachment in attachmentsByNotificationId)
            {
                var lestalkAttachmentModel = messageService.CreateLetstalkAttachmentModel(recipient, attachment);
                var statusAttachment = letstalkService.SendMessage(lestalkAttachmentModel);
                result.Add(statusAttachment);
            }

            var status = !result.Any(s => !s);
            dataService.UpdateStatus(recipient.NotificationID, recipient.RecipientID, status);
        }
    }
}