using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.DayPage
{
    class DayPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TimeTableViewModel> Tables { get; set; }
        public DayPageViewModel(Page page)
        {
            Tables = new ObservableCollection<TimeTableViewModel>();
            colorResource = new ColorResource();
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

        public ColorResource colorResource { get; set; }
    }
}
