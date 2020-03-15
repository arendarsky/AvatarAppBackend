using System;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class Message : BaseEntity
    {
        public User From { get; set; }
        public User To { get; set; }
        public DateTime SentDate { get; set; }
        public string Text { get; set; }
        public string Contact { get; set; }
        public bool? Accepted { get; set; }
    }
}
