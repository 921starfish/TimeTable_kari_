using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Data;

namespace TimeTableOne.Utils
{
    public static class DateTimeUtil
    {
        /// <summary>
        /// 渡された値が正しい日付なのか検証します
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <param name="iDay"></param>
        /// <returns>検証結果</returns>
        public static bool IsDate(int iYear, int iMonth, int iDay)
        {
            if ((DateTime.MinValue.Year > iYear) || (iYear > DateTime.MaxValue.Year))
            {
                return false;
            }

            if ((DateTime.MinValue.Month > iMonth) || (iMonth > DateTime.MaxValue.Month))
            {
                return false;
            }

            int iLastDay = DateTime.DaysInMonth(iYear, iMonth);

            if ((DateTime.MinValue.Day > iDay) || (iDay > iLastDay))
            {
                return false;
            }

            return true;
        }

        public static DateTime NextKeyDay(DateTime time,DayOfWeek weekday)
        {
           DateTime normalized=new DateTime(time.Year,time.Month,time.Day,0,0,0);
            for (int i = 0; i < 7; i++)
            {
                if (normalized.AddDays(i).DayOfWeek == weekday)
                {
                    return normalized.AddDays(i);
                }
            }
            throw  new InvalidDataContractException();
        }

    }
}
