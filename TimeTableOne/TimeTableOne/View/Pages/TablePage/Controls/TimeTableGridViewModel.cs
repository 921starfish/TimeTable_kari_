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
using TimeTableOne.Data;
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
            TableType type = ApplicationData.Instance.Configuration.TableTypeSetting;
            var f = Window.Current.Content as Frame;
            int n = type == TableType.AllDay ? 7 : 5;
            GridItems.Add(new EmptyGridUnitViewModel());
            for (int i = 0; i < n; i++)
            {
                GridItems.Add(new TimeGridHeaderViewModel(headers[i]) {  TextBrush = headers[i].GetWeekColor() });
            }
            for (int i = 0; i < ApplicationData.Instance.Configuration.TableCount; i++)
            {
                for (int w = 0; w < n+1; w++)
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
                            GridItems.Add(new TimeTableViewModel(new TableKey(i + 1, w)));
                        }
                    }
                }
            }
           GridItems.Add(new AppendRowButtonViewModel());
            ElementWidth =TableLayoutManager.getElementWidth(type);
            ElementHeight = 90;
            WidthSplit = n+1;
        }


        public ObservableCollection<BasicViewModel> GridItems { get; set; } 

        public int ElementWidth { get; set; }

        public int ElementHeight { get; set; }

        public int WidthSplit { get; set; }

        public DeleteRowCommand DeleteRow { get; set; }
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

        public DataTemplate AppendRowTemplate { get; set; }

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
            }else if (item is AppendRowButtonViewModel)
            {
                return AppendRowTemplate;
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

    public class TimeTableGridContainerStyleSelector : StyleSelector
    {
        public Style HeaderStyle { get; set; }

        public Style TableElementStyle { get; set; }

        public Style TimeRegionStyle { get; set; }
        
        public Style EmptyStyle { get; set; }

        public Style AppendRowButtonStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {

            if (item is TimeDisplayUnitViewModel)
            {
                return TimeRegionStyle;
            }
            else if (item is TimeTableViewModel)
            {
                return TableElementStyle;
            }
            else if (item is TimeGridHeaderViewModel)
            {
                return HeaderStyle;
            }
            else if (item is EmptyGridUnitViewModel)
            {
                return EmptyStyle;
            }else if (item is AppendRowButtonViewModel)
            {
                return AppendRowButtonStyle;
            }
            else
            {
                throw new InvalidDataContractException("Invalid Viewmodel was pasesd!!");
            }
        }
    }
}
