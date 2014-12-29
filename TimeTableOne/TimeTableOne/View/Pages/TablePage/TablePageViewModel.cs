using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.StartScreen;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.DayPage.Controls;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.TablePage
{
    public class TablePageViewModel : BasicViewModel
    {
        private TimeTableGridViewModel _timeTableDataContext;
        private string _pageTitleForEdit;

        public TablePageViewModel()
        {
            TimeTableDataContext=new TimeTableGridViewModel();
            Width = TableLayoutManager.getElementWidth(ApplicationData.Instance.Configuration.TableTypeSetting);
            var config = ApplicationData.Instance.Configuration;
            PageTitleForEdit=PageTitle = config.PageTitle;
            TimeControlData = new TimeControlViewModel();
            GridHeaders = new ObservableCollection<BasicViewModel>();
            UpdateHeader();
        }

        public TimeTableGridViewModel TimeTableDataContext
        {
            get { return _timeTableDataContext; }
            set
            {
                if (Equals(value, _timeTableDataContext)) return;
                _timeTableDataContext = value;
                OnPropertyChanged();
            }
        }

        public int Width { get; set; }


        public int WidthSplit { get; set; }

        public string PageTitle { get; set; }

        public string PageTitleForEdit
        {
            get { return _pageTitleForEdit; }
            set
            {
                if (value == _pageTitleForEdit||string.IsNullOrEmpty(value)) return;
                _pageTitleForEdit = value;
                PageTitle = value;
                ApplicationData.Instance.Configuration.PageTitle = value;
                ApplicationData.SaveData();
                OnPropertyChanged("PageTitle");
                OnPropertyChanged();
            }
        }

        public TimeControlViewModel TimeControlData { get; set; }

        public ObservableCollection<BasicViewModel> GridHeaders { get; set; }

        public int ElementWidth { get; set; }

        public int ElementHeight { get; set; }

        public void UpdateHeader()
        {
            GridHeaders.Clear();
            TableType type = ApplicationData.Instance.Configuration.TableTypeSetting;
            int n = TableLayoutManager.getElementCount(type);
            GridHeaders.Add(new EmptyGridUnitViewModel());
            DayOfWeek[] headers = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

            for (int i = 0; i < n; i++)
            {
                GridHeaders.Add(new TimeGridHeaderViewModel(headers[i]) { TextBrush = headers[i].GetWeekColor() });
            }
            ElementWidth = TableLayoutManager.getElementWidth(type);
            ElementHeight = 90;
            WidthSplit = n + 1;
        }
    }

    public class TablePageViewModelInDesign : TablePageViewModel
    {
        public TablePageViewModelInDesign()
        {

        }
    }
}
