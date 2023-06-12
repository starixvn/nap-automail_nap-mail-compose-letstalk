using Fanex.ComposeLetstalkMessage.Business.Criterias;
using Fanex.ComposeLetstalkMessage.Business.Models;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.ComposeLetstalkMessage.Business.Services
{
    public interface DataService
    {
        Tuple<
            List<Recipient>,
            List<Message>,
            List<Attachment>
            > QueryMessage();

        void UpdateStatus(int notificationId, int recipientId, bool isSuccess);
    }

    public class DataServiceImp : DataService
    {
        private readonly IDynamicRepository repository;

        public DataServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public Tuple<
            List<Recipient>,
            List<Message>,
            List<Attachment>
            > QueryMessage()
        {
            var criteria = new NotifScanSendNewNotificationCriteria { IsTest = AppConfigService.AppConfig.IsTest };
            var result = repository.FetchMultiple<Recipient, Message, Attachment>(criteria);

            return new Tuple<
                List<Recipient>,
                List<Message>,
                List<Attachment>
                >(
                result.Item1.ToList(),
                result.Item2.ToList(),
                result.Item3.ToList()
                );
        }

        public void UpdateStatus(int notificationId, int recipientId, bool isSuccess)
        {
            int status = isSuccess
                ? (int)Status.Successful
                : (int)Status.Failed;
            var criteria = new NotifUpdateNotificationRepientStatusCriteria
            {
                NotificationID = notificationId,
                RecipientID = recipientId,
                StatusID = status
            };

            repository.Execute(criteria);
        }
    }
}