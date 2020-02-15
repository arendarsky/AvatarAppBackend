using System.Text.Json.Serialization;

namespace Avatar.App.Api.Models.Impl
{
    public class ConfirmationResponseModel
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
