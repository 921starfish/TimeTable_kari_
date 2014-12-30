using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI.Xaml;
using TimeTableOne.Data;
using TimeTableOne.Utils;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class EditPageOneNoteControlViewModel : BasicViewModel
    {
        private SolidColorBrush _tableColor;
        private Brush _foreColor;
        public EditPageOneNoteControlViewModel()
        {
            NoteName = TableUnitDataHelper.GetCurrentSchedule().TableName;
            NoteColor = new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData); ;
        }

        public string NoteName { get; set; }
        public SolidColorBrush NoteColor
        {
            get { return _tableColor = new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData); }
            set
            {
                if (Equals(value, _tableColor)) return;
                _tableColor = value;
                OnPropertyChanged();
                ForeColor = null;
            }
        }
        public Brush ForeColor
        {
            get
            {
                return TableUnitDataHelper.GetCurrentSchedule().ColorData.Liminance() >= 0.5 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White);

            }
            set
            {
                _foreColor = value;
                OnPropertyChanged();
            }
        }
    }
}
