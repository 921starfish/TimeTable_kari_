﻿

using System;
using TimeTableOne.Data;
using TimeTableOne.Utils;

namespace TimeTableOne.Common
{
    /// <summary>
    ///     スケジュールに関する便利クラス
    /// </summary>
    public class ScheduleManager
    {
        private static ScheduleManager _instance;
        private TableKey _currentKey;

        /// <summary>
        ///     Instance of Singleton
        /// </summary>
        public static ScheduleManager Instance
        {
            get
            {
                _instance = _instance ?? new ScheduleManager();
                return _instance;
            }
        }


        private ScheduleManager()
        {
            UpdateCurrentTable();
            TimerManager.Instance.GUITick += Instance_GUITick;
        }
        /// <summary>
        /// タイマーによる現在のテーブルの選択の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Instance_GUITick(object sender, EventArgs e)
        {
            UpdateCurrentTable();
        }

        /// <summary>
        ///     現在の時間帯が変わったときに呼び出されるイベント
        /// </summary>
        public event EventHandler<CurrentScheduleKeyChangedEventArgs> CurrentScheduleChanged = delegate { };

        /// <summary>
        ///     現在の時間帯の再チェック
        /// </summary>
        private void UpdateCurrentTable()
        {
            var now = new DateTime(2015, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            for (var i = 0; i < ApplicationData.Instance.TimeSpans.Count; i++)
            {
                var span = ApplicationData.Instance.TimeSpans[i];
                if ((now - span.FromTime).TotalMilliseconds > 0 && (span.ToTime - now).TotalMilliseconds > 0)
                {
                    if (CurrentKey == null || (CurrentKey.dayOfWeek != now.DayOfWeek || CurrentKey.TableNumber != i + 1))
                    {
                        CurrentKey = new TableKey(i + 1, ((int) DateTime.Now.DayOfWeek)%7);
                    }
                }
            }
        }

        /// <summary>
        ///     DateTime.Nowが指す今のTableKey
        /// </summary>
        public TableKey CurrentKey
        {
            get { return _currentKey; }
            set
            {
                if (_currentKey != null && _currentKey.Equals(value)) return;
                var lastKey = _currentKey;
                _currentKey = value;
                CurrentScheduleChanged(this, new CurrentScheduleKeyChangedEventArgs(lastKey, value));
            }
        }
    }

    /// <summary>
    ///     現在の時間帯が変わった際にコールされるイベントのイベント引数
    /// </summary>
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

        /// <summary>
        ///     もともとのテーブルキー
        /// </summary>
        public TableKey ChangedFrom { get; private set; }

        /// <summary>
        ///     変移先のテーブルキー
        /// </summary>
        public TableKey ChangedTo { get; private set; }
    }
}
