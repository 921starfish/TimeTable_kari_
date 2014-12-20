using System;
using System.ComponentModel;
using Windows.Security.Cryptography.Certificates;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.Utils.Commands;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public class TimeTableViewModel
    : BasicViewModel
    {
        private ScheduleData data;
        private SolidColorBrush _tableColor;
        private Brush _foreColor;

        public TimeTableViewModel(TableKey key)
        {
            data=ApplicationData.Instance.GetSchedule(key.NumberOfDay, key.TableNumber);
            this.TableKey = key;
            this.Width =TableLayoutManager.getElementWidth(ApplicationData.Instance.Configuration.TableTypeSetting);
            this.Hight = 90;
            this.TableNumber = key.TableNumber;
            this.TableName = (data ?? new ScheduleData()).TableName;
            if (TableName=="")
            {
                TableColor = new SolidColorBrush(Color.FromArgb(225, 240, 237, 243));
            }
            else
            {
                TableColor = new SolidColorBrush(data.ColorData);
                this.Place = (data ?? new ScheduleData()).Place;
            }
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += OnTick;
            timer.Start();
            TableClickedAction = new AlwaysExecutableDelegateCommand(
            () =>
            {
                Frame f = Window.Current.Content as Frame;
                if(f!=null)f.Navigate(typeof(EditPage.EditPage), key);
            });
        }

        public TimeTableViewModel()
        {
            
        }

        private void OnTick(object sender, object e)
        {
        }

        public SolidColorBrush TableColor
        {
            get { return _tableColor; }
            set
            {
                if (Equals(value, _tableColor)) return;
                _tableColor = value;
                OnPropertyChanged();
                ForeColor=value.Color.Liminance()>=0.5?new SolidColorBrush(Colors.Black):
                new SolidColorBrush(Colors.White);
            }
        }

        public Brush ForeColor
        {
            get { return _foreColor; }
            set
            {
                if (Equals(value, _foreColor)) return;
                _foreColor = value;
                OnPropertyChanged();
        }
        }

        public string TableName { get; set; }
        public string Place { get; set; }
        public DayOfWeek dayOfWeek { get; set; }
        public int TableNumber { get; set; }
        public int Hight { get; set; }
        public int Width { get; set; }
        public DispatcherTimer timer { get; set; }
        public AlwaysExecutableDelegateCommand TableClickedAction { get; set; }
        public Page Page { get; set; }
        public TableKey TableKey { get; set; }
    }

    public class TimeTableViewModelInDesign : TimeTableViewModel
    {
        public TimeTableViewModelInDesign()
        {
            this.Width = 170;
            this.Hight = 90;
            TableName = "SampleData";
            TableColor = new SolidColorBrush(Color.FromArgb(225, 128, 57, 123));
        }
    }
}
