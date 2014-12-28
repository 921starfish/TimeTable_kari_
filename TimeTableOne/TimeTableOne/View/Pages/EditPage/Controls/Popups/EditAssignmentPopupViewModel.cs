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

namespace TimeTableOne.View.Pages.EditPage.Controls.Popups
{
    public class EditAssignmentPopupViewModel : BasicViewModel
    {
        public EditAssignmentPopupViewModel()
        {

            this._assignmentName = "aa";
            //this._assignmentDetail = null;
            //this._yearEdit = 0;
            //this._monthEdit = 0;
            //this._dayEdit = 0;
            //this._dueDate = null;
            _acceptCommand = new BasicCommand(OnAcceptAssignmentData, ValidateAssignmentData);
            AllDelete = new AlwaysExecutableDelegateCommand(DeleteWithCheck);
        }

        private bool ValidateAssignmentData()
        {
            return DateTimeUtil.IsDate(YearEdit, MonthEdit, DayEdit) && !String.IsNullOrWhiteSpace(_assignmentName);
        }

        private void OnAcceptAssignmentData()
        {
            var current = TableUnitDataHelper.GetCurrentSchedule();
            var assignment = current.GenerateAssignmentEmpty();
            assignment.AssignmentName = _assignmentName;
            assignment.AssignmentDetail = _assignmentDetail;
            assignment.DueTime = _dueDate;
            ApplicationData.Instance.Assignments.Add(assignment);
            ApplicationData.SaveData();
        }

        protected string _assignmentName;
        protected string _assignmentDetail;
        protected int _yearEdit;
        protected int _monthEdit;
        protected int _dayEdit;
        protected DateTime _dueDate;
        protected BasicCommand _acceptCommand;

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
