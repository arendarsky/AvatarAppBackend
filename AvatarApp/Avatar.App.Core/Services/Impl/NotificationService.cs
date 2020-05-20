using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.SharedKernel.Settings;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace Avatar.App.Core.Services.Impl
{
    public class NotificationService: INotificationService
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public NotificationService(FirebaseMessaging firebaseMessaging)
        {
            _firebaseMessaging = firebaseMessaging;
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