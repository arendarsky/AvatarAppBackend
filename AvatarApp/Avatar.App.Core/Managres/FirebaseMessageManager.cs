using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Services.Impl;
using FirebaseAdmin.Messaging;
using Message = FirebaseAdmin.Messaging.Message;

namespace Avatar.App.Core.Managres
{
    public class FirebaseMessageManager
    {
        public static Message CreateLikeMessage(User recipient, User sender)
        {
            return new Message
            {
                Notification = new Notification
                {
                    Title = $"{sender.Name}",
                    Body = "Хочет видеть тебя в финале XCE FACTOR 2020!",
                    ImageUrl = sender.ProfilePhoto != null
                        ? $"{MyHttpContext.AppBaseUrl}/api/profile/photo/get/{sender.ProfilePhoto}"
                        : null
                },
                Token = recipient.FireBaseId
            };
        }
    }
}
