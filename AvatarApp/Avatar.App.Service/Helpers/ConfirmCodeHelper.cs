using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Service.Helpers
{
    public static class ConfirmCodeHelper
    {
        public static string CreateRandomCode(int length = 6)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
    }
}
