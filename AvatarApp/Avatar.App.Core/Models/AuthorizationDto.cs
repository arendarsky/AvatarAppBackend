using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Core.Models
{
    public class AuthorizationDto
    {
        public string Token { get; set; }
        public bool ConfirmationRequired { get; set; }
    }
}
