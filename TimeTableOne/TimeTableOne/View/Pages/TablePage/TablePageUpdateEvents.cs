using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTableOne.Data;
using Windows.UI.Popups;

namespace TimeTableOne.View.Pages.TablePage
{
    public static class TablePageUpdateEvents
    {
        public static event Action EditTimeDisplay;

        public static void OnEditTimeDisplay()
        {
            if (EditTimeDisplay != null) EditTimeDisplay();
        }

        public static event Action CommitTimeDisplay;

        public static bool OnCommitTimeDisplay()
        {
            ScheduleTimeSpansList.Clear();
            if (RequestTimeCommit != null) RequestTimeCommit();

            DateTime a = new DateTime();
            foreach (var itr in ScheduleTimeSpansList)
            {
                if ((itr.FromTime - itr.ToTime).TotalMilliseconds > 0 || (a - itr.FromTime).TotalSeconds > 0)
                {
                    MessageDialog dialog = new MessageDialog("入力が不適切です。", "エラー");
                    dialog.ShowAsync();
                    return false;
                }
                 
                a = itr.ToTime;
            }
           
            if (CommitTimeDisplay != null) CommitTimeDisplay();
            return true;
        }

        public static List<ScheduleTimeSpan> ScheduleTimeSpansList=new List<ScheduleTimeSpan>();

        public static event Action RequestTimeCommit;

   
    }
}
