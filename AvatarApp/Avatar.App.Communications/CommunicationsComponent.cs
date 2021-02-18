using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Communications.Commands;
using Avatar.App.Communications.Models;
using Avatar.App.SharedKernel;
using FirebaseAdmin.Messaging;
using MediatR;

namespace Avatar.App.Communications
{
    public interface ICommunicationsComponent
    {
        Task<IEnumerable<LikeNotification>> GetNotificationsAsync(Guid userGuid, int take, int skip);
    }

    internal class CommunicationsComponent: AvatarAppComponent, ICommunicationsComponent
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public CommunicationsComponent(IMediator mediator, FirebaseMessaging firebaseMessaging, IQueryManager queryManager) : base(mediator, queryManager)
        {
            _firebaseMessaging = firebaseMessaging;
        }

        public async Task<IEnumerable<LikeNotification>> GetNotificationsAsync(Guid userGuid, int take, int skip)
        {
            var likeNotificationsQuery = await Mediator.Send(new GetLikeNotificationsQuery(userGuid));
            var partOfNotifications = likeNotificationsQuery.OrderByDescending(l => l.Date).Take(take).Skip(skip);
            return  await QueryManager.ToListAsync(partOfNotifications);
        }

        public async Task<string> SendNotificationAsync(Message message)
        {
            var result = await _firebaseMessaging.SendAsync(message, false);
            return result;
        }

        public async Task<BatchResponse> SendNotificationAsync(IEnumerable<Message> messages)
        {
            var result = await _firebaseMessaging.SendAllAsync(messages, false);
            return result;
        }
    }
}
