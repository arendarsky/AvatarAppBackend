using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Communications.Models;
using Avatar.App.SharedKernel;
using Avatar.App.SharedKernel.Commands;
using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Communications
{
    public interface ICommunicationsComponent
    {
        Task<IEnumerable<LikeNotification>> GetNotificationsAsync(Guid userGuid, int take, int skip);
    }

    internal class CommunicationsComponent: AvatarAppComponent, ICommunicationsComponent
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public CommunicationsComponent(IMediator mediator, FirebaseMessaging firebaseMessaging) : base(mediator)
        {
            _firebaseMessaging = firebaseMessaging;
        }

        public async Task<IEnumerable<LikeNotification>> GetNotificationsAsync(Guid userGuid, int take, int skip)
        {
            var likeNotifications = await Mediator.Send(new GetQuery<LikeNotification>());
            var partOfNotifications = likeNotifications.OrderByDescending(l => l.Date).Take(take).Skip(skip);
            return  await partOfNotifications.ToListAsync();
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
