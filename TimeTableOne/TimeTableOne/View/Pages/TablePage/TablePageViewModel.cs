using System.Collections.ObjectModel;
using System.ComponentModel;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.TablePage
{
    public class TablePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public TablePageViewModel()
        {
            TimeTableDataContext=new TimeTableGridViewModel();
        }

        public TimeTableGridViewModel TimeTableDataContext { get; set; }
    }

    public class TablePageViewModelInDesign : TablePageViewModel
    {
        public TablePageViewModelInDesign()
        {

        }
    }
}
