using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Avatar.App.Api.Models.Impl
{
    public class ConfirmationResponseModel
    {
        [JsonPropertyName("session_guid")]
        public string SessionGuid { get; set; }
    }
}
