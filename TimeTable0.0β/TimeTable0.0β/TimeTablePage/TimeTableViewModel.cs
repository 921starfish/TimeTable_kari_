using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable0._0β.Commands;
using TimeTable0._0β.EditPage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimeTable0._0β.TimeTablePage
{
    class TimeTableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public TimeTableViewModel(Page page, TableKey key)
        {
            this.TableKey = key;
            this.Page = page; 
            this.Width = 200;
            this.Hight = 100;
            this.TableNumber = key.TableNumber;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += OnTick;
            timer.Start();
            TableClickedAction =new AlwaysExecutableDelegateCommand(
            ()=>
            {
                Page.Frame.Navigate(typeof(SplitPage),key);          
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
        public Page Page { get; set; }
        public TableKey TableKey { get; set; }
    }
}
