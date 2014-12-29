using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using TimeTableOne.Data;
using TimeTableOne.Utils;

namespace TimeTableOne.Common
{
    public class NotificationManager
    {
        private static NotificationManager _instance;

        public static NotificationManager Instance
        {
            get
            {
                _instance = _instance ?? new NotificationManager();
                return _instance;
            }
        }

        private NotificationManager()
        {
            
        }

        public void UpdateTile()
        {
            XmlDocument template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310BlockAndText01);
            //日付の追加
            template.AppendTextElement(7, DateTime.Now.ToString("dd"))
                .AppendTextElement(8, DateTime.Now.ToString("dddd"));
            //本日の予定のデータをとってくる
            ScheduleData currentSc;
            if ((currentSc = ScheduleManager.Instance.CurrentSchedule) != null)
            {
                if (ScheduleManager.Instance.CurrentScheduleState == ScheduleState.NoClass)
                {
                    template.AppendTextElement(1, "現在の授業");
                    template.AppendTextElement(2,
                        ScheduleManager.Instance.CurrentTimeSpanIndex + "時間目  " + currentSc.TableName+"(休講)");
                }
                else
                {
                    template.AppendTextElement(1, "現在の授業");
                    template.AppendTextElement(2,
                        ScheduleManager.Instance.CurrentTimeSpanIndex + "時間目  " + currentSc.TableName);
                    template.AppendTextElement(3,
                        currentSc.Place + "  " + ScheduleManager.Instance.CurrentTimeSpan.ToString());
                    
                }
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

        public void UpdateBadgeNotification()
        {
            XmlDocument doc=BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeGlyph);
            UpdateWideTileNotification();
        }

        public void UpdateWideTileNotificationOfToday()
        {
            var spans = ScheduleManager.Instance.GetTodayKeyTiming(ScheduleTimingType.BeginTimeWithNoClass|ScheduleTimingType.EndTime);
            var status = ScheduleManager.Instance.GetScheduleStatus(0);
            foreach (var unit in spans)
            {
                var widecontent = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150BlockAndText02);
                widecontent.AppendTextElement(1,unit.Time.ToString("dd"));
                widecontent.AppendTextElement(2,unit.Time.ToString("dddd"));
                string thirdContent = "";
                //現在授業中かどうか
                if (ScheduleManager.Instance.CurrentSchedule != null)//現在授業中である
                {
                    var cs = ScheduleManager.Instance.CurrentSchedule;
                    thirdContent = String.Format("只今の授業 {0}\n{1}  {2}", cs.TableName, cs.GetTimeSpan().ToString(), cs.Place);
                }
                else
                {
                    var cs = ScheduleManager.Instance.NextScheduleInToday;
                    if (cs == null)
                    {
                        thirdContent = "本日次の授業はありません。";
                    }
                    else
                    {
                        thirdContent = String.Format("次の授業 {0}\n{1}  {2}", cs.TableName, cs.GetTimeSpan().ToString(), cs.Place);
                    }
                }
            }

        }

        public void UpdateWideTileNotification()
        {
            var widecontent = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150BlockAndText02);
            widecontent.AppendTextElement(1,DateTime.Now.ToString("dd"));
            widecontent.AppendTextElement(2, DateTime.Now.ToString("dddd"));
            string thirdContent = "";
            //現在授業中かどうか
            if (ScheduleManager.Instance.CurrentSchedule != null)//現在授業中である
            {
                var cs = ScheduleManager.Instance.CurrentSchedule;
                thirdContent=String.Format("只今の授業 {0}\n{1}  {2}",cs.TableName,cs.GetTimeSpan().ToString(),cs.Place);
            }
            else
            {
                var cs = ScheduleManager.Instance.NextScheduleInToday;
                if (cs == null)
                {
                    thirdContent = "本日次の授業はありません。";
                }
                else
                {
                    thirdContent = String.Format("次の授業 {0}\n{1}  {2}", cs.TableName, cs.GetTimeSpan().ToString(), cs.Place);
                }
            }
            widecontent.AppendTextElement(0, thirdContent);
            TileNotification notif=new TileNotification(widecontent);
            notif.ExpirationTime = DateTimeOffset.UtcNow.AddHours(1);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notif);
        }

        /// <summary>
        /// 本日のトースト通知を更新します。
        /// </summary>
        public void UpdateNotificationDataOfToday()
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var notifs = toastNotifier.GetScheduledToastNotifications();
            foreach (var notif in notifs)
            {
                toastNotifier.RemoveFromSchedule(notif);
            }
            IEnumerable<ScheduleTimingUnit> todayCoreTime =
                ScheduleManager.Instance.GetTodayKeyTiming(ScheduleTimingType.BeginTime);
            var dat=ScheduleManager.Instance.TodaySchedule;
            foreach (var dateTime in todayCoreTime)
            {
                DateTime daysoffset = DateTimeUtil.NowMoments();
                TimeSpan span = dateTime.Time - daysoffset -
                                    TimeSpan.FromMinutes(ApplicationData.Instance.Configuration.NotifictionExtratime);
                ScheduledToastNotification schd = new ScheduledToastNotification(generateToastNotification(dat[dateTime.ClassNumber]), DateTimeOffset.UtcNow.Add(span));
                toastNotifier.AddToSchedule(schd);
            }
    }

        private XmlDocument generateToastNotification(ScheduleData schedule)
        {
            XmlDocument template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            template.AppendTextElement(0, schedule.GetTimeSpanIndex() + "時間目 " + schedule.TableName);
            template.AppendTextElement(1, "場所:" + schedule.Place);
            template.AppendTextElement(2, "時間:" + schedule.GetTimeSpan().ToString());
            return template;
        }                                                                                                                                                                                                                                                            
        
    }
}
