using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TimeTableOne.Common;
using TimeTableOne.Data;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public class TimeGridHeaderViewModel:BasicViewModel
    {
        public TimeGridHeaderViewModel(DayOfWeek dayofWeek)
        {
            _dayofWeek = dayofWeek;
            Header = WeekStringConverter.getAsString(dayofWeek);
            GenerateHelpDisplay();
            Width = TableLayoutManager.getElementWidth(ApplicationData.Instance.Configuration.TableTypeSetting);
        }

        private void GenerateHelpDisplay()
        {
            DayOfWeek today = DateTime.Now.DayOfWeek;
            if (today == _dayofWeek)
            {
                HelpDisplay = "今日";
            }
            int diff = ((int) today - (int) _dayofWeek)%7;
            if (diff == 6)
            {
                HelpDisplay = "明日";
            }else if (diff == 1)
            {
                HelpDisplay = "昨日";
            }else if (diff == 5)
            {
                HelpDisplay = "明後日";
            }
        }

        private DayOfWeek _dayofWeek;
        public string Header { get; set; }

        public string HelpDisplay { get; set; }

        public int Width { get; set; }

        public SolidColorBrush TextBrush { get; set; }
    }

    public class TimeGridHeaderViewModelInDesign : TimeGridHeaderViewModel
    {
        public TimeGridHeaderViewModelInDesign():base(DayOfWeek.Sunday)
        {
            HelpDisplay = "きょう";
            this.Header = "Sun";
            TextBrush=new SolidColorBrush(Colors.Red);
        }
    }
}
