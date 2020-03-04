using System.Text.Json.Serialization;

namespace Avatar.App.Api.Models.Impl
{
    public class AuthorizationResponseModel
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
