using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using XAML_Application1.Commands;

namespace XAML_Application1
{


     public class TimeControlViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

         private DispatcherTimer timer;

         public TimeControlViewModel(int i)
         {
             this.Number = i;
             timer = new DispatcherTimer();
             timer.Interval = TimeSpan.FromSeconds(60);
             timer.Tick += OnTick;
             timer.Start();
             TestChildCommand = new AlwaysExecutableDelegateCommand(
                 () =>
                 {
                     Debug.WriteLine("Clicked:" + Number);
                 });
         }

         private void OnTick(object sender, object e)
         {
             PropertyChanged(this, new PropertyChangedEventArgs("TimeText"));
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

         public int Number
         {
             get;
             set;
         }

         public ICommand TestChildCommand { get; set; }
       
    }
}
