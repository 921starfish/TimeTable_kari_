using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.EditPage.Controls.Units;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class ClassRoomControlViewModel:BasicViewModel
    {
        private SolidColorBrush _tableColor;
        private Brush _foreColor;
        public ClassRoomControlViewModel()
        {
            ClassRoomChanges=new ObservableCollection<ClassRoomChangeUnitViewModel>();
            var schedule = TableUnitDataHelper.GetCurrentSchedule();
            EditPageUpdateEvents.ColorUpdateEvent += () =>
            {
                this.TableColor = new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData);
                OnPropertyChanged();
            };
            if (schedule != null)
            {
                RecordFirstDate = schedule.CreationDate.ToString("yyyy年M月dd日からの記録");
                var key = TableUnitDataHelper.GetCurrentKey();
                DateTime current = DateTimeUtil.NextKeyDay(schedule.CreationDate, key.dayOfWeek);
                for (int i = 0; i < 100; i++)
                {
                    ClassRoomChanges.Add(new ClassRoomChangeUnitViewModel(current,
                        ApplicationData.Instance.GetClassRoomChangeSchedule(current, key),i));
                    current = current.AddDays(7);
                    if ((current - DateTime.Now) >TimeSpan.FromDays(60))
                    {
                        break;
                    }
                }
            }

            NoClasses = new ObservableCollection<NoClassRoomListUnitViewModel>();
            schedule = TableUnitDataHelper.GetCurrentSchedule();
            if (schedule != null)
            {
                var key = TableUnitDataHelper.GetCurrentKey();
                DateTime current = DateTimeUtil.NextKeyDay(schedule.CreationDate, key.dayOfWeek);
                for (int i = 0; i < 100; i++)
                {
                    NoClasses.Add(
                        new NoClassRoomListUnitViewModel(current, key));
                    current = current.AddDays(7);
                    if ((current - DateTime.Now) >TimeSpan.FromDays(60))
                    {
                        break;
                    }
                }
            }
        }

        public SolidColorBrush TableColor
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
        public string RecordFirstDate { get; set; }

        public ObservableCollection<ClassRoomChangeUnitViewModel> ClassRoomChanges { get; set; } 

        public ObservableCollection<NoClassRoomListUnitViewModel> NoClasses { get; set; } 
    }

    public class ClassRoomControlViewModelInDesign : ClassRoomControlViewModel
    {
        public ClassRoomControlViewModelInDesign()
        {
            RecordFirstDate = DateTime.Now.ToString("M月dd日からの記録");
        }
    }

    
}
