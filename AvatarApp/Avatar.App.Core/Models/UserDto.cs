namespace Avatar.App.Core.Models
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? ConsentToGeneralEmail { get; set; }
    }
}
