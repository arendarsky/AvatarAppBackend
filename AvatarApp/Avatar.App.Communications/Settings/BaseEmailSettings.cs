namespace Avatar.App.Communications.Settings
{
    public abstract class BaseEmailSettings
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
