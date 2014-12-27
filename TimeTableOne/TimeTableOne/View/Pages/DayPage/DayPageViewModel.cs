using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.DayPage
{
    public class DayPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ScheduleData data;
        public ObservableCollection<TimeTableViewModel> Tables { get; set; }
        public DayPageViewModel()
        {
            Tables = new ObservableCollection<TimeTableViewModel>();
            TimeTableViewModel VM;
            for (int i = 1; i < 8; i++)
            {
                data = ApplicationData.Instance.GetSchedule(i, (int) DateTime.Now.DayOfWeek);
                if (data != null || data.TableName=="")
                {
                    VM = new TimeTableViewModel(new TableKey(i, DateTime.Now.DayOfWeek));
                    Tables.Add(VM);
                }
            }

            Background = new Uri(ApplicationData.Instance.Configuration.BackgroundImagePath);
        }
        public DispatcherTimer timer { get; set; }

        public Uri Background { get; set; }
    }

    public class DayPageViewModelInDesign : DayPageViewModel
    {
        public DayPageViewModelInDesign()
        {
            Background = new Uri("ms-appx:///Assets/OneNotePurple.png");
        }
    }
}
