﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Entities.Settings
{
    public class EmailSettings
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
