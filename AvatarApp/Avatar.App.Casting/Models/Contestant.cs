using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Casting.Models
{
    public class Contestant
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
    }
}
