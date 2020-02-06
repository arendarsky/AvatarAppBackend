using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Avatar.App.Api.Models.Impl
{
    public class ConfirmationResponseModel
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
