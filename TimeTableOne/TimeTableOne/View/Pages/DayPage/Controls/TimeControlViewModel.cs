using System;
using System.ComponentModel;
using TimeTableOne.Common;
using TimeTableOne.Utils;
using Windows.UI.Xaml;

namespace TimeTableOne.View.Pages.DayPage.Controls
{
    public class TimeControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged= delegate { };
        public TimeControlViewModel()
        {
            TimerManager.Instance.GUITick += Instance_Tick;
        }

        void Instance_Tick(object sender, EventArgs e)
        {
            OnTick();
        }

        private void OnTick()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("TimeText"));
            PropertyChanged(this, new PropertyChangedEventArgs("WeekText"));
            PropertyChanged(this, new PropertyChangedEventArgs("DayText"));
        }
        public string TimeText
        {
            get
            {
                return DateTime.Now.ToString("HH:mm");
            }
        }

        public string WeekText
        {
            get
            {
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Friday:
                        return "金曜日";
                    case DayOfWeek.Monday:
                        return "月曜日";
                    case DayOfWeek.Saturday:
                        return "土曜日";
                    case DayOfWeek.Sunday:
                        return "日曜日";
                    case DayOfWeek.Thursday:
                        return "木曜日";
                    case DayOfWeek.Tuesday:
                        return "火曜日";
                    case DayOfWeek.Wednesday:
                        return "水曜日";
                    default:
                        return null;
                }
            }
        }

        public string DayText
        {
            get
            {
                return DateTime.Now.ToString("M月d日");
            }
        }
        public DispatcherTimer timer { get; set; }
    }
}
