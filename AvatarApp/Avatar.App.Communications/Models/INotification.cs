using System;

namespace Avatar.App.Communications.Models
{
    public interface INotification
    {
        string Text { get; }
        NotificationAuthor Author { get; }
        DateTime Date { get; }
    }
}
