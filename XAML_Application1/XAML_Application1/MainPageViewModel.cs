using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using Windows.UI.Xaml;
using XAML_Application1.Commands;

namespace XAML_Application1
{
    public class MainPageViewModel:INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            Times = new ObservableCollection<TimeControlViewModel>();
            Times.Add(new TimeControlViewModel(0));
            Times.Add(new TimeControlViewModel(3));
            Times.Add(new TimeControlViewModel(6));
            Times.Add(new TimeControlViewModel(9));
            TestCommand = new AlwaysExecutableDelegateCommand(() =>
            {
                Times.Add(new TimeControlViewModel(19));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ThisIsData));
                MemoryStream ms = new MemoryStream();
                xmlSerializer.Serialize(ms, new ThisIsData() { NameOfSomeOne = "Hosino", NumberOfSomething = 21898 });
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(ms);
                Debug.WriteLine(reader.ReadToEnd());
            });
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ObservableCollection<TimeControlViewModel> Times { get; set; }

        public ICommand TestCommand { get; set; }
        //private DispatcherTimer timer;
        //public MainPageViewModel()
        //{

        //    TestDisplay = 0;
        //    timer = new DispatcherTimer();
        //    timer.Interval = TimeSpan.FromMilliseconds(1000);
        //    timer.Tick += OnTick;
        //    timer.Start();
        //}

        //private void OnTick(object sender, object e)
        //{
        //    TestDisplay++;
        //    PropertyChanged(this, new PropertyChangedEventArgs("TestDisplay"));
        //}

        //public int TestDisplay
        //{
        //    get;
        //    set;
        //}

        
    }
}
