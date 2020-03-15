using Microsoft.Extensions.Logging;

namespace Avatar.App.SharedKernel
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
