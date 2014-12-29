using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;
using TimeTableOne.View.Pages.DayPage.Controls;
using TimeTableOne.Utils.Commands;

namespace TimeTableOne.View.Pages.DayPage
{
    public class DayPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ScheduleData data;
        public ObservableCollection<DayPageTableViewModel> Tables { get; set; }
        public DayPageViewModel()
        {
            Tables = new ObservableCollection<DayPageTableViewModel>();
            DayPageTableViewModel VM;
            for (int i = 1; i <= ApplicationData.Instance.TimeSpans.Count; i++)
            {

                data = ApplicationData.Instance.GetSchedule((int)DateTime.Now.DayOfWeek, i);
                if (data != null)
                {
                    if (data.TableName != "")
                    {
                        VM = new DayPageTableViewModel(new TableKey(i, DateTime.Now.DayOfWeek));
                        Tables.Add(VM);
                    }
                }
            }
            Background = new Uri(ApplicationData.Instance.Configuration.BackgroundImagePath);
        }

       
        public DispatcherTimer timer { get; set; }

        public Uri Background { get; set; }

        public AlwaysExecutableDelegateCommand BackToTablePageCommand { get; set; }
    }

    public class DayPageViewModelInDesign : DayPageViewModel
    {
        public DayPageViewModelInDesign()
        {
            Background = new Uri("ms-appx:///Assets/Background(1).jpg");
        }
    }
}
