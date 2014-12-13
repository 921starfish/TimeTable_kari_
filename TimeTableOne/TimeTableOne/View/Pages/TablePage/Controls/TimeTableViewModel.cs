using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.Utils.Commands;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    class TimeTableViewModel
    : INotifyPropertyChanged
    {
        private ScheduleData data;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public TimeTableViewModel(Page page, TableKey key)
        {
            data=ApplicationData.Instance.GetSchedule(key.NumberOfDay, key.TableNumber);
            this.TableKey = key;
            this.Page = page;
            this.Width = 200;
            this.Hight = 90;
            this.TableNumber = key.TableNumber;
            this.TableName = (data ?? new ScheduleData()).TableName;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += OnTick;
            timer.Start();
            TableClickedAction = new AlwaysExecutableDelegateCommand(
            () =>
            {
                Page.Frame.Navigate(typeof(EditPage.EditPage), key);
            });
        }

        private void OnTick(object sender, object e)
        {
        }
        public string TableName { get; set; }
        public DayOfWeek dayOfWeek { get; set; }
        public int TableNumber { get; set; }
        public int Hight { get; set; }
        public int Width { get; set; }
        public DispatcherTimer timer { get; set; }
        public AlwaysExecutableDelegateCommand TableClickedAction { get; set; }
        public Page Page { get; set; }
        public TableKey TableKey { get; set; }
    }
}
