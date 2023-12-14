using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BirthdayCard
{
    public class WeekHoliday
    {
        public int Month;
        public int WeekAtMonth;
        public int WeekDay;
        public string HolidayName;

        public WeekHoliday(int month, int weekAtMonth, int weekDay, string name)
        {
            Month = month;
            WeekAtMonth = weekAtMonth;
            WeekDay = weekDay;
            HolidayName = name;
        }
    }
}