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

namespace TimeTableOne.View.Pages.EditPage.Controls.Popups
{
    public class AddAssignmentPopupViewModel:BasicViewModel
    {
        public AddAssignmentPopupViewModel()
        {
            _acceptCommand=new BasicCommand(OnAcceptAssignmentData,ValidateAssignmentData);
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
            assignment.DueTime = _dueDate;
            ApplicationData.Instance.Assignments.Add(assignment);
            ApplicationData.SaveData();
        }

        protected string _assignmentName;
        protected string _assignmentDetail;
        protected int _yearEdit;
        protected int _monthEdit;
        protected int _dayEdit;
        protected DateTime _dueDate;
        protected BasicCommand _acceptCommand;

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
                OnPropertyChanged(); _acceptCommand.NotifyCanExecuteChanged();

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

            }
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
