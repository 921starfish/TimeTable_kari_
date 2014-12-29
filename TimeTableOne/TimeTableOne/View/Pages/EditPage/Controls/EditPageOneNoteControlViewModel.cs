using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI.Xaml;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class EditPageOneNoteControlViewModel : BasicViewModel
    {
        public EditPageOneNoteControlViewModel()
        {
            NoteName = "NoteName";
            NoteColor = new SolidColorBrush((Color)Application.Current.Resources["OneNotePurpleColor"]);
            ForeColor=new SolidColorBrush(Colors.White);
          
        }

        public string NoteName { get; set; }
        public SolidColorBrush NoteColor { get; set; }
        public SolidColorBrush ForeColor { get; set; }
    }
}
