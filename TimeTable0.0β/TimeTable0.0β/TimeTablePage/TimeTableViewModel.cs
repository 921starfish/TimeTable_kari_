using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace TimeTable0._0β.TimeTablePage
{
    class TimeTableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public TimeTableViewModel(int i,int w)
        {
            this.TableNumber = i+1;
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
        }

        private void OnTick(object sender, object e)
        {
        }
        public DayOfWeek dayOfWeek { get; set; }
        public int TableNumber { get; set; }
        public int Hight { get; set; }
        public int Width { get; set; }
        public DispatcherTimer timer { get; set; }
    }
}
