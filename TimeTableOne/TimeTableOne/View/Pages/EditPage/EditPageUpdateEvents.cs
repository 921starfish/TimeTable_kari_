using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.View.Pages.EditPage
{
    public static class EditPageUpdateEvents
    {
        public delegate void ColorUpdateEventHandler();

        public static event ColorUpdateEventHandler ColorUpdateEvent;

        public static void OnColorUpdate()
        {
            ColorUpdateEventHandler handler = ColorUpdateEvent;
            if (handler != null)
            {
                handler();
            }
        }


    }
}
