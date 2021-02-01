using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Avatar.App.Schedulers.Settings;
using Coravel;
using Coravel.Scheduling.Schedule.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avatar.App.Schedulers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string SchedulerSection = "SchedulersStartTimeSettings";

        public static void AddCronSchedulers(this IServiceCollection services, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
        {
            services.AddScheduler();
            var schedulers = GetAllSchedulers(assemblies);

            foreach (var scheduler in schedulers)
                services.Add(new ServiceDescriptor(scheduler, scheduler, serviceLifetime));
        }

        private static IEnumerable<Type> GetAllSchedulers(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(
                    assembly =>
                        assembly.GetTypes())
                ?.Where(x => x.IsClass && !x.IsAbstract && typeof(ICronInvocable).IsAssignableFrom(x));
        }

        public static ISchedulerConfiguration UseCronSchedulers(
            this IApplicationBuilder app,
            IConfiguration configuration, params Assembly[] assemblies)
        {
            var schedulerTypes = GetAllSchedulers(assemblies).ToArray();
            if (!schedulerTypes.Any())
                return null;
            var provider = app.ApplicationServices;
            return provider.UseScheduler(sch =>
                RegisterSchedulers(configuration, sch, schedulerTypes, SchedulerSection));
        }

        private static void RegisterSchedulers(
            IConfiguration configuration,
            IScheduler sch,
            IEnumerable<Type> schedulersTypes,
            string schedulerSection)
        {
            var schedulersSettings = new SchedulerSettingsCollection();
            configuration.GetSection(schedulerSection).Bind(schedulersSettings);
            foreach (var schedulerType in schedulersTypes)
                RegisterScheduler(sch, schedulersSettings, schedulerType);
        }

        private static void RegisterScheduler(
            IScheduler sch,
            IReadOnlyDictionary<string, SchedulerSettings> schedulersSettings,
            Type schedulerType)
        {
            var schedulerSettings = schedulersSettings[schedulerType.Name];
            if (schedulerSettings.Enabled)
                sch.ScheduleInvocableType(schedulerType).Cron(schedulerSettings.Cron);
        }
    }
}
