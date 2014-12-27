using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public class NoClassRoomListUnitViewModel:BasicViewModel
    {
        private string _displayDate;
        private string _supportText;
        private Brush _noClassStateBrush;
        private string _noClassStateText;

        public NoClassRoomListUnitViewModel()
        {
            
        }

        public NoClassRoomListUnitViewModel(DateTime time,NoClassSchedule sc, TableKey key)
        {
            this.DisplayDate = time.ToString("M月dd日");
            if (sc == null)
            {
                this.NoClassStateText = "通常";
                NoClassStateBrush=new SolidColorBrush(Colors.Green);
            }
            else
            {
                NoClassStateBrush = new SolidColorBrush(Colors.Red);
                NoClassStateText = "休講";
            }
        }

        public
            Brush NoClassStateBrush
        {
            get { return _noClassStateBrush; }
            set
            {
                if (Equals(value, _noClassStateBrush)) return;
                _noClassStateBrush = value;
                OnPropertyChanged();
            }
        }

        public string NoClassStateText
        {
            get { return _noClassStateText; }
            set
            {
                if (value == _noClassStateText) return;
                _noClassStateText = value;
                OnPropertyChanged();
            }
        }

        public string DisplayDate
        {
            get { return _displayDate; }
            set
            {
                if (value == _displayDate) return;
                _displayDate = value;
                OnPropertyChanged();
            }
        }

        public string SupportText
        {
            get { return _supportText; }
            set
            {
                if (value == _supportText) return;
                _supportText = value;
                OnPropertyChanged();
            }
        }
    }

    public class NoClassRoomListUnitViewModelInDesign : NoClassRoomListUnitViewModel
    {
        public NoClassRoomListUnitViewModelInDesign()
        {
            DisplayDate = DateTime.Now.ToString("M月dd日");
            SupportText = "次";
            NoClassStateBrush=new SolidColorBrush(Colors.Red);
            NoClassStateText = "休講";
        }
    }
}
