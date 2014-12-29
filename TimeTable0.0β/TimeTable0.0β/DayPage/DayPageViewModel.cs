using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable0._0β.TimeTablePage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimeTable0._0β.DayPage
{
    class DayPageViewModel : INotifyPropertyChanged
    {
         public event PropertyChangedEventHandler PropertyChanged;
         public ObservableCollection<TimeTableViewModel> Tables{get;set;}
         public DayPageViewModel(Page page)
         {
             Tables = new ObservableCollection<TimeTableViewModel>();
             TimeTableViewModel VM;
             for (int i = 1; i < 8; i++)
             {
                 VM = new TimeTableViewModel(page, new TableKey(i, DateTime.Now.DayOfWeek));
                 VM.Hight *= 2;
                 VM.Width *= 2;
                 Tables.Add(VM);
             }
         }
         public DispatcherTimer timer { get; set; }
    }
}
