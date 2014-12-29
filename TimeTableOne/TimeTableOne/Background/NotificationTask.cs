using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using TimeTableOne.Common;

namespace TimeTableOne.Background
{
    public class NotificationTask:IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            NotificationManager.Instance.UpdateNotificationDataOfToday();
        }
    }
}
