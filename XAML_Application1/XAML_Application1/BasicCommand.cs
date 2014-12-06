using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace XAML_Application1
{
    public class BasicCommand:ICommand
    {

        public event EventHandler CanExecuteChanged;
        private DispatcherTimer timer;

        public bool isClickable;

        public BasicCommand()
        {
            isClickable = false;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += OnTick;
            timer.Start();
        }

        private void OnTick(object sender, object e)
        {
            isClickable = !isClickable;
            CanExecuteChanged(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return isClickable;
        }



        public void Execute(object parameter)
        {
            Debug.WriteLine("Pushed!!");
        }
    }
}
