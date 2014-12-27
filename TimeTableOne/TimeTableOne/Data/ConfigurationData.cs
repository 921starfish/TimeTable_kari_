﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.Data
{
    public class ConfigurationData
    {

        public ConfigurationData()
        {
            
        }
        public TableType TableTypeSetting=TableType.AllDay;

        public int TableCount = 7;

        public string BackgroundImagePath = "ms-appx:///Assets/Background(1).jpg";

        public string PageTitle = "TimeTable";
    }

    public enum TableType
    {
        AllDay=0,WeekDay=1,WithoutSunday=2
    }
}
