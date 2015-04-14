using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Data;
using TimeTableOne.View.Pages.EditPage.Controls.Units;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage
{
    public class AssignmentControlViewModel:BasicViewModel
    {
        public AssignmentControlViewModel()
        {
            EditPageUpdateEvents.AssignmentListUnitUpdateEvent += UpdateData;
            UpdateData();
        }

        public void UpdateData()
        {
            var pAssignments = new List<AssignmentListUnitViewModel>();
            var assignments = ApplicationData.Instance.GetAssignments(TableUnitDataHelper.GetCurrentSchedule());
            foreach (var assignmentSchedule in assignments)
            {
                pAssignments.Add(new AssignmentListUnitViewModel(assignmentSchedule));
            }
            var comparator = new AssignmentListUnitViewModelComparator();
            pAssignments.Sort(comparator);
            Assignments = new ObservableCollection<AssignmentListUnitViewModel>(pAssignments);
            OnPropertyChanged("Assignments");
        }

        public ObservableCollection<AssignmentListUnitViewModel> Assignments { get; set; }

        private class AssignmentListUnitViewModelComparator : IComparer<AssignmentListUnitViewModel>
        {
            public int Compare(AssignmentListUnitViewModel x, AssignmentListUnitViewModel y)
            {
                double xelapsed = ToDaysScaling(x._schedule.DueTime);
                double yelapsed = ToDaysScaling(y._schedule.DueTime);
                xelapsed += x._schedule.IsCompleted ? 3650000 : 0;
                yelapsed += y._schedule.IsCompleted ? 3650000 : 0;
                return (int) (xelapsed - yelapsed);
            }

            private double ToDaysScaling(DateTime time)
            {
                var elapsed = time - DateTime.MinValue;
                return elapsed.TotalDays;
            }
        }
    }

    class AssignmentControlViewModelInDesign : AssignmentControlViewModel
    {
        public AssignmentControlViewModelInDesign()
        {
            Assignments.Add(new AssignmentListUnitViewModel(){AssignmentName = "TestTest",DueDateInformation = "残り3日"});
            Assignments.Add(new AssignmentListUnitViewModel() { AssignmentName = "TestTest", RemainingDateInformation = "残り3日" });
            Assignments.Add(new AssignmentListUnitViewModel() { AssignmentName = "TestTest", RemainingDateInformation = "残り3日" });
            Assignments.Add(new AssignmentListUnitViewModel() { AssignmentName = "TestTest", RemainingDateInformation = "残り3日" });
        }
    }
}
