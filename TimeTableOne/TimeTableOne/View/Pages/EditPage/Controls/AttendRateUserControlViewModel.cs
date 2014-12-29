using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class AttendRateUserControlViewModel:BasicViewModel
    {

        public AttendRateUserControlViewModel()
        {
            
        }

        public int TotalLecture { get; set; }

        public int AttendLecture { get; set; }

        public double BarHeight
        {
            get { return (AttendLecture/(double) TotalLecture)*200; }
        }

        public string TextByDay
        {
            get { return string.Format("{0}日/{1}日", AttendLecture, TotalLecture); }
        }

        public string TextByPercentage
        {
            get { return string.Format("{0}%", (AttendLecture/(double) TotalLecture*100).ToString("###.0")); }
        }
    }

    class AttendRateUserControlViewModelInDesign : AttendRateUserControlViewModel
    {
        public AttendRateUserControlViewModelInDesign()
        {
            TotalLecture = 14;
            AttendLecture = 12;
        }
    }
}
