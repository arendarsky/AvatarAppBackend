using Microsoft.Extensions.Logging;

namespace Avatar.App.Entities
{
    public class Logger
    {
        public static ILogger Log { get; private set; }

        public static void RegisterLogger(ILogger logger)
        {
            Log = logger;
        }
    }
}
