using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public class AssignmentListUnitViewModel:BasicViewModel
    {
        private readonly AssignmentSchedule _schedule;

        public AssignmentListUnitViewModel()
        {
            
        }

        public AssignmentListUnitViewModel(AssignmentSchedule schedule)
        {
            _schedule = schedule;
            updatAssignmentStatus();
        }

        private string _assignmentName;
        private string _dueDateInformation;
        private string _remainingDateInformation;
        private string _assignmentStatus;
        private Brush _assignmentStatusColor;

        private void updatAssignmentStatus()
        {
            if(_schedule==null)return;
            if (_schedule.IsCompleted)
            {
                AssignmentStatusColor=new SolidColorBrush(Colors.GreenYellow);
            }
            else
            {
                DateTime dueTime = _schedule.DueTime;

            }
        }

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

        public string DueDateInformation
        {
            get { return _dueDateInformation; }
            set
            {
                if (value == _dueDateInformation) return;
                _dueDateInformation = value;
                OnPropertyChanged();
            }
        }

        public string RemainingDateInformation
        {
            get { return _remainingDateInformation; }
            set
            {
                if (value == _remainingDateInformation) return;
                _remainingDateInformation = value;
                OnPropertyChanged();
            }
        }

        public string AssignmentStatus
        {
            get { return _assignmentStatus; }
            set
            {
                if (value == _assignmentStatus) return;
                _assignmentStatus = value;
                OnPropertyChanged();
            }
        }

        public Brush AssignmentStatusColor
        {
            get { return _assignmentStatusColor; }
            set
            {
                if (Equals(value, _assignmentStatusColor)) return;
                _assignmentStatusColor = value;
                OnPropertyChanged();
            }
        }
    }

    class AssignmentListUnitViewModelInDesign : AssignmentListUnitViewModel
    {
        public AssignmentListUnitViewModelInDesign()
        {
            this.AssignmentName = "後期レポート";
            this.RemainingDateInformation = "残り1日";
            this.AssignmentStatusColor = new SolidColorBrush(Colors.Yellow);
            this.AssignmentStatus = "未完了";
        }
    }
}
