﻿using System;
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
    public class NotificationTileManager
    {
        private static NotificationTileManager _instance;

        public static NotificationTileManager Instance
        {
            get
            {
                _instance = _instance ?? new NotificationTileManager();
                return _instance;
            }
        }

        private NotificationTileManager()
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

        public void NotifynextScheduleData()
    {
            var dat=ScheduleManager.Instance.TodaySchedule;

            for (int i = 0; i < dat.Length; i++)
            {
                if (dat[i]!=null&&dat[i].GetTimeSpan().FromTime-DateTimeUtil.NowMoments()>TimeSpan.FromMinutes(ApplicationData.Instance.Configuration.NotifictionExtratime))
                {
                    DateTime daysoffset = DateTimeUtil.NowMoments();
                    TimeSpan span = dat[i].GetTimeSpan().FromTime - daysoffset -
                                        TimeSpan.FromMinutes(ApplicationData.Instance.Configuration.NotifictionExtratime);
                    ScheduledToastNotification schd=new ScheduledToastNotification(generateToastNotification(dat[i]),DateTimeOffset.UtcNow.Add(span));
                    ToastNotificationManager.CreateToastNotifier().AddToSchedule(schd);
                }
            }
    }

        private XmlDocument generateToastNotification(ScheduleData schedule)
        {
            XmlDocument template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            //TODO 休講の場合はじく処理
            template.AppendTextElement(0, schedule.GetTimeSpanIndex() + "時間目 " + schedule.TableName);
            template.AppendTextElement(1, "場所:" + schedule.Place);
            template.AppendTextElement(2, "時間:" + schedule.GetTimeSpan().ToString());
            return template;
        }                                                                                                                                                                                                                                                            
        
    }
}
