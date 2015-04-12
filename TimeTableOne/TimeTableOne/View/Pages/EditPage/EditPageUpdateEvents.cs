using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.View.Pages.EditPage
{
    public static class EditPageUpdateEvents
    {
     

        public static event Action ColorUpdateEvent;

        public static void OnColorUpdate()
        {
            Action handler = ColorUpdateEvent;
            if (handler != null)
            {
                handler();
            }
        }

        public static Action ReloadOneNoteAction;

        public static void ReloadOneNote()
        {
          
            if (ReloadOneNoteAction != null)
            {
                ReloadOneNoteAction();
            }

        }

        public static event Action AssignmentListUnitUpdateEvent;

        public static void OnUpdateAssignmentListUnit()
        {
            if (AssignmentListUnitUpdateEvent != null) AssignmentListUnitUpdateEvent();
        }
    }
}
