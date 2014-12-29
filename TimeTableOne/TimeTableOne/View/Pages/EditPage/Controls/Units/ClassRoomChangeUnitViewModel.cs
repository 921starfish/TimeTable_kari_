using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public class ClassRoomChangeUnitViewModel:BasicViewModel
    {
        private readonly DateTime _current;
        private readonly ClassRoomChangeSchedule _getClassRoomChangeSchedule;

        public ClassRoomChangeUnitViewModel()
        {
            
        }
        private string _supportText;
        private string _displayDate;
        private string _classRoomChangeCaption;
        private Brush _captionColor;
        private string _changeTo;

        public ClassRoomChangeUnitViewModel(DateTime current, ClassRoomChangeSchedule getClassRoomChangeSchedule,int i)
        {
            _current = current;
            _getClassRoomChangeSchedule = getClassRoomChangeSchedule;
            string[] supprtTexts = {"次回","次の次"};
            if (getClassRoomChangeSchedule != null)
            {
                ChangeTo = getClassRoomChangeSchedule.ChangedTo;
            }
            else
            {
                CaptionColor = new SolidColorBrush(Colors.Green);
                ClassRoomChangeCaption = "通常";
                ChangeTo = "";
            }
            this.DisplayDate = current.ToString("yyyy年M月dd日");
            if (i < supprtTexts.Length)
            {
                SupportText = supprtTexts[i];
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

        public string ChangeTo
        {
            get
            {
                return string.IsNullOrEmpty(_changeTo) ? "\uE104" : _changeTo;
            }
            set
            {
                // if (value == _changeTo) return;
                _changeTo = value;
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

        public void ApplyChangeStatus()
        {
            var currentKey = TableUnitDataHelper.GetCurrentKey();
            var currentChangeData = ApplicationData.Instance.GetClassRoomChangeSchedule(_current, currentKey);
            if (string.IsNullOrWhiteSpace(_changeTo))
            {
                if (currentChangeData != null)
                {
                    ApplicationData.Instance.ClassRoomChanges.Remove(currentChangeData);
                    ApplicationData.SaveData();
                }
            }
            else
            {
                if (currentChangeData != null)
                {
                    ApplicationData.Instance.ClassRoomChanges.Remove(currentChangeData);
                    ApplicationData.SaveData();
                }
                var newData = ClassRoomChangeSchedule.Generagte(_current, TableUnitDataHelper.GetCurrentSchedule(),
                    _changeTo);
                ApplicationData.Instance.ClassRoomChanges.Add(newData);
                ApplicationData.SaveData();
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
