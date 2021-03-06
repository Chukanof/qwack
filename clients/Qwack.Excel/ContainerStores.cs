using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Qwack.Dates;
using Qwack.Excel.Utils;
using Qwack.Futures;
using Qwack.Providers.Json;

namespace Qwack.Excel
{
    public static class ContainerStores
    {
        private const string _calendarJSONFile = "Calendars.json";
        private const string _futureSettingsFile = "futuresettings.json";

        static ContainerStores()
        {
            GlobalContainer = ((IServiceCollection)new ServiceCollection())
             .AddLogging()
             .AddCalendarsFromJson(GetCalendarFilename())
             .AddFutureSettingsFromJson(GetFutureSettingsFile())
             .AddSingleton(typeof(IObjectStore<>), typeof(ExcelObjectStore<>))
             .BuildServiceProvider();

            SessionContainer = GlobalContainer.CreateScope().ServiceProvider;

            SessionContainer.GetRequiredService<IFutureSettingsProvider>();
        }
        
        public static IServiceProvider GlobalContainer { get; internal set; }
        public static IServiceProvider SessionContainer { get;set;}

        private static string GetFutureSettingsFile() => Path.Combine(GetRunningDirectory(), _futureSettingsFile);

        private static string GetRunningDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return dirPath;
        }

        private static string GetCalendarFilename() => Path.Combine(GetRunningDirectory(), _calendarJSONFile);

        public static IObjectStore<T> GetObjectCache<T>() => SessionContainer.GetService<IObjectStore<T>>();
    }
}
