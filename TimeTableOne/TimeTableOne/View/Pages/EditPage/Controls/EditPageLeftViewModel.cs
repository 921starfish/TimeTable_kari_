using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Utils;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
   public class EditPageLeftViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EditPageLeftViewModel(TableKey key)
        {

            ClassName = "テーブル名";
            this.TableNumber = key.TableNumber;
            this.WeekDayText = key.dayOfWeek.ToString();
            this.DetailText = WeekDayText + " " + TableNumber.ToString();
            colorResource = new ColorResource();
        }
        public string ClassName { get; set; }

        int TableNumber { get; set; }

        public string WeekDayText { get; set; }

        public string DetailText { get; set; }

        public ColorResource colorResource{get;set;}
    }
}
