using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace TimeTableOne.Common
{
    static class StaticResource
    {
        public static Color OneNotePurple { get {return Color.FromArgb(0,128,57,123);  } }
        public static Color EditBoxGray { get { return Color.FromArgb(0, 232, 226, 232); } }
        public static Color TextBoxGray { get { return Color.FromArgb(0, 240, 237, 243); } }
        public static Color TextBoxBorder { get { return Color.FromArgb(0, 224, 218, 222); } }
    }
}
