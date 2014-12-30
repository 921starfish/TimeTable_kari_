using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using TimeTableOne.Data;
using TimeTableOne.Utils;
using TimeTableOne.View.Pages.TablePage.Controls;
using System.ComponentModel;
using TimeTableOne.Utils.Commands;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public class ColorPopupViewModel : BasicViewModel
    {
        private int _selectedIndex;
        private SolidColorBrush _tableColor;
        private Brush _foreColor;

        protected ColorPopupViewModel()
        {
            this.ColorItems = new ObservableCollection<ColorPopupUnitViewModel>();
         
        }

        private ColorPopupViewModel(ScheduleData data) : this()
        {
            _tableColor = new SolidColorBrush(data.ColorData);
            GenerateColors();
            for (int i = 0; i < ColorItems.Count; i++)
            {
                if (ColorItems[i].ColorBrush.Color.Equals(data.ColorData))
                {
                    SelectedIndex = i;
                    return;
                }
                EditPageUpdateEvents.ColorUpdateEvent += () =>
                {
                    this.TableColor = new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData);
                };
            }
            EditPageUpdateEvents.ColorUpdateEvent += () =>
            {
                this.TableColor=new SolidColorBrush(TableUnitDataHelper.GetCurrentSchedule().ColorData);
            };
        }

        public ObservableCollection<ColorPopupUnitViewModel> ColorItems { get; set; }


        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value == _selectedIndex) return;
                _selectedIndex = value;
                OnPropertyChanged();

            }
        }


        protected void GenerateColors()
        {

            Color OneNotePurpleColor = (Color) Application.Current.Resources["OneNotePurpleColor"];
            
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Red));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Orange));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Yellow));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.YellowGreen));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Green));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Blue));
            ColorItems.Add(new ColorPopupUnitViewModel(OneNotePurpleColor));

            ColorItems.Add(new ColorPopupUnitViewModel(Colors.DeepPink));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightSalmon));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightYellow));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightGoldenrodYellow));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightGreen));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightBlue));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.MediumPurple));

            ColorItems.Add(new ColorPopupUnitViewModel(Colors.Black));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 32, 32, 32)));
            ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 64, 64, 64)));
            ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 96, 96, 96)));
            ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 128, 128, 128)));
            ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 160, 160, 160)));
            ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 192, 192, 192)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 224, 224, 224)));
            ColorItems.Add(new ColorPopupUnitViewModel(Colors.White));

            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 254,  99, 119)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.Pink));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightSalmon));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightYellow));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightGreen));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.LightBlue));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.Violet));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.OrangeRed));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.RoyalBlue));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.Azure));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.Khaki));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.Maroon));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 6, 255, 119)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255, 242, 44, 90)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.DeepPink));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255,99,216,183)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255,255,134,93)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255,251,226,0)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Color.FromArgb(255,152, 81, 75)));
            //ColorItems.Add(new ColorPopupUnitViewModel(Colors.Transparent));
        }


      

        public static ColorPopupViewModel GenerateViewModel(ScheduleData data)
        {
            return new ColorPopupViewModel(data);
        }

        public SolidColorBrush TableColor
        {
            get { return _tableColor; }
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
