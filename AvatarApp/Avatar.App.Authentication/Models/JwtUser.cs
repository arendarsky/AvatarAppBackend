using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Avatar.App.Authentication.Security;
using Microsoft.IdentityModel.Tokens;

namespace Avatar.App.Authentication.Models
{
    public class JwtUser
    {
        public JwtUser(User user, string password, IJwtSigningEncodingKey signingEncodingKey)
        {
            _user = user;

            if (_user == null)
            {
                IsUserExists = false;
                return;
            }

            IsEmailConfirmed = user.IsEmailConfirmed;

            if (!user.IsPasswordCorrect(password))
            {
                return;
            }

            IsPasswordCorrect = true;
            _signingEncodingKey = signingEncodingKey;

        }

        public bool IsEmailConfirmed { get; }
        public bool IsUserExists { get; }
        public bool IsPasswordCorrect { get; }
        private readonly User _user;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public string JwtToken
        {
            get
            {
                if (_signingEncodingKey == null)
                {
                    return null;
                }

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, _user.Guid.ToString())
                };
                var token = new JwtSecurityToken(
                    "AvatarApp",
                    "AvatarAppClient",
                    claims,
                    signingCredentials: new SigningCredentials(
                        _signingEncodingKey.GetKey(),
                        _signingEncodingKey.SigningAlgorithm));
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
