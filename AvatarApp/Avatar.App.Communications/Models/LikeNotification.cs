using System;
using Avatar.App.Casting.Models;

namespace Avatar.App.Communications.Models
{
    public class LikeNotification: INotification
    {
        public string Text => $"{Author.Name} хочет видеть тебя в финале XCE FACTOR";
        public DateTime Date { get; set; }
        public Video Video { get; set; }
        public NotificationAuthor Author { get; set; }
    }
}
