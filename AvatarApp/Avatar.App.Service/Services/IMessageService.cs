using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;

namespace Avatar.App.Service.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(string text, Guid fromGuid, User to);
        Task SetAcceptanceStatusAsync(long messageId, Guid toGuid, bool isAccepted);
        Task<ICollection<Message>> GetMessages(Guid userGuid);
        Task<ICollection<Message>> GetContacts(Guid userGuid);
    }
}
