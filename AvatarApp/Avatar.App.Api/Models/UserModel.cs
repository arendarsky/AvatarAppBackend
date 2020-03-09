using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avatar.App.Api.Models
{
    public class UserModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
        public ICollection<VideoModel> Videos { get; set; }
    }
}
