using System;

namespace Avatar.App.SharedKernel.Models
{
    public class BaseContestant
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
    }

}
