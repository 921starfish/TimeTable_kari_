using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable0._0β.TimeTablePage
{
    class TimeTablePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public TimeTablePageViewModel()
        {
            Tables = new ObservableCollection<TimeTableViewModel>();
            for (int j = 1; j < 8;j++ )
                for (int i = 0; i < 7; i++)
                {
                    Tables.Add(new TimeTableViewModel(i, j));
                }
            
        }

        public ObservableCollection<TimeTableViewModel> Tables { get; set; }
    }
}
