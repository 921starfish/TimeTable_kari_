using System.Collections.ObjectModel;
using System.ComponentModel;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.TablePage
{
    public class TablePageViewModel : BasicViewModel
    {
        private TimeTableGridViewModel _timeTableDataContext;
        public TablePageViewModel()
        {
            TimeTableDataContext=new TimeTableGridViewModel();
        }

        public TimeTableGridViewModel TimeTableDataContext
        {
            get { return _timeTableDataContext; }
            set
            {
                if (Equals(value, _timeTableDataContext)) return;
                _timeTableDataContext = value;
                OnPropertyChanged();
            }
        }
    }

    public class TablePageViewModelInDesign : TablePageViewModel
    {
        public TablePageViewModelInDesign()
        {

        }
    }
}
