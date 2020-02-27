using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Service.Security
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }

    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
