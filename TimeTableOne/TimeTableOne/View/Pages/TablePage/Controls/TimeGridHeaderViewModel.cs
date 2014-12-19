using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public class TimeGridHeaderViewModel:BasicViewModel
    {
        public TimeGridHeaderViewModel(DayOfWeek dayofWeek)
        {
            _dayofWeek = dayofWeek;
        }

        private DayOfWeek _dayofWeek;
        public string Header { get; set; }

        public string HelpDisplay { get; set; }

        public SolidColorBrush TextBrush { get; set; }
    }

    public class TimeGridHeaderViewModelInDesign : TimeGridHeaderViewModel
    {
        public TimeGridHeaderViewModelInDesign():base(DayOfWeek.Sunday)
        {
            this.Header = "Sun";
            TextBrush=new SolidColorBrush(Colors.Red);
        }
    }
}
