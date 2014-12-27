using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class ClassRoomControlViewModel:BasicViewModel
    {
        public ClassRoomControlViewModel()
        {
            
        }

        public string RecordFirstDate { get; set; }
    }

    public class ClassRoomControlViewModelInDesign : ClassRoomControlViewModel
    {
        public ClassRoomControlViewModelInDesign()
        {
            RecordFirstDate = DateTime.Now.ToString("M月dd日からの記録");
        }
    }
}
