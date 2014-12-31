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

    

        public static event Action ReloadOneNoteEvent;

        public static void ReloadOneNote()
        {
            Action handler = ReloadOneNoteEvent;
            if (handler != null)
            {
                handler();
            }
        }
    }
}
