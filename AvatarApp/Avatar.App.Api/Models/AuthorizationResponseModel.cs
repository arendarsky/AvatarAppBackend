namespace Avatar.App.Api.Models
{
    public class AuthorizationResponseModel
    {
        public string Token { get; set; }
        public bool ConfirmationRequired { get; set; }
    }
}
