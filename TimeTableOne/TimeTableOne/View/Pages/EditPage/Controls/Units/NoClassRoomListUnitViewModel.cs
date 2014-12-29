using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public class NoClassRoomListUnitViewModel:BasicViewModel
    {
        private readonly DateTime _time;
        private readonly TableKey _key;
        private string _displayDate;
        private string _supportText;
        private Brush _noClassStateBrush;
        private string _noClassStateText;
        public bool IsNoClass { get; set; }

        public NoClassRoomListUnitViewModel()
        {

        }

        public NoClassRoomListUnitViewModel(DateTime time,TableKey key)
        {
            _time = time;
            _key = key;
            this.DisplayDate = time.ToString("M月dd日");
            Update();
        }

        public void ChangeNoClassState(bool noclass)
        {
            if (noclass)
            {
                var sc = ApplicationData.Instance.GetNoClassSchedule(_time, _key);
                if (sc == null)
                {
                    NoClassSchedule nsc = new NoClassSchedule()
                    {
                        Day = _time,
                        ScheduleId = TableUnitDataHelper.GetCurrentSchedule().ScheduleId
                    };
                    ApplicationData.Instance.NoClassSchedules.Add(nsc);
                }
            }
            else
            {
                var sc = ApplicationData.Instance.GetNoClassSchedule(_time, _key);
                if (sc != null)
                {
                    ApplicationData.Instance.NoClassSchedules.Remove(sc);
                }
            }

            ApplicationData.SaveData();
            Update();
        }

        private void Update()
        {
            var sc = ApplicationData.Instance.GetNoClassSchedule(_time, _key);
            if (sc == null)
            {
                this.NoClassStateText = "通常";
                NoClassStateBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                NoClassStateBrush = new SolidColorBrush(Colors.Red);
                NoClassStateText = "休講";
            }
            IsNoClass=sc!=null;
        }

        public
            Brush NoClassStateBrush
        {
            get { return _noClassStateBrush; }
            set
            {
                if (Equals(value, _noClassStateBrush)) return;
                _noClassStateBrush = value;
                OnPropertyChanged();
            }
        }

        public string NoClassStateText
        {
            get { return _noClassStateText; }
            set
            {
                if (value == _noClassStateText) return;
                _noClassStateText = value;
                OnPropertyChanged();
            }
        }

        public string DisplayDate
        {
            get { return _displayDate; }
            set
            {
                if (value == _displayDate) return;
                _displayDate = value;
                OnPropertyChanged();
            }
        }

        public string SupportText
        {
            get { return _supportText; }
            set
            {
                if (value == _supportText) return;
                _supportText = value;
                OnPropertyChanged();
            }
        }
    }

    public class NoClassRoomListUnitViewModelInDesign : NoClassRoomListUnitViewModel
    {
        public NoClassRoomListUnitViewModelInDesign()
        {
            DisplayDate = DateTime.Now.ToString("M月dd日");
            SupportText = "次";
            NoClassStateBrush=new SolidColorBrush(Colors.Red);
            NoClassStateText = "休講";
        }
    }
}
