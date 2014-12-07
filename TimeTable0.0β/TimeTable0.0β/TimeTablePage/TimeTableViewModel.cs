using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable0._0β.Commands;
using Windows.UI.Xaml;

namespace TimeTable0._0β.TimeTablePage
{
    class TimeTableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public TimeTableViewModel(TablePage page, int i,int w)
        {
            this.TableNumber = i+1;
            Page = page; 
            switch (w)
            { 
                case 1:
                    dayOfWeek = DayOfWeek.Monday;
                    break;
                case 2:
                    dayOfWeek = DayOfWeek.Tuesday;
                    break;
                case 3:
                    dayOfWeek = DayOfWeek.Wednesday;
                    break;
                case 4:
                    dayOfWeek = DayOfWeek.Thursday;
                    break;
                case 5:
                    dayOfWeek = DayOfWeek.Friday;
                    break;
                case 6:
                    dayOfWeek = DayOfWeek.Saturday;
                    break;
                case 7:
                    dayOfWeek = DayOfWeek.Sunday;
                    break;
            }
            this.Width = 200;
            this.Hight = 90;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += OnTick;
            timer.Start();
            TableClickedAction =new AlwaysExecutableDelegateCommand(
            ()=>
            {
                Page.Frame.Navigate(typeof(SplitPage));          
            });
          
        }

        private void OnTick(object sender, object e)
        {
        }
        public DayOfWeek dayOfWeek { get; set; }
        public int TableNumber { get; set; }
        public int Hight { get; set; }
        public int Width { get; set; }
        public DispatcherTimer timer { get; set; }
        public AlwaysExecutableDelegateCommand TableClickedAction { get; set; }
        public TablePage Page { get; set; }
    }
}
