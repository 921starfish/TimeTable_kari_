using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.Common
{
    public enum Language
    {
        Japanese,
        English,
        Chinese
    };

    static class TextManager
    {
        static TextManager()
        {
            SetLanguage(Language.Japanese);
        }

        public static void SetLanguage(Language language)
        {
            switch (language)
            {
                case Language.Japanese:
                    break;
                case Language.English:
                    break;
                case Language.Chinese:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("language");
            }
        }

        public static void SetJapanese()
        {
        }

        public static void SetEnglish()
        {

        }

        public static void SetChinese()
        {
        }
    }
}
