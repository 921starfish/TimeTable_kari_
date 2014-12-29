using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public class ClassRoomChangeUnitViewModel:BasicViewModel
    {
        public ClassRoomChangeUnitViewModel()
        {
            
        }
        private string _supportText;
        private string _displayDate;
        private string _classRoomChangeCaption;
        private Brush _captionColor;
        private string _changeTo;
        private string _changeToEdit;

        public ClassRoomChangeUnitViewModel(DateTime current, ClassRoomChangeSchedule getClassRoomChangeSchedule,int i)
        {
            string[] supprtTexts = {"次回","次の次"};
            if (getClassRoomChangeSchedule != null)
            {

            }
            else
            {
                this.DisplayDate = current.ToString("yyyy年M月dd日");
                if (i < supprtTexts.Length)
                {
                    SupportText = supprtTexts[i];
                }
                CaptionColor = new SolidColorBrush(Colors.Green);
                ClassRoomChangeCaption = "通常";
                ChangeTo = "";
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

        public string ClassRoomChangeCaption
        {
            get { return _classRoomChangeCaption; }
            set
            {
                if (value == _classRoomChangeCaption) return;
                _classRoomChangeCaption = value;
                OnPropertyChanged();
            }
        }

        public Brush CaptionColor
        {
            get { return _captionColor; }
            set
            {
                if (Equals(value, _captionColor)) return;
                _captionColor = value;
                OnPropertyChanged();
            }
        }

        public string ChangeToEdit
        {
            get
            {
                return string.IsNullOrEmpty(_changeTo) ? "" : _changeTo;
            }
            set
            {
                // if (value == _changeTo) return;
                _changeTo = value;
                _changeToEdit = value;
                if (!string.IsNullOrEmpty(_changeTo))
                {
                    CaptionColor = new SolidColorBrush(Colors.DarkOrange);
                    ClassRoomChangeCaption = "教室変更";
                }
                else
                {
                    CaptionColor = new SolidColorBrush(Colors.Green);
                    ClassRoomChangeCaption = "通常";
                }
                OnPropertyChanged();
            }
        }

        public string ChangeTo
        {
            get
            {
                return string.IsNullOrEmpty(_changeTo) ? "\uE104" : _changeTo;
            }
            set
            {
                if(value == _changeTo) return;
                _changeTo = value;
                OnPropertyChanged();
            }
        }
    }

    public class ClassRoomChangeUnitViewModelInDesign : ClassRoomChangeUnitViewModel
    {
        public ClassRoomChangeUnitViewModelInDesign()
        {
            this.DisplayDate = DateTime.Now.ToString("yyyy年M月dd日");
            this.SupportText = "次回";
            CaptionColor = new SolidColorBrush(Colors.Yellow);
            ClassRoomChangeCaption = "教室変更";
            ChangeTo = "628教室";
        }
    }
}
