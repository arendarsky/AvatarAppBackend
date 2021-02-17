namespace Avatar.App.Api.Models.Authorization
{
    public class AuthorizationResponse
    {
        public string Token { get; set; }
        public bool ConfirmationRequired { get; set; }
    }
}
