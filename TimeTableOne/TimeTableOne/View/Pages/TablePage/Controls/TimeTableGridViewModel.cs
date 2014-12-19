using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public class TimeTableGridViewModel:BasicViewModel
    {
        public TimeTableGridViewModel()
        {
            GridItems=new ObservableCollection<BasicViewModel>();
            
        }

        public ObservableCollection<BasicViewModel> GridItems { get; set; } 

        public int ElementWidth { get; set; }

        public int ElementHeight { get; set; }

        public int WidthSplit { get; set; }
    }

    public class TimeTableGridViewModelInDesign : TimeTableGridViewModel
    {
        public TimeTableGridViewModelInDesign()
        {
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            GridItems.Add(new TimeDisplayUnitViewModel());
            ElementWidth = 200;
            ElementHeight = 90;
            WidthSplit = 8;
        }
    }

    public class TimeTableGridTemplateSelector : DataTemplateSelector
    {

        public DataTemplate TimeRegionTemplate { get; set; }

        public DataTemplate TimeTableUnitTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is TimeDisplayUnitViewModel)
            {
                return TimeRegionTemplate;
            }
            else
            {
                return TimeTableUnitTemplate;
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return this.SelectTemplateCore(item);
        }
    }
}
