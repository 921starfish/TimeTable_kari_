using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.EditPage.Controls;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public class TimeTableGridViewModel:BasicViewModel
    {
        public TimeTableGridViewModel()
        {
            GridItems=new ObservableCollection<BasicViewModel>();
            bool isDesignmode = DesignMode.DesignModeEnabled;
            DayOfWeek[] headers = {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            Color[] colors =
            {
                Colors.Black, Colors.Black, Colors.Black, Colors.Black, Colors.Black,
                Colors.CornflowerBlue, Colors.Red
            };
           
            var f = Window.Current.Content as Frame;
            GridItems.Add(new EmptyGridUnitViewModel());
            for (int i = 0; i < 7; i++)
            {
                GridItems.Add(new TimeGridHeaderViewModel(headers[i]) {  TextBrush = new SolidColorBrush(colors[i]) });
            }
            for (int i = 0; i < 7; i++)
            {
                for (int w = 0; w < 8; w++)
                {
                    if (w == 0)
                    {
                        if (isDesignmode) GridItems.Add(new TimeDisplayUnitViewModelInDesign());
                        else
                        {
                            GridItems.Add(TimeDisplayUnitViewModel.FromData(i));
                        }
                    }
                    else
                    {
                        if (isDesignmode) GridItems.Add(new TimeTableViewModelInDesign());
                        else
                        {
                            GridItems.Add(new TimeTableViewModel(new TableKey(i + 1, w + 1)));
                        }
                    }
                }
            }
            ElementWidth = 170;
            ElementHeight = 90;
            WidthSplit = 8;
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

        }
    }

    public class TimeTableGridTemplateSelector : DataTemplateSelector
    {
        private int trCount = 0;
        private int ttutCount = 0;
        private int htCount = 0;
        public TimeTableGridTemplateSelector()
        {
            Debug.WriteLine("Construct!");
        }
        public DataTemplate TimeRegionTemplate { get; set; }

        public DataTemplate TimeTableUnitTemplate { get; set; }

        public DataTemplate HeaderTemplate { get; set; }

        public DataTemplate EmptyTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is TimeDisplayUnitViewModel)
            {
                trCount++;
                return TimeRegionTemplate;
            }
            else if(item is TimeTableViewModel)
            {
                ttutCount++;
                return TimeTableUnitTemplate;
            }
            else if (item is TimeGridHeaderViewModel)
            {
                htCount++;
                return HeaderTemplate;
            }else if (item is EmptyGridUnitViewModel)
            {
                return EmptyTemplate;
            }
            else
            {
                throw new InvalidDataContractException("Invalid Viewmodel was pasesd!!");
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return this.SelectTemplateCore(item);
        }

    }
}
