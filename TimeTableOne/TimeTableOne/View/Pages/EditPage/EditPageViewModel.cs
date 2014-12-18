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
using TimeTableOne.Utils.Commands;

namespace TimeTableOne.View.Pages.EditPage
{
    class EditPageViewModel : INotifyPropertyChanged
    {
        private readonly TableKey _key;
        private TableKey _tableKey;
        private string _tableName = "";
        private string _weekDayText = "";
        private string _detailText = "";
        private string _komidashi = "";
        private string _placeInformation = "";
        private string _tableInformation = "";
        private ScheduleData _scheduleData;

        public ScheduleData ScheduleData
        {
            get { return _scheduleData; }
        }
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
            AllDelete = new AlwaysExecutableDelegateCommand(
            () =>
            {
                this._detailText = "";
                this._komidashi = "";
                this._tableName = "";
                this._placeInformation = "";
                this._tableInformation = "";
                PropertyChanged(this, new PropertyChangedEventArgs("TableName"));
                PropertyChanged(this, new PropertyChangedEventArgs("DetailText"));
                PropertyChanged(this, new PropertyChangedEventArgs("Komidashi"));
                PropertyChanged(this, new PropertyChangedEventArgs("RecLength"));
                PropertyChanged(this, new PropertyChangedEventArgs("PlaceInfomation"));
                PropertyChanged(this, new PropertyChangedEventArgs("TableInfomation"));
                _scheduleData.TableName = TableName;
                _scheduleData.Place = PlaceInformation;
                _scheduleData.Description = DetailText;
                _scheduleData.FreeFormText = TableInformation;
              
            });
            OpenOneNote = new AlwaysExecutableDelegateCommand(
           () =>
           {
               OneNoteControl.open(_scheduleData.TableName);
           });
        }

        private void Initialize()
        {
            this.TableKey = _key;
            this.TableNumber = _key.TableNumber;
            this.WeekDayText = _key.dayOfWeek.ToString();
            this.DetailText = _scheduleData.Description;
            this.Komidashi = _scheduleData.TableName;
            this.PlaceInformation = _scheduleData.Place;
            this.TableInformation = _scheduleData.FreeFormText;
            this.TableName = _scheduleData.TableName;
        }


        public void saveData()
        {
            if (tableNameEdited)
            {
                _scheduleData.TableName = TableName;
            }
            if (placeEdited)
            {
                _scheduleData.Place = PlaceInformation;
            }
            if (detailTextEdited)
            {
                _scheduleData.Description = DetailText;
            }
            if (freeTextEdited)
            {
                _scheduleData.FreeFormText = TableInformation;
            }
        }

        private bool tableNameEdited;
        private bool placeEdited;
        private bool freeTextEdited;
        private bool detailTextEdited;
        public TableKey TableKey { get; set; }

        int TableNumber { get; set; }

        public string WeekDayText { get; set; }

        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TableName"));
            }
        }

        public string DetailText
        {
            get { return _detailText; }
            set
            {
                if (value == "")
                {
                    _detailText = _key.dayOfWeek.ToString() + " " + _key.TableNumber.ToString();
                    detailTextEdited = false;
                }
                else
                {
                    _detailText = value;
                    detailTextEdited = true;
                }
                PropertyChanged(this, new PropertyChangedEventArgs("DetailText"));
            }
        }


        public string Komidashi
        {
            get { return _komidashi; }
            set
            {
                if (value == "")
                {
                    TableName = "";
                    tableNameEdited = false;

                }
                else
                {
                    _komidashi = value;
                    TableName = value;
                    tableNameEdited = true;
                }

                PropertyChanged(this, new PropertyChangedEventArgs("Komidashi"));
                PropertyChanged(this, new PropertyChangedEventArgs("RecLength"));
            }
        }

        public string PlaceInformation
        {
            get { return _placeInformation; }
            set
            {
                if (value == "")
                {
                    placeEdited = false;
                }
                else
                {
                    _placeInformation = value;
                    placeEdited = true;
                }
                PropertyChanged(this, new PropertyChangedEventArgs("PlaceInfomation"));
            }
        }

        public string TableInformation
        {
            get { return _tableInformation; }
            set
            {
                if (value == "")
                {
                    freeTextEdited = false;
                }
                else
                {
                    _tableInformation = value;
                    freeTextEdited = true;
                }
                PropertyChanged(this, new PropertyChangedEventArgs("PlaceInfomation"));
            }
        }

        public int RecLength
        {
            get
            {
                if (_tableName == "")
                {
                    return 18*50;
                }
                else
                {
                    return Komidashi.Length * 50 + 50;
                }
              
            }
        }

        public AlwaysExecutableDelegateCommand AllDelete { get; set; }

        public AlwaysExecutableDelegateCommand OpenOneNote { get; set; }
    
    }
}
