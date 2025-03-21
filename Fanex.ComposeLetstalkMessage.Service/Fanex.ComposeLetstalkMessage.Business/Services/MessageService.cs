using Fanex.ComposeLetstalkMessage.Business.Models;
using System;
using System.Collections.Generic;

namespace Fanex.ComposeLetstalkMessage.Business.Services
{
    using LetstalkService.Client.Models;

    public interface MessageService
    {
        LetstalkModel CreateLetstalkMessageModel(Recipient recipient, Message message);

        LetstalkModel CreateLetstalkAttachmentModel(Recipient recipient, Attachment attachment);
    }

    public class MessageServiceImp : MessageService
    {
        public LetstalkModel CreateLetstalkMessageModel(Recipient recipient, Message message)
        {
            return new LetstalkModel
            {
                TargetName = recipient.Target,
                Group = recipient.Tag,
                LetstalkAttachments = new List<LetstalkAttachment>(),
                Subject = string.IsNullOrEmpty(message.Title) ? string.Empty : message.Title,
                Content = message.Content.Replace("|", Environment.NewLine)
            };
        }

        public LetstalkModel CreateLetstalkAttachmentModel(Recipient recipient, Attachment attachment)
        {
            var letstalkAttachments = new List<LetstalkAttachment>();
            letstalkAttachments.Add(new LetstalkAttachment
            {
                FileName = attachment.FileName,
                Bytes = attachment.Content
            });

            return new LetstalkModel
            {
                TargetName = recipient.Target,
                Group = recipient.Tag,
                LetstalkAttachments = letstalkAttachments,
                Subject = string.Empty,
                Content = string.Empty
            };
        }
    }
}