using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using TimeTableOne.Data;

namespace TimeTableOne.Common
{
    public class LiveTileManager
    {
        private static LiveTileManager _instance;

        public static LiveTileManager Instance
        {
            get
            {
                _instance = _instance ?? new LiveTileManager();
                return _instance;
            }
        }

        private LiveTileManager()
        {
            
        }

        public void GenerateTile()
        {
            XmlDocument template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310BlockAndText01);
            //日付の追加
            template.AppendTextElement(7, DateTime.Now.ToString("dd"))
                .AppendTextElement(8, DateTime.Now.ToString("dddd"));
            //本日の予定のデータをとってくる
            ScheduleData currentSc;
            if ((currentSc = ScheduleManager.Instance.CurrentSchedule) != null)
            {
                template.AppendTextElement(1, "現在の授業");
                template.AppendTextElement(2, ScheduleManager.Instance.CurrentTimeSpanIndex+"時間目  "+currentSc.TableName);
                template.AppendTextElement(3, currentSc.Place+"  "+ScheduleManager.Instance.CurrentTimeSpan.ToString());
            }
            else
            {
                template.AppendTextElement(1, "現在授業はありません。");
            }
            //次の授業をとってくる
            ScheduleData nextSc;
            if ((nextSc = ScheduleManager.Instance.NextScheduleInToday) != null)
            {
                template.AppendTextElement(4, "本日の次の授業");
                template.AppendTextElement(5, ScheduleManager.Instance.NextTimeSpanIndexInToday+"時間目 "+nextSc.TableName);
                template.AppendTextElement(6,
                    nextSc.Place + "  " + ScheduleManager.Instance.NextTimeSpanInToday.ToString());
            }
            else
            {
                template.AppendTextElement(4, "本日次の授業はありません。");
            }
            TileNotification notif=new TileNotification(template);
            notif.ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(60);
           TileUpdateManager.CreateTileUpdaterForApplication().Update(notif);

        }

        
    }
}
