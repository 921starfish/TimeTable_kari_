using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace TimeTableOne.Common
{
    public class TimerManager
    {
        private static TimerManager _instance;

        public static TimerManager Instance
        {
            get
            {
                _instance = _instance ?? new TimerManager();
                return _instance;
            }
        }

        public TimerManager()
        {
            this.SchedulerTimer=new Timer(OnTick,null,0,1000);   
            this.GUITimer=new DispatcherTimer();
            GUITimer.Interval = TimeSpan.FromSeconds(60);
            GUITimer.Tick += GUITimer_Tick;
            GUITimer.Start();
        }

        void GUITimer_Tick(object sender, object e)
        {
            GUITick(this,new EventArgs());
        }

        private void OnTick(object state)
        {
            Tick(this,new EventArgs());
        }

        public Timer SchedulerTimer { get; private set; }

        public DispatcherTimer GUITimer { get; private set; }

        public event EventHandler Tick= delegate { };

        public event EventHandler GUITick = delegate { };

    }
}
