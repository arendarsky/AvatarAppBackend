using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Api.Models.Impl
{
    public class SigningSymmetricKey: IJwtSigningEncodingKey, IJwtSigningDecodingKey
    {
        private readonly SymmetricSecurityKey _secretKey;

        public string SigningAlgorithm => SecurityAlgorithms.HmacSha256;

        public SigningSymmetricKey(string key)
        {
            this._secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public SecurityKey GetKey() => this._secretKey;
    }
}
