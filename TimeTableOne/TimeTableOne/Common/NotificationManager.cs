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
                template.AppendTextElement(4, "このあとの授業");
                template.AppendTextElement(5, ScheduleManager.Instance.NextTimeSpanIndexInToday+"時間目 "+nextSc.TableName);
                template.AppendTextElement(6,
                    nextSc.Place + "  " + ScheduleManager.Instance.NextTimeSpanInToday.ToString());
            }
            else
            {
                template.AppendTextElement(4, "このあと授業はありません。");
            }
            TileNotification notif=new TileNotification(template);
            notif.ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(60);
           TileUpdateManager.CreateTileUpdaterForApplication().Update(notif);
        }

        private void UpdateWideTileNow()
        {
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            var scheduleStatus = ScheduleManager.Instance.CurrentScheduleState;
            var widecontent = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150BlockAndText02);
            widecontent.AppendTextElement(1, DateTime.Now.ToString("dd"));
            widecontent.AppendTextElement(2, DateTime.Now.ToString("dddd"));
            string thirdContent = "";
            //現在授業中かどうか
            if (!scheduleStatus.IsNoClass()&&ScheduleManager.Instance.CurrentSchedule!=null)
            {
                var cs = ScheduleManager.Instance.CurrentSchedule;
                thirdContent = String.Format("只今の授業 {0}\n{1}  {2}", cs.TableName, cs.GetTimeSpan().ToString(), cs.Place);
            }
            else
            {
                if (ScheduleManager.Instance.NextScheduleInToday==null)
                {
                    thirdContent = "このあと授業はありません。";
                }
                else
                {
                    var cs = ScheduleManager.Instance.NextScheduleInToday;
                    thirdContent = String.Format("次の授業 {0}\n{1}  {2}", cs.TableName, cs.GetTimeSpan().ToString(), cs.Place);
                }
            }
            widecontent.AppendTextElement(0, thirdContent);
            tileUpdater.Update(new TileNotification(widecontent));
        }

        public void UpdateWideTileNotificationOfToday()
        {
            ScheduleManager.Instance.UpdateCurrentTable();
            var spans = ScheduleManager.Instance.GetTodayKeyTiming(ScheduleTimingType.BeginTimeWithNoClass|ScheduleTimingType.EndTime);
            var status = ScheduleManager.Instance.GetScheduleStatus(0);
            var schedules = ScheduleManager.Instance.TodaySchedule;
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            foreach (var scNotif in tileUpdater.GetScheduledTileNotifications())
            {
                tileUpdater.RemoveFromSchedule(scNotif);
            }
            foreach (var unit in spans)
            {
                var widecontent = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150BlockAndText02);
                widecontent.AppendTextElement(1,unit.Time.ToString("dd"));
                widecontent.AppendTextElement(2,unit.Time.ToString("dddd"));
                string thirdContent = "";
                //現在授業中かどうか
                if (unit.TimingType==SpanTimeType.BeginTime&&!status[unit.ClassNumber][0].IsNoClass())
                {
                    var cs = schedules[unit.ClassNumber];
                    thirdContent = String.Format("只今の授業 {0}\n{1}  {2}", cs.TableName, cs.GetTimeSpan().ToString(), cs.Place);
                }
                else
                {
                    int nextClassIndex = -1;
                    for (int i = unit.ClassNumber+1; i < schedules.Length; i++)
                    {
                        if (schedules[i] != null)
                        {
                            nextClassIndex = i;
                            break;
                        }
                    }
                    if (nextClassIndex==-1)
                    {
                        thirdContent = "このあと授業はありません。";
                    }
                    else
                    {
                        var cs = schedules[nextClassIndex];
                        thirdContent = String.Format("次の授業 {0}\n{1}  {2}", cs.TableName, cs.GetTimeSpan().ToString(), cs.Place);
                    }
                }
                widecontent.AppendTextElement(0, thirdContent);
                var schedulerTiming = DateTimeOffset.UtcNow + (unit.Time - DateTime.Now.AsTimeData());
                var scNotif = new ScheduledTileNotification(widecontent, schedulerTiming);
                //デバッグ表示
                Debug.WriteLine("Schedule:{0} minutes after",(schedulerTiming-DateTimeOffset.UtcNow).TotalMinutes);
                Debug.WriteLine(widecontent.GetXml());
                tileUpdater.AddToSchedule(scNotif);
            }
            UpdateWideTileNow();
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
                if(span<new TimeSpan())continue;
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
