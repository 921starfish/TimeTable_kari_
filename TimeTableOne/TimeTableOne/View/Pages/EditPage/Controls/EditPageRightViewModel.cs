using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Utils;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
   public class EditPageRightViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public EditPageRightViewModel()
        {
            Komidashi = "ここにテーブル名を入力してください";
        }
        public string Komidashi { get; set; }


        public int RecLength
        {
            get
            {
                return Komidashi.Length * 50;
            }
        }
    }
}
