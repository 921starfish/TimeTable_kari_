using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    class EditPageOneNoteControle : BasicViewModel
    {
        private SolidColorBrush _noteColor;
        public EditPageOneNoteControle()
        {
            
        }
        public SolidColorBrush NoteColor
        {
            get { return _noteColor; }
            set
            {
                if (Equals(value, _noteColor)) return;
                _noteColor = value;
                OnPropertyChanged();
            }
        }
        public SolidColorBrush ForeColor
        {
            get { return new SolidColorBrush(BackgroundColor.Color.Liminance() > 0.5 ? Colors.Black : Colors.White); }
        }
    }
}
