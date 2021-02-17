namespace Avatar.App.Schedulers.Settings
{
    internal class SchedulerSettings
    {
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// https://docs.coravel.net/Scheduler/#cron-expressions 
        /// Регулярное выражение для задания параметров запуска планировщика
        /// </summary>
        public string Cron { get; set; }
    }
}
