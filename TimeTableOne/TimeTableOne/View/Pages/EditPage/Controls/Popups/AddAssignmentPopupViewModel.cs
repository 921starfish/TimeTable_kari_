using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeTableOne.Utils.Commands;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls.Popups
{
    public class AddAssignmentPopupViewModel:BasicViewModel
    {
        public AddAssignmentPopupViewModel()
        {
            _acceptButton=new BasicCommand(OnAcceptAssignmentData,ValidateAssignmentData);
        }

        private bool ValidateAssignmentData()
        {
            
        }

        private void OnAcceptAssignmentData()
        {
            
        }

        protected string _assignmentName;
        protected string _assignmentDetail;
        protected string _yearEdit;
        protected string _monthEdit;
        protected string _dayEdit;
        protected DateTime _dueDate;
        protected ICommand _acceptButton;

        public string AssignmentName
        {
            get { return _assignmentName; }
            set
            {
                if (value == _assignmentName) return;
                _assignmentName = value;
                OnPropertyChanged();
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
            }
        }

        public string YearEdit
        {
            get { return _yearEdit; }
            set
            {
                if (value == _yearEdit) return;
                _yearEdit = value;
                OnPropertyChanged();
            }
        }

        public string MonthEdit
        {
            get { return _monthEdit; }
            set
            {
                if (value == _monthEdit) return;
                _monthEdit = value;
                OnPropertyChanged();
            }
        }

        public string DayEdit
        {
            get { return _dayEdit; }
            set
            {
                if (value == _dayEdit) return;
                _dayEdit = value;
                OnPropertyChanged();
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
            }
        }

        public ICommand AcceptButton
        {
            get { return _acceptButton; }
            set
            {
                if (Equals(value, _acceptButton)) return;
                _acceptButton = value;
                OnPropertyChanged();
            }
        }
    }

    class AddAssignmentPopupViewModelInDesign : AddAssignmentPopupViewModel
    {
        public AddAssignmentPopupViewModelInDesign()
        {
            _assignmentName = "前期レポート課題";
            _yearEdit = "2015";
            _monthEdit = "12";
            _dayEdit = "25";
            _assignmentDetail = "計算数学のレポート課題";
        }
    }
}
