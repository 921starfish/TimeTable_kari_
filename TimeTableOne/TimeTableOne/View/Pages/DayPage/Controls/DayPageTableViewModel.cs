using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Common;
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
        private DispatcherTimer timer;
        private int _width;
        private int _height;

        public DayPageTableViewModel(TableKey key)
        {
            data = ApplicationData.Instance.GetSchedule(key.NumberOfDay, key.TableNumber);
            this.TableKey = key;
            OnTick(null,null);
            this.TableNumber = key.TableNumber;
            this.TableNumberText = key.TableNumber.ToString() + "時限";
            this.Time = ApplicationData.Instance.TimeSpans[key.TableNumber - 1].FromTime.ToString("HH:mm") + " - " +
                        ApplicationData.Instance.TimeSpans[key.TableNumber - 1].ToTime.ToString("HH:mm");
            this.TableName = (data ?? new ScheduleData()).TableName;
            this.TableColor = new SolidColorBrush(data.ColorData);
            this.Place = (data ?? new ScheduleData()).Place;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += OnTick;
            timer.Start();
            TableClickedAction = new AlwaysExecutableDelegateCommand(
            () =>
            {
                Frame f = Window.Current.Content as Frame;
                if (f != null) f.Navigate(typeof(EditPage.EditPage), key);
            });
        }

       

        private void OnTick(object sender, object e)
        {
            if (TableKey.Equals(ScheduleManager.Instance.CurrentKey))
            {
                this.Width = 600;
                this.Height = 250;
            }
            else
            {
                this.Width = 350;
                this.Height = 150;
            }

        }

        public string Time { get; set; }
        public string TableNumberText { get; set; }

        public int Width
        {
            get { return _width; }
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int TableNumber { get; set; }
        public string TableName { get; set; }
        public SolidColorBrush TableColor { get; set; }
        public string Place { get; set; }
        public AlwaysExecutableDelegateCommand TableClickedAction { get; set; }
    }
}
