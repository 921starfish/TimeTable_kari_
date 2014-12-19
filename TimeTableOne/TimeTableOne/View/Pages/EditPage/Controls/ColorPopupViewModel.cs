using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.View.Pages.TablePage.Controls;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class ColorPopupViewModel:BasicViewModel
    {
        private int _selectedIndex;

        protected ColorPopupViewModel()
        {
            this.ColorItems=new ObservableCollection<ColorPopupUnitViewModel>();
        }

        private ColorPopupViewModel(ScheduleData data):this()
        {
            GenerateColors();
            for (int i = 0; i < ColorItems.Count; i++)
            {
                if (ColorItems[i].ColorBrush.Color.Equals(data.ColorData))
                {
                    SelectedIndex = i;
                    return;
                }
            }
        }

        public ObservableCollection<ColorPopupUnitViewModel> ColorItems { get; set; }


        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (value == _selectedIndex) return;
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }


        protected void GenerateColors()
        {
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.AliceBlue));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.AntiqueWhite));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Aquamarine));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Bisque));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Orange));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.IndianRed));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Red));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.YellowGreen));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Black));
        }

        public static ColorPopupViewModel GenerateViewModel(ScheduleData data)
        {
            return new ColorPopupViewModel(data);
        }
    }

    public class ColorPopupViewModelInDesign : ColorPopupViewModel
    {
        public ColorPopupViewModelInDesign()
        {
            GenerateColors();
        }

    }

    public class ColorPopupUnitViewModel:BasicViewModel
    {
        public ColorPopupUnitViewModel()
        {
            
        }

        public ColorPopupUnitViewModel(byte r,byte g,byte b)
        {
            ColorBrush=new SolidColorBrush(Color.FromArgb(255,r,g,b));
        }

        public ColorPopupUnitViewModel(Color color)
        {
            ColorBrush=new SolidColorBrush(color);
        }
        public SolidColorBrush ColorBrush { get; set; }
    }

    public class ColorPopupUnitViewModelInDesign : ColorPopupUnitViewModel
    {
        public ColorPopupUnitViewModelInDesign():base(128,128,255)
        {
            
        }
    }
}
