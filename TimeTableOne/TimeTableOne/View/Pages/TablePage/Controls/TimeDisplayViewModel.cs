using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using TimeTableOne.Utils.Commands;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public class TimeDisplayViewModel:BasicViewModel
    {
        public TimeDisplayViewModel()
        {
            TimeRegions=new ObservableCollection<TimeDisplayUnitViewModel>();
        }

        public ObservableCollection<TimeDisplayUnitViewModel> TimeRegions { get; set; }

    }

    public class TimeDisplayUnitViewModel:BasicViewModel
    {
        private string _fromTimeToEdit;
        private string _toTimeToEdit;
        private DateTime _fromTime;
        private DateTime _toTime;

        public TimeDisplayUnitViewModel()
        {
            
        }
        public int TimeIndex { get; set; }

        public DateTime FromTime
        {
            get { return _fromTime; }
            set
            {
                if (value.Equals(_fromTime)) return;
                this.FromTimeToEdit = string.Format("{0}:{1:D2}", value.Hour, value.Minute);
                _fromTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime ToTime
        {
            get { return _toTime; }
            set
            {
                if (value.Equals(_toTime)) return;
                this.ToTimeToEdit = string.Format("{0}:{1:D2}", value.Hour, value.Minute);
                _toTime = value;
                OnPropertyChanged();
            }
        }

        public string FromTimeToEdit
        {
            get { return _fromTimeToEdit; }
            set
            {
                if (value == _fromTimeToEdit) return;
                _fromTimeToEdit = value;
                OnPropertyChanged();
            }
        }

        public string ToTimeToEdit
        {
            get { return _toTimeToEdit; }
            set
            {
                if (value == _toTimeToEdit) return;
                _toTimeToEdit = value;
                OnPropertyChanged();
            }
        }
    }

    public class TimeDisplayUnitViewModelInDesign : TimeDisplayUnitViewModel
    {
        public TimeDisplayUnitViewModelInDesign()
        {
            this.TimeIndex = 1;
            this.FromTime=new DateTime(2015,1,1,8,50,0);
            this.ToTime=new DateTime(2015,1,1,10,20,0);
        }
    }

    public class TimeDisplayViewModelInDesign:TimeDisplayViewModel
    {

        private static DateTime genFromTime(int h, int m)
        {
            return new DateTime(2015,1,1,h,m,0);
        }
        public TimeDisplayViewModelInDesign()
        {
            TimeRegions.Add(new TimeDisplayUnitViewModel(){TimeIndex = 1,FromTime = genFromTime(8,50),ToTime = genFromTime(10,20)});
            TimeRegions.Add(new TimeDisplayUnitViewModel() { TimeIndex = 2, FromTime = genFromTime(10, 30), ToTime = genFromTime(12, 00) });
            TimeRegions.Add(new TimeDisplayUnitViewModel() { TimeIndex = 3, FromTime = genFromTime(12, 50), ToTime = genFromTime(14, 20) });
            TimeRegions.Add(new TimeDisplayUnitViewModel() { TimeIndex = 4, FromTime = genFromTime(14, 30), ToTime = genFromTime(16, 00) });
            TimeRegions.Add(new TimeDisplayUnitViewModel() { TimeIndex = 5, FromTime = genFromTime(16, 10), ToTime = genFromTime(17, 40) });
            TimeRegions.Add(new TimeDisplayUnitViewModel() { TimeIndex = 6, FromTime = genFromTime(18, 00), ToTime = genFromTime(19, 30) });
            TimeRegions.Add(new TimeDisplayUnitViewModel() { TimeIndex = 7, FromTime = genFromTime(19, 40), ToTime = genFromTime(21, 10) });
        }
    }
    public class TimeDisplayUnitTimeConverter : IValueConverter
    {
        public TimeDisplayUnitTimeConverter()
        {
            
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is DateTime))
            {
                return "";
            }
            DateTime time = (DateTime) value;
            return string.Format("{0}:{1:D2}",time.Hour,time.Minute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return "";
        }
    }
}
