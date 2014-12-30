using System;
using System.Collections.ObjectModel;
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
    public class EditHeaderControlViewModel : BasicViewModel
    {
        private const string InitialLectureName = "（授業名をここに入力）";
        private const string InitialPlaceName = "（場所をここに入力）";
        private SolidColorBrush _backgroundColor;
        private TableKey _tableKey;
        protected ScheduleData _scheduleData;
        private string _lectureNameForEdit;
        private string _placeNameForEdit;
        private SolidColorBrush _basicForeground;
        private SolidColorBrush _tableColor;
        private Brush _foreColor;

      
        public SolidColorBrush TableColor
        {
            get { return _tableColor=new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData); }
            set
            {
                if (Equals(value, _tableColor)) return;
                _tableColor = value;
                OnPropertyChanged();
                ForeColor = value.Color.Liminance() >= 0.5 ? new SolidColorBrush(Colors.Black) :
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

        public EditHeaderControlViewModel(TableKey tableKey)
        {
            _tableKey = tableKey;
            _scheduleData = ApplicationData.Instance.GetSchedule(tableKey.NumberOfDay, tableKey.TableNumber);
            _scheduleData = _scheduleData ?? ScheduleData.GenerateEmpty();
            _lectureNameForEdit = _scheduleData.TableName;
            _backgroundColor = new SolidColorBrush(_scheduleData.ColorData);
            PlacePredictions = new ObservableCollection<string>();
            LectureNamePredictions = new ObservableCollection<string>();
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
            BackToTablePageCommand = new AlwaysExecutableDelegateCommand(() =>
            {
                PageUtil.MovePage(MainStaticPages.TablePage);
            });
            EditPageUpdateEvents.ColorUpdateEvent += () =>
            {
                this.TableColor = new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData);
            };
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

        public Brush WeekBrush
        {
            get { return TableKey.dayOfWeek.GetWeekColor(TableColor.Color.Liminance()>0.5); }
        }

        public string WeekText
        {
            get { return WeekStringConverter.getAsStringInJpn(TableKey.dayOfWeek) + "曜日"; }
        }

        public string TimeText
        {
            get { return string.Format("{0}時限", TableKey.TableNumber); }
        }

        private string ManagedLectureName
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_scheduleData.TableName) ? _scheduleData.TableName : InitialLectureName;
            }
        }

        private string ManagedPlace
        {
            get { return !string.IsNullOrWhiteSpace(_scheduleData.Place) ? _scheduleData.Place : InitialPlaceName; }
        }

        public string LectureName
        {
            get { return ManagedLectureName; }
        }

        public string LectureNameForEdit
        {
            get { return ManagedLectureName; }
            set
            {
                if (value == _lectureNameForEdit || String.IsNullOrEmpty(value)) return;       
                _lectureNameForEdit = value;
                if (value != InitialLectureName)
                {
                    _scheduleData.TableName = value;
                    ApplicationData.SaveData();
                }
                OnPropertyChanged("LectureName");
                OnPropertyChanged();
            }
        }

        public string PlaceForEdit
        {
            get { return ManagedPlace; }
            set
            {
                if (value == _placeNameForEdit || String.IsNullOrEmpty(value)) return;
                _placeNameForEdit = value;
                if (value != InitialPlaceName)
                {
                    _scheduleData.Place = value;
                    ApplicationData.SaveData();
                }
                OnPropertyChanged("Place");
                OnPropertyChanged();
            }
        }

        
        public ICommand BackToTablePageCommand { get; set; }

        public string Place
        {
            get { return ManagedPlace; }
        }

        public ObservableCollection<string> PlacePredictions { get; set; }

        public ObservableCollection<string> LectureNamePredictions { get; set; }

        public Func<string, string, bool> AutoCompleteFunction { get; set; }
    }

    internal class EditHeaderControlViewModelInDesign : EditHeaderControlViewModel
    {
        public EditHeaderControlViewModelInDesign() : base(new TableKey(1, DayOfWeek.Sunday))
        {
            _scheduleData = new ScheduleData
            {
                Place = "624教室",
                TableName = "経済学Ⅰ"
            };
    TableColor = new SolidColorBrush(Colors.Purple);
        }
    }
}
