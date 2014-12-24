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
            Assignments=new ObservableCollection<AssignmentListUnitViewModel>();
            var assignments = ApplicationData.Instance.GetAssignments(TableUnitDataHelper.GetCurrentSchedule());
            foreach (var assignmentSchedule in assignments)
            {
                Assignments.Add(new AssignmentListUnitViewModel()
                {
                    AssignmentName = assignmentSchedule.AssignmentName,
                    
                });
            }
        }
        public ObservableCollection<AssignmentListUnitViewModel> Assignments { get; set; } 
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
