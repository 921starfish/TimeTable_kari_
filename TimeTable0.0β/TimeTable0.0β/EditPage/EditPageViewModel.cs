using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable0._0β.EditPage
{
    class EditPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EditPageViewModel(int n,DayOfWeek w)
        {
            this.dayOfWeek = w;
            this.TableNumber = n;
        }

        public DayOfWeek dayOfWeek { get; set; }
        public int TableNumber { get; set; }
        
    }
}
