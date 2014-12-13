using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TimeTableOne.Utils;
using TimeTableOne.Data;
using TimeTableOne.View.Pages.EditPage.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using System.Threading.Tasks;

namespace TimeTableOne.View.Pages.EditPage
{
    class EditPageViewModel : INotifyPropertyChanged
    {
        private readonly TableKey _key;
        private TableKey _tableKey;
        private string _tableName = "";
        private string _weekDayText ="";
        private string _detailText ="";
        private string _komidashi ="";
        private string _placeInformation ="";
        private string _tableInformation ="";
        private ScheduleData _scheduleData;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        public EditPageViewModel(TableKey key)
        {
            _key = key;
            _scheduleData = ApplicationData.Instance.GetSchedule(key.NumberOfDay, key.TableNumber);
            if (_scheduleData == null)
            {
                _scheduleData = ScheduleData.GenerateEmpty();
                ScheduleKey keyModel = ScheduleKey.Generate(key.NumberOfDay, key.TableNumber, _scheduleData);
                ApplicationData.Instance.Keys.Add(keyModel);
                ApplicationData.Instance.Data.Add(_scheduleData);
            }
            else
            {
                
            Initialize();
            }

        }

        private void Initialize()
        {
            this.TableKey = _key;
            TableName = "テーブル名";
            this.TableNumber = _key.TableNumber;
            this.WeekDayText = _key.dayOfWeek.ToString();
            this.DetailText = WeekDayText + " " + TableNumber.ToString();

            Komidashi = "ここにテーブル名を入力してください";
            PlaceInformation = "場所を入力";
            TableInformation = "テーブルについての情報を入力してください";
        }

        public TableKey TableKey
        {
            get { return _tableKey; }
            set { _tableKey = value; }
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        int TableNumber { get; set; }

        public string WeekDayText
        {
            get { return _weekDayText; }
            set { _weekDayText = value; }
        }

        public string DetailText
        {
            get { return _detailText; }
            set { _detailText = value; }
        }

        public ScheduleData scheduleKey { get; set; }

        public string Komidashi
        {
            get { return _komidashi; }
            set
            {
                _komidashi = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Komidashi"));
                PropertyChanged(this, new PropertyChangedEventArgs("RecLength"));
            }
        }

        public string PlaceInformation
        {
            get { return _placeInformation; }
            set { _placeInformation = value; }
        }

        public string TableInformation
        {
            get { return _tableInformation; }
            set { _tableInformation = value; }
        }

        public int RecLength
        {
            get
            {
                return Komidashi.Length * 50;
            }
        }
    }
}
