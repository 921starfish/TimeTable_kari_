using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.Utils.Commands;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.View.Pages.DayPage.Controls
{
    public class DayPageTableViewModel : BasicViewModel
    {
        private ScheduleData data;
        private TableKey TableKey;
        public DayPageTableViewModel(TableKey key)
        {
            data = ApplicationData.Instance.GetSchedule(key.NumberOfDay, key.TableNumber);
            this.TableKey = key;
            this.Width = 120;
            this.Hight = 90;
            this.TableNumber = key.TableNumber;
            this.TableName = (data ?? new ScheduleData()).TableName;

            this.TableColor = new SolidColorBrush(data.ColorData);
            this.Place = (data ?? new ScheduleData()).Place;
            TableClickedAction = new AlwaysExecutableDelegateCommand(
            () =>
            {
                Frame f = Window.Current.Content as Frame;
                if (f != null) f.Navigate(typeof(EditPage.EditPage), key);
            });
        }

        public int Width { get; set; }
        public int Hight { get; set; }
        public int TableNumber { get; set; }
        public string TableName { get; set; }
        public SolidColorBrush TableColor { get; set; }
        public string Place { get; set; }
        public AlwaysExecutableDelegateCommand TableClickedAction { get; set; }
    }
}
