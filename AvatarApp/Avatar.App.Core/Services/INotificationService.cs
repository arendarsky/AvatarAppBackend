using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace Avatar.App.Core.Services
{
    public interface INotificationService
    {
        Task<string> SendNotificationAsync(Message message);
        Task<BatchResponse> SendNotificationAsync(IEnumerable<Message> messages);
    }
}