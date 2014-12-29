using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace TimeTableOne.Background
{
    public static class BackgroundTaskManager
    {
        public async static Task AskRegister()
        {
            if (BackgroundTaskRegistration.AllTasks.Count >= 1)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    task.Value.Unregister(true);
                }
            }
            TimeTrigger trigger = new TimeTrigger(60, false);
            var taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.TaskEntryPoint = "TimeTableOne.Background.NotificationTask";
            taskBuilder.SetTrigger(trigger);
            var regist = taskBuilder.Register();
            await BackgroundExecutionManager.RequestAccessAsync();
            regist = taskBuilder.Register();
        }
    }
}
