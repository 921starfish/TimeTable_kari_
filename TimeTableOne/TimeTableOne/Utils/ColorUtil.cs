using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace TimeTableOne.Utils
{
    public static class ColorUtil
    {
        public static float Liminance(this Color col)
        {
            return (float) (0.298912*col.R + 0.586611*col.G + 0.114478*col.B);
        }
    }
}
