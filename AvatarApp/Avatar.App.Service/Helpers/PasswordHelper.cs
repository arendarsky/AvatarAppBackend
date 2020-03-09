﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Avatar.App.Service.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(
                password));
            return Convert.ToBase64String(hash);
        }
    }
}
