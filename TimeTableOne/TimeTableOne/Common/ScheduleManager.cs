
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Data;
using TimeTableOne.Utils;

namespace TimeTableOne.Common
{
    /// <summary>
    /// スケジュールに関する便利クラス
    /// </summary>
    public class ScheduleManager
    {
        private static ScheduleManager _instance;
        private TableKey _currentKey;

        public static ScheduleManager Instance
        {
            get
            {
                _instance = _instance ?? new ScheduleManager();
                return _instance;
            }
        }

        

        public ScheduleManager()
        {
            UpdateCurrentTable();
            TimerManager.Instance.GUITick += Instance_GUITick;
        }

        void Instance_GUITick(object sender, EventArgs e)
        {
            UpdateCurrentTable();
        }

        public event EventHandler<CurrentScheduleKeyChangedEventArgs> CurrentScheduleChanged = delegate { };

        private void UpdateCurrentTable()
        {
            var now =new DateTime(2015,1,1,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
            for (int i = 0; i < ApplicationData.Instance.TimeSpans.Count; i++)
            {
                var span = ApplicationData.Instance.TimeSpans[i];
                if ((now - span.FromTime).TotalMilliseconds > 0 && (span.ToTime - now).TotalMilliseconds > 0)
                {
                    if (CurrentKey == null || (CurrentKey.dayOfWeek != now.DayOfWeek || CurrentKey.TableNumber != i + 1))
                    {
                        CurrentKey = new TableKey(i + 1, ((int)DateTime.Now.DayOfWeek)%7);
                    }
                }
            }
        }

        public TableKey CurrentKey
        {
            get { return _currentKey; }
            set
            {
                if(_currentKey!=null&&_currentKey.Equals(value))return;
                var lastKey = _currentKey;
                _currentKey = value;
                CurrentScheduleChanged(this,new CurrentScheduleKeyChangedEventArgs(lastKey,value));
            }
        }
    }

    public class CurrentScheduleKeyChangedEventArgs : EventArgs
    {
        public CurrentScheduleKeyChangedEventArgs()
        {
            
        }

        public CurrentScheduleKeyChangedEventArgs(TableKey changedFrom, TableKey changedTo)
        {
            ChangedFrom = changedFrom;
            ChangedTo = changedTo;
        }

        public TableKey ChangedFrom { get;private set; }

        public TableKey ChangedTo { get; private set; }
    }
}
