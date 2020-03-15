using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;

namespace Avatar.App.Core.Services.Impl
{
    public class MessageService: IMessageService
    {

        public MessageService()
        {
       
        }

        public async Task SendMessageAsync(string text, Guid fromGuid, User to)
        {
            //var from = await GetUserAsync(fromGuid);

            //var message = new Message
            //{
            //    From = from,
            //    To = to,
            //    Text = text,
            //    SentDate = DateTime.Now
            //};

            //await _context.Messages.AddAsync(message);
            //await _context.SaveChangesAsync();
        }

        public async Task SetAcceptanceStatusAsync(long messageId, Guid toGuid, bool isAccepted)
        {
            //var to = await GetUserAsync(toGuid);
            //var message = await _context.Messages.Include(m => m.To).FirstOrDefaultAsync(m => m.Id == messageId);
            //if (message == null || message.To.Id != to.Id) throw new MessageNotFoundException();
            //if (isAccepted)
            //{
            //    message.Contact = message.To.Contact;
            //    message.Accepted = true;
            //}
            //else
            //{
            //    message.Accepted = false;
            //}
            //await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Message>> GetMessages(Guid userGuid)
        {
            //var messages = await Task.Run(() =>
            //{
            //    return _context.Messages.Where(m => (!m.Accepted.HasValue || m.Accepted.Value == true) && m.To.Guid == userGuid).ToList();
            //});
            //return messages;

            return null;
        }

        public async Task<ICollection<Message>> GetContacts(Guid userGuid)
        {
            //var contacts = await Task.Run(() =>
            //{
            //    return _context.Messages.Where(m =>
            //        m.Accepted.HasValue && m.Accepted.Value == true && m.From.Guid == userGuid).ToList();
            //});
            //return contacts;

            return null;
        }

        #region Private Methods


        #endregion
    }
}
