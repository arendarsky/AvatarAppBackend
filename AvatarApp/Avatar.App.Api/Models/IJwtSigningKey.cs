using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Api.Models
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }
    }

    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
