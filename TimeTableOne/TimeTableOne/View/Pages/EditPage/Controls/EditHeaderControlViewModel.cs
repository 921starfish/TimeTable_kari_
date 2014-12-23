using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using TimeTableOne.Common;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.Utils.Commands;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class EditHeaderControlViewModel:BasicViewModel
    {
        
        private Brush _backgroundColor;
        private TableKey _tableKey;
        protected ScheduleData _scheduleData;
        private string _lectureNameForEdit;
        private string _placeNameForEdit;
        private ICommand _backToTablePageCommand;

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (Equals(value, _backgroundColor)) return;
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public EditHeaderControlViewModel(TableKey tableKey)
        {
            _tableKey = tableKey;
            _scheduleData = ApplicationData.Instance.GetSchedule(tableKey.NumberOfDay, tableKey.NumberOfDay);
            _scheduleData = _scheduleData ?? new ScheduleData();
            _lectureNameForEdit = _scheduleData.TableName;
            PlacePredictions=new ObservableCollection<string>();
            LectureNamePredictions=new ObservableCollection<string>();
            foreach (var scheduleData in ApplicationData.Instance.Data)
            {
                if (!String.IsNullOrEmpty(scheduleData.Place))
                {
                    PlacePredictions.Add(scheduleData.Place);
                }
            }
            foreach (var scheduleData in ApplicationData.Instance.Data)
            {
                if (!String.IsNullOrEmpty(scheduleData.TableName))
                {
                    LectureNamePredictions.Add(scheduleData.TableName);
                }
            }
            AutoCompleteFunction = AutoCompleteFunctionImpl;
            BackToTablePageCommand=new AlwaysExecutableDelegateCommand(() =>
            {
                ((Frame) Window.Current.Content).Navigate(typeof (TablePage.TablePage));
;            });

        }

        private bool AutoCompleteFunctionImpl(string inlist, string input)
        {
            return inlist.ToLower().Contains(input.ToLower());
        }

        public TableKey TableKey
        {
            get { return _tableKey; }
            set
            {
                if (Equals(value, _tableKey)) return;
                _tableKey = value;
                OnPropertyChanged();
            }
        }

        public  Brush WeekBrush
        {
            get { return TableKey.dayOfWeek.GetWeekColor(); }
        }

        public string WeekText
        {
            get { return WeekStringConverter.getAsStringInJpn(TableKey.dayOfWeek)+"曜日"; }
        }

        public string TimeText
        {
            get { return string.Format("{0}時限",TableKey.TableNumber); }
        }

        public string LectureName
        {
            get { return _scheduleData.TableName; }
        }

        public string LectureNameForEdit
        {
            get { return _scheduleData.TableName; }
            set
            {
                if (value == _lectureNameForEdit||String.IsNullOrEmpty(value)) return;
                _lectureNameForEdit = value;
                _scheduleData.TableName = value;
                ApplicationData.SaveData(ApplicationData.Instance);
                OnPropertyChanged("LectureName");
                OnPropertyChanged();
            }
        }
        public string PlaceForEdit
        {
            get { return _scheduleData.Place; }
            set
            {
                if (value == _placeNameForEdit || String.IsNullOrEmpty(value)) return;
                _placeNameForEdit = value;
                _scheduleData.Place = value;
                ApplicationData.SaveData(ApplicationData.Instance);
                OnPropertyChanged("Place");
                OnPropertyChanged();
            }
        }

        public ICommand BackToTablePageCommand
        {
            get { return _backToTablePageCommand; }
            set { _backToTablePageCommand = value; }
        }

        public string Place
        {
            get { return _scheduleData.Place; }
        }

        public ObservableCollection<string> PlacePredictions { get; set; }

        public ObservableCollection<string> LectureNamePredictions { get; set; } 

        public Func<string,string,bool> AutoCompleteFunction { get; set; } 

    }

    class EditHeaderControlViewModelInDesign : EditHeaderControlViewModel
    {
        public EditHeaderControlViewModelInDesign():base(new TableKey(1,DayOfWeek.Sunday))
        {
            _scheduleData=new ScheduleData()
            {
                Place = "624教室",
                TableName = "経済学Ⅰ"
            };
            BackgroundColor=new SolidColorBrush(Colors.Purple);
        }
    }
}
