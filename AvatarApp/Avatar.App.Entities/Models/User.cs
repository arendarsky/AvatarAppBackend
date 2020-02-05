using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Entities.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<Video> Videos { get; set; }
    }
}
