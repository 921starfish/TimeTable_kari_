using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.StartScreen;
using TimeTableOne.Data;
using TimeTableOne.Utils;
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
    }

    public class TablePageViewModelInDesign : TablePageViewModel
    {
        public TablePageViewModelInDesign()
        {

        }
    }
}
