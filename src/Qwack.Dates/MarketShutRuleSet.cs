using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Qwack.Dates
{
    public class MarketShutRuleSet
    {
        public DayOfWeek[] ShutWholeDay { get; set; }
        public TimeSpan CloseWhenHolidayFollows { get; set; }
        public TimeSpan OpenOnHolidayWhenNormalDayFollows { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public string Calendar { get; set; }
        public TimePeriod[] MarketPauses { get; set; }

        private Calendar _calendar;
        private ICalendarProvider _calendarProvider;

        public MarketShutRuleSet(ICalendarProvider calendarProvider) => _calendarProvider = calendarProvider;
        
        public void LoadFromXml(XElement elementToLoad, string calendar, TimeZoneInfo timezone)
        {
            Calendar = calendar;
            TimeZone = timezone;
            _calendar = _calendarProvider.Collection[calendar];
            ShutWholeDay = elementToLoad.Elements("ShutWholeDay").Select(e => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), e.Value)).ToArray();
            CloseWhenHolidayFollows = TimeSpan.Parse(elementToLoad.Element("CloseWhenHolidayFollows").Value);
            OpenOnHolidayWhenNormalDayFollows = TimeSpan.Parse(elementToLoad.Element("OpenOnHolidayWhenNormalDayFollows").Value);
            MarketPauses = elementToLoad.Elements("MarketPause").Select(e =>
                new TimePeriod() { Start = TimeSpan.Parse(e.Element("Start").Value), End = TimeSpan.Parse(e.Element("End").Value) }).ToArray();
        }
                
        public bool IsOpenFromUTC(DateTime checkDate)
        {
            checkDate = TimeZoneInfo.ConvertTimeFromUtc(checkDate, TimeZone);
            if (ShutWholeDay.Contains(checkDate.DayOfWeek))
                return false;

            //Okay it is not shut all of this day

            var isHoliday = _calendarProvider.Collection[Calendar].IsHoliday(checkDate.Date);
            var isNextDayAHoliday = _calendarProvider.Collection[Calendar].IsHoliday(checkDate.Date.AddDays(1));

            if (isHoliday && isNextDayAHoliday)
                return false;

            var timeOfDay = checkDate.TimeOfDay;
            if (isHoliday)
            {
                //It is a holiday check the time is equal or after the open
                if (timeOfDay >= OpenOnHolidayWhenNormalDayFollows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //So it is a normal market day check it isn't after close before holiday
            if (isNextDayAHoliday)
            {
                if (timeOfDay >= CloseWhenHolidayFollows)
                {
                    //It is after close
                    return false;
                }
            }
            //We have made it this far, check it isn't during the market pause

            foreach (var mp in MarketPauses)
            {
                //normal case
                if (mp.Start < mp.End)
                {
                    if (timeOfDay >= mp.Start && timeOfDay < mp.End)
                    {
                        return false;
                    }
                }
                else //case where pause spans midnight
                {
                    if (timeOfDay >= mp.Start || timeOfDay < mp.End)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public (DateTime pauseStart, DateTime pauseEnd) NextMarketPause(DateTime inDate)
        {
            inDate = TimeZoneInfo.ConvertTimeFromUtc(inDate, TimeZone);

            var nF = MarketPauses.Where(g => inDate.TimeOfDay < g.Start).Count();
            if (nF == 0)
            {
                inDate = inDate.AddPeriod(RollType.F, _calendar, new Frequency(1, DatePeriodType.B));
                inDate = new DateTime(inDate.Year, inDate.Month, inDate.Day);
            }

            foreach (var mp in MarketPauses.OrderBy(k => k.Start))
            {
                if (inDate.TimeOfDay < mp.Start)
                {
                    var pStart = inDate.Date + mp.End;
                    var pEnd = inDate.Date + mp.End;
                    if (pEnd < pStart)
                    {
                        pEnd = pEnd.AddPeriod(RollType.F, _calendarProvider.Collection[Calendar], new Frequency(1, DatePeriodType.B));
                    }
                    return (TimeZoneInfo.ConvertTimeToUtc(pStart, TimeZone), TimeZoneInfo.ConvertTimeToUtc(pEnd, TimeZone));
                }
            }

            return (DateTime.MaxValue, DateTime.MaxValue);
        }
    }
}