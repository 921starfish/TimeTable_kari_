using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private string _assignmentName;
        private string _dueDateInformation;
        private string _remainingDateInformation;

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
    }

    class AssignmentListUnitViewModelInDesign : AssignmentListUnitViewModel
    {
        public AssignmentListUnitViewModelInDesign()
        {
            this.AssignmentName = "後期レポート";
            this.RemainingDateInformation = "残り1日";
        }
    }
}
