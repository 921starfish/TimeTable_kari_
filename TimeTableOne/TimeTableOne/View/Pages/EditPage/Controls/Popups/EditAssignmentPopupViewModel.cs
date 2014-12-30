using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.Utils.Commands;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace TimeTableOne.View.Pages.EditPage.Controls.Popups
{
    public class EditAssignmentPopupViewModel : BasicViewModel
    {
        public EditAssignmentPopupViewModel()
        {

            //this._assignmentName = "aa";
            //this._assignmentDetail = null;
            //this._yearEdit = 0;
            //this._monthEdit = 0;
            //this._dayEdit = 0;
            //this._dueDate = null;
            _acceptCommand = new BasicCommand(OnAcceptAssignmentData, ValidateAssignmentData);
            AllDelete = new AlwaysExecutableDelegateCommand(DeleteWithCheck);
            EditPageUpdateEvents.ColorUpdateEvent += () =>
            {
                TableColor = new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData);
                OnPropertyChanged();
            };
        }

        private bool ValidateAssignmentData()
        {
            return DateTimeUtil.IsDate(YearEdit, MonthEdit, DayEdit) && !String.IsNullOrWhiteSpace(_assignmentName);
        }

        private void OnAcceptAssignmentData()
        {
            //TODO ここでscheduleに対して代入をする
            _schedule.AssignmentName = AssignmentName;
            _schedule.AssignmentDetail = AssignmentDetail;
            _schedule.DueTime = DueDate;
            ApplicationData.SaveData();
        }

        protected string _assignmentName;
        protected string _assignmentDetail;
        protected int _yearEdit;
        protected int _monthEdit;
        protected int _dayEdit;
        protected DateTime _dueDate;
        protected BasicCommand _acceptCommand;
        private Brush _foreColor;
        private AssignmentSchedule _schedule;
        private SolidColorBrush _tableColor;

        public EditAssignmentPopupViewModel(AssignmentSchedule schedule)
        {
            _acceptCommand = new BasicCommand(OnAcceptAssignmentData, ValidateAssignmentData);
            //TODO ここでVMに反映されるようにする。
            _schedule = schedule;
            AssignmentName = schedule.AssignmentName;
            AssignmentDetail = schedule.AssignmentDetail;
            DueDate = schedule.DueTime;
            UpdateFromDueDate();
     
        }

        private void UpdateFromDueDate()
        {
            YearEdit = DueDate.Year;
            MonthEdit = DueDate.Month;
            DayEdit = DueDate.Day;
        }

        public string AssignmentName
        {
            get { return _assignmentName; }
            set
            {
                if (value == _assignmentName) return;
                _assignmentName = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();
            }
        }

        public string AssignmentDetail
        {
            get { return _assignmentDetail; }
            set
            {
                if (value == _assignmentDetail) return;
                _assignmentDetail = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();

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
                ForeColor = value.Color.Liminance() >= 0.5 ? new SolidColorBrush(Colors.Black) :
                new SolidColorBrush(Colors.White);
            }
        }
        public Brush ForeColor
        {
            get { return _foreColor; }
            set
            {
                if (Equals(value, _foreColor)) return;
                _foreColor = value;
                OnPropertyChanged();
            }
        }

        public int YearEdit
        {
            get { return _yearEdit; }
            set
            {
                if (value == _yearEdit) return;
                _yearEdit = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();

            }
        }

        public int MonthEdit
        {
            get { return _monthEdit; }
            set
            {
                if (value == _monthEdit) return;
                _monthEdit = value;
                OnPropertyChanged(); _acceptCommand.NotifyCanExecuteChanged();

            }
        }

        public int DayEdit
        {
            get { return _dayEdit; }
            set
            {
                if (value == _dayEdit) return;
                _dayEdit = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();

            }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                if (value.Equals(_dueDate)) return;
                _dueDate = value;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();
                UpdateFromDueDate();
            }
        }

        public BasicCommand AcceptCommand
        {
            get { return _acceptCommand; }
            set
            {
                if (Equals(value, _acceptCommand)) return;
                _acceptCommand = value;
                OnPropertyChanged();
            }
        }

        public AlwaysExecutableDelegateCommand AllDelete { get; set; }

        private async void DeleteWithCheck()
        {
            MessageDialog dlg = new MessageDialog("本当に削除しますか？");
            dlg.Commands.Add(new UICommand("はい"));
            dlg.Commands.Add(new UICommand("いいえ"));
            dlg.DefaultCommandIndex = 1;// いいえがデフォ
            var cmd = await dlg.ShowAsync();
            // いいえのとき
            if (cmd == dlg.Commands[1])
            {
                return;
            }
            else if (cmd == dlg.Commands[0])
            {
                this._assignmentName = "";
                this._assignmentDetail = "";
                this._yearEdit = 1;
                this._monthEdit = 1;
                this._dayEdit = 1;
                OnPropertyChanged();
                _acceptCommand.NotifyCanExecuteChanged();
            }

        }
    }
    public class EditAssignmentPopupViewModelInDesign : EditAssignmentPopupViewModel
    {
        public EditAssignmentPopupViewModelInDesign()
        {
            _assignmentName = "イカ娘の生体";
            _yearEdit = 2014;
            _monthEdit = 12;
            _dayEdit = 31;
            _assignmentDetail = "イカ娘学Ⅰの最終レポート";
        }
    }
}
