using System;

namespace Avatar.App.Entities.Models
{
    public class Message
    {
        public long Id { get; set; }
        public User From { get; set; }
        public User To { get; set; }
        public DateTime SentDate { get; set; }
        public string Text { get; set; }
        public string Contact { get; set; }
        public bool? Accepted { get; set; }
    }
}
