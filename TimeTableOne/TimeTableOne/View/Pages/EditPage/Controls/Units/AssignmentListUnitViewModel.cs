using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils.Commands;
using TimeTableOne.View.Pages.EditPage.Controls.Popups;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.Networking.Connectivity;
using OneNoteControl;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public class AssignmentListUnitViewModel:BasicViewModel
    {
        public readonly AssignmentSchedule _schedule;

        public AssignmentListUnitViewModel()
        {
            CompleteCommand = new AlwaysExecutableDelegateCommand(Completed);
            OneNoteCommand = new AlwaysExecutableDelegateCommand(Onenote);
            EditAssignmentPopupData = new EditAssignmentPopupViewModel();
        }

        public AssignmentListUnitViewModel(AssignmentSchedule schedule)
        {
            _schedule = schedule;
            AssignmentDetail = schedule.AssignmentDetail;
            if (string.IsNullOrWhiteSpace(AssignmentDetail)) AssignmentDetail = "(説明はありません。)";
            AssignmentName =_assignmentName = schedule.AssignmentName ;
            updatAssignmentStatus();
            CompleteCommand=new AlwaysExecutableDelegateCommand(Completed);
            OneNoteCommand = new AlwaysExecutableDelegateCommand(Onenote);
            EditAssignmentPopupData = new EditAssignmentPopupViewModel();
            EditAssignmentPopupData.AssignmentName = this.AssignmentName;
            EditAssignmentPopupData.AssignmentDetail = this.AssignmentDetail;

        }

        private async void Completed()
        {
            MessageDialog dlg = new MessageDialog("課題「"+AssignmentName+"」を完了に設定します");
            dlg.Commands.Add(new UICommand("はい"));
            dlg.Commands.Add(new UICommand("いいえ"));
            dlg.DefaultCommandIndex = 1;
            var cmd = await dlg.ShowAsync();
            if(cmd == dlg.Commands[0])
            {
                _schedule.IsCompleted = true;
            }
            else if (cmd == dlg.Commands[1])
            {
                _schedule.IsCompleted = false;
            }
            updatAssignmentStatus();
        }


        private async void Onenote()
        {
            
                var data = NetworkInformation.GetInternetConnectionProfile();
                if (data == null) return;
                if (data.IsWlanConnectionProfile || data.IsWwanConnectionProfile)
                {
                    if (await OneNoteControler.Current.IsExistNotebook(TableUnitDataHelper.GetCurrentSchedule().TableName))
                    {
                        OneNoteControl.OneNoteControler.Current.OpenNewSection(TableUnitDataHelper.GetCurrentSchedule().TableName, AssignmentName);
                    }
                    else
                    {
                        MessageDialog dlg = new MessageDialog("関連付けられたノートが存在しません。先にノートを追加してください。");
                        dlg.Commands.Add(new UICommand("はい"));
                        dlg.DefaultCommandIndex = 0;
                        try
                        {
                            var cmd = await dlg.ShowAsync();
                            if (cmd == dlg.Commands[0])
                            {
                                return;
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            return;
                        }

                    }
                }
            


        }



         

        private string _assignmentName;
        private string _dueDateInformation;
        private string _remainingDateInformation;
        private string _assignmentStatus;
        private Brush _assignmentStatusColor;
        private string _assignmentDetail;
        private AddAssignmentPopupViewModel _assignmentPopupData;
        private EditAssignmentPopupViewModel _editAssignmentPopupData;

        private void updatAssignmentStatus()
        {
            if(_schedule==null)return;
            if (_schedule.IsCompleted)
            {
                AssignmentStatusColor=new SolidColorBrush(Colors.GreenYellow);
                AssignmentStatus = "完了済";
            }
            else
            {
                AssignmentStatusColor=new SolidColorBrush(Colors.DarkOrange);
                AssignmentStatus = "未完了";
            }
        }

        public string AssignmentName
        {
            get { return _assignmentName; }
            set
            {
                if (value == _assignmentName) return;
                _assignmentName = value;
                OnPropertyChanged();
            }
        }

        public string DueDateInformation
        {
            get { return _dueDateInformation; }
            set
            {
                if (value == _dueDateInformation) return;
                _dueDateInformation = value;
                OnPropertyChanged();
            }
        }

        public string RemainingDateInformation
        {
            get { return _remainingDateInformation; }
            set
            {
                if (value == _remainingDateInformation) return;
                _remainingDateInformation = value;
                OnPropertyChanged();
            }
        }

        public string AssignmentStatus
        {
            get { return _assignmentStatus; }
            set
            {
                if (value == _assignmentStatus) return;
                _assignmentStatus = value;
                OnPropertyChanged();
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
            }
        }

        public Brush AssignmentStatusColor
        {
            get { return _assignmentStatusColor; }
            set
            {
                if (Equals(value, _assignmentStatusColor)) return;
                _assignmentStatusColor = value;
                OnPropertyChanged();
            }
        }

        public ICommand CompleteCommand { get; set; }
        public ICommand OneNoteCommand { get; set; }

        public EditAssignmentPopupViewModel EditAssignmentPopupData
        {
            get { return _editAssignmentPopupData; }
            set
            {
                if (Equals(value, _editAssignmentPopupData)) return;
                _editAssignmentPopupData = value;
                OnPropertyChanged();
            }
        }
    }

    class AssignmentListUnitViewModelInDesign : AssignmentListUnitViewModel
    {
        public AssignmentListUnitViewModelInDesign()
        {
            this.AssignmentName = "後期レポート";
            this.RemainingDateInformation = "残り1日";
            this.AssignmentStatusColor = new SolidColorBrush(Colors.DarkOrange);
            this.AssignmentStatus = "未完了";
            this.AssignmentDetail = "This is detail.\nThis is detail.\nThis is detail.\nThis is detail.";
        }
    }
}
