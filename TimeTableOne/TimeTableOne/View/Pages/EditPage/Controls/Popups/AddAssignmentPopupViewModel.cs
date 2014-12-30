using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.Utils.Commands;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.View.Pages.EditPage.Controls.Popups
{
    public class AddAssignmentPopupViewModel:BasicViewModel
    {
        public AddAssignmentPopupViewModel()
        {
            _acceptCommand=new BasicCommand(OnAcceptAssignmentData,ValidateAssignmentData);
            initializeAsToday();
            EditPageUpdateEvents.ColorUpdateEvent += () =>
            {
                this.TableColor = new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData);
                OnPropertyChanged();
            };
           
        }

        private void initializeAsToday()
        {
            DueDate=DateTime.Now.AddDays(7);
            updateDuedateForEdit();

        }

        private bool ValidateAssignmentData()
        {
            return DateTimeUtil.IsDate(YearEdit, MonthEdit, DayEdit) && !String.IsNullOrWhiteSpace(_assignmentName);
         
        }

        private void OnAcceptAssignmentData()
        {
            var current=TableUnitDataHelper.GetCurrentSchedule();
            var assignment = current.GenerateAssignmentEmpty();
            assignment.AssignmentName = _assignmentName;
            assignment.AssignmentDetail = _assignmentDetail;
            assignment.DueTime = new DateTime(YearEdit,MonthEdit,DayEdit,0,0,0,0);
            ApplicationData.Instance.Assignments.Add(assignment);
            ApplicationData.SaveData();
           AssignmentControl.NotifyPopupClose();
        }

        protected string _assignmentName;
        protected string _assignmentDetail;
        protected int _yearEdit;
        protected int _monthEdit;
        protected int _dayEdit;
        protected DateTime _dueDate;
        protected BasicCommand _acceptCommand;
        private SolidColorBrush _tableColor;
        private Brush _foreColor;


        private void updateDuedateForEdit()
        {
            var d = DueDate;
            YearEdit= d.Year;
            MonthEdit = d.Month;
            DayEdit = d.Day;
        }
       

        public string AssignmentName
        {
            get { return _assignmentName; }
            set
            {
                if (value == _assignmentName) return;
                _assignmentName = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();
            }
        }

        public string AssignmentDetail
        {
            get { return _assignmentDetail; }
            set
            {
                if (value == _assignmentDetail) return;
                _assignmentDetail = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();

            }
        }

        public int YearEdit
        {
            get { return _yearEdit; }
            set
            {
                if (value == _yearEdit) return;
                _yearEdit = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();

            }
        }

        public int MonthEdit
        {
            get { return _monthEdit; }
            set
            {
                if (value == _monthEdit) return;
                _monthEdit = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();

            }
        }

        public int DayEdit
        {
            get { return _dayEdit; }
            set
            {
                if (value == _dayEdit) return;
                _dayEdit = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();

            }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                if (value.Equals(_dueDate)) return;
                _dueDate = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();
                updateDuedateForEdit();
            }
        }
        public SolidColorBrush TableColor
        {
            get { return new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData); ; }
            set { }
        }
        public Brush ForeColor
        {
            get
            {
                return TableUnitDataHelper.GetCurrentSchedule().ColorData.Liminance() >= 0.5
                    ? new SolidColorBrush(Colors.Black)
                    : new SolidColorBrush(Colors.White);
            }
            set { }
        }

        public BasicCommand AcceptCommand
        {
            get { return _acceptCommand; }
            set
            {
                if (Equals(value, _acceptCommand)) return;
                _acceptCommand = value;
                OnPropertyChanged();
            }
        }
    }

    class AddAssignmentPopupViewModelInDesign : AddAssignmentPopupViewModel
    {
        public AddAssignmentPopupViewModelInDesign()
        {
            _assignmentName = "前期レポート課題";
            _yearEdit = 2015;
            _monthEdit = 12;
            _dayEdit = 25;
            _assignmentDetail = "計算数学のレポート課題";
        }
    }
}
