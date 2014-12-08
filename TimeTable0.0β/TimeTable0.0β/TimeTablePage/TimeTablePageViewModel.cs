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
        public TimeTablePageViewModel(TablePage page)
        {
            Page = page;
            Tables = new ObservableCollection<TimeTableViewModel>[7];
            for (int j = 0; j < 7; j++)
            {
                Tables[j]=new ObservableCollection<TimeTableViewModel>();
                for (int i = 1; i < 8; i++)
                {
                    Tables[j].Add(new TimeTableViewModel(Page, new TableKey(i, j+1)));
                }
            }
               
            PropertyChanged(this, new PropertyChangedEventArgs("Tables"));
        }
        public ObservableCollection<TimeTableViewModel> MonTables { get { return Tables[0]; } }
        public ObservableCollection<TimeTableViewModel> TueTables { get { return Tables[1]; } }
        public ObservableCollection<TimeTableViewModel> WedTables { get { return Tables[2]; } }
        public ObservableCollection<TimeTableViewModel> ThuTables { get { return Tables[3]; } }
        public ObservableCollection<TimeTableViewModel> FriTables { get { return Tables[4]; } }
        public ObservableCollection<TimeTableViewModel> SatTables { get { return Tables[5]; } }
        public ObservableCollection<TimeTableViewModel> SunTables { get { return Tables[6]; } }
        private ObservableCollection<TimeTableViewModel>[] Tables;
        public TablePage Page { get; set; }
    }
}
