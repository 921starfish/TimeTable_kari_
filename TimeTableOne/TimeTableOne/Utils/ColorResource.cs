using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.Utils
{
    public class ColorResource
    {
        public Brush OneNotePurple
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 128, 57, 123)); }
        }
        public Brush EditBoxGray
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 232, 226, 232)); }
        }
        public Brush TextBoxGray
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 240, 237, 243)); }
        }
        public Brush TextBoxBorder
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 224, 218, 222)); }
        }
        public Brush DetailBoxBlack
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 33, 33, 33)); }
        }
        public Brush SelectedDetailBoxBlack
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 71, 71, 71)); }
        }
    }
}
