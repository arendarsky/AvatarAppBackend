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
        private readonly EnvironmentConfig _environmentConfig;
        private readonly FirebaseMessaging _messagingInstance;

        public NotificationService(IOptions<EnvironmentConfig> environmentConfig)
        {
            _environmentConfig = environmentConfig.Value;
            _messagingInstance = GetMessagingInstance();
        }

        public async Task<string> SendNotificationAsync(Message message)
        {
            var result = await _messagingInstance.SendAsync(message, false);
            return result;
        }

        public async Task<BatchResponse> SendNotificationAsync(IEnumerable<Message> messages)
        {
            var result = await _messagingInstance.SendAllAsync(messages, false);
            return result;
        }

        #region PrivateMethods

        private FirebaseMessaging GetMessagingInstance()
        {
            var path = _environmentConfig.STORAGE_PATH + "\\FireBaseAuth.json";
            FirebaseApp app = null;
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "FBApp");
            }
            catch (Exception)
            {
                app = FirebaseApp.GetInstance("FBApp");
            }
            var messagingInstance = FirebaseMessaging.GetMessaging(app);
            return messagingInstance;
        }

        #endregion
    }
}