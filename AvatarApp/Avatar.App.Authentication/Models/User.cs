using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Authentication.Models
{
    public class User
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Email { get; set; }
    }
}
