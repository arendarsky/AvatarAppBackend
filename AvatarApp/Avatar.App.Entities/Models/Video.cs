using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Avatar.App.Entities.Models
{
    public class Video
    {
        public long Id { get; set; }

        public User User { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }
        public bool? IsApproved { get; set; }

        [NotMapped] public string FileName => Name + Extension;

        public ICollection<WatchedVideo> WatchedBy { get; set; }
    }
}
