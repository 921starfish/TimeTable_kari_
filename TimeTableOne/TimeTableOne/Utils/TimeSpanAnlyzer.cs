using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Data;

namespace TimeTableOne.Utils
{
    public static class TimeSpanAnlyzer
    {
        public static int IdealSpanLength()
        {
            List<ScheduleTimeSpan> spans = ApplicationData.Instance.TimeSpans;
            CountSet cs=new CountSet();
            foreach (var scheduleTimeSpan in spans)
            {
                cs.Add((int) (scheduleTimeSpan.ToTime-scheduleTimeSpan.FromTime).TotalMinutes);
            }
            return cs.MaxCount();
        }

        public static int IdealSpanStride()
        {
            List<ScheduleTimeSpan> spans = ApplicationData.Instance.TimeSpans;
            CountSet cs = new CountSet();
            for (int index = 0; index < spans.Count-1; index++)
            {
                var scheduleTimeSpan = spans[index];
                cs.Add((int) (spans[index+1].FromTime - spans[index].ToTime).TotalMinutes);
            }
            return cs.MaxCount();
        }

        private class CountSet : Dictionary<int, int>
        {
            public new void Add(int value)
            {
                if (ContainsKey(value))
                {
                    this[value] += 1;
                }
                else
                {
                    base.Add(value,1);
                }
            }

            public int MaxCount()
            {
                int max=0, value=-1;
                foreach (var dict in this)
                {
                    if (dict.Value > value)
                    {
                        value = dict.Value;
                        max = dict.Key;
                    }
                }
                return max;
            }
        }
    }
}
