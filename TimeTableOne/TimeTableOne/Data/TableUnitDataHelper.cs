using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TimeTableOne.View.Pages.EditPage;

namespace TimeTableOne.Data
{
    public static class TableUnitDataHelper
    {
        public static ScheduleData GetCurrentSchedule()
        {
            var f = (Frame) Window.Current.Content;
            var editPage = f.Content as EditPage;
            return editPage.ViewModel.ScheduleData;
        }
    }
}
