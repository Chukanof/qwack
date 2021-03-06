using System;
using System.Collections.Generic;
using Microsoft.Extensions.PlatformAbstractions;
using Qwack.Providers.Json;
using Xunit;

namespace Qwack.Dates.Tests
{
    public class CalendarFacts
    {
        public static readonly string JsonCalendarPath = System.IO.Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Calendars.json");

        public static readonly ICalendarProvider CalendarProvider = CalendarsFromJson.Load(JsonCalendarPath);

        [Fact]
        public void LoadsJsonFileAndHasCalendars() => Assert.True(CalendarProvider.OriginalCalendars.Count > 100);

        [Fact]
        public void CheckUSDCalendarHasHolidayOnJuly4th()
        {
            Assert.True(CalendarProvider.Collection.TryGetCalendar("nyc", out var calendar));

            Assert.True(calendar.IsHoliday(new DateTime(2016, 07, 04)));
        }

        [Fact]
        public void CheckUSDCalendarHasWeekendAsHolidays()
        {
            Assert.True(CalendarProvider.Collection.TryGetCalendar("nyc", out var calendar));

            Assert.True(calendar.IsHoliday(new DateTime(2016, 07, 03)));
        }

        [Theory]
        [MemberData(nameof(GetUSExclusiveHolidays))]
        public void CheckCombinedCalendarHasJuly4th(DateTime dateToCheck)
        {
            CalendarProvider.Collection.TryGetCalendar("nyc", out var us);
            CalendarProvider.Collection.TryGetCalendar("lon", out var gb);
            CalendarProvider.Collection.TryGetCalendar("lon+nyc", out var combined);

            Assert.True(us.IsHoliday(dateToCheck));
            Assert.False(gb.IsHoliday(dateToCheck));
            Assert.True(combined.IsHoliday(dateToCheck));
        }

        [Fact]
        public void CheckThatClonedCalendarIsEqualButNotTheSame()
        {
            CalendarProvider.Collection.TryGetCalendar("nyc", out var usd);
            var clone = usd.Clone();

            Assert.NotSame(usd.DaysToExclude, clone.DaysToExclude);
            Assert.Equal(usd.DaysToExclude, clone.DaysToExclude);
        }

        public static IEnumerable<object[]> GetUSExclusiveHolidays()
        {
            var holidays = new List<object[]>()
            {
                new object[] { new DateTime(2016,07,04) }
            };

            return holidays;
        }
    }
}
