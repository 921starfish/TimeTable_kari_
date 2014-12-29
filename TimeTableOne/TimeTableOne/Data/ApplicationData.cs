using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.Web.AtomPub;
using TimeTableOne.Data.Args;
using TimeTableOne.Utils;

namespace TimeTableOne.Data
{
    public class ApplicationData
    {

        private static ApplicationData _instance;

        public static ApplicationData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadData();
                    InitializeBeforeInstanceUsing(_instance);
                }
                return _instance;
            }
        }

        private static void InitializeBeforeInstanceUsing(ApplicationData instance)
        {
            instance.Assignments.CollectionChanged += Assignments_CollectionChanged;
        }

        static void Assignments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AssignmentSchedule sc = Instance.Assignments[e.NewStartingIndex];
            Instance.OnAssignmentChanged(Instance, new AssignmentChangedEventArgs(sc.ScheduleId));
        }

        #region Events
        /// <summary>
        /// 保存されている課題リストが変動した際に呼び出されます。
        /// </summary>
        public event EventHandler<AssignmentChangedEventArgs> OnAssignmentChanged = delegate { };

        #endregion

        public List<ScheduleKey> Keys = new List<ScheduleKey>();

        public List<ScheduleData> Data = new List<ScheduleData>();

        public List<ScheduleTimeSpan> TimeSpans = new List<ScheduleTimeSpan>();

        public ConfigurationData Configuration = new ConfigurationData();

        public ObservableCollection<AssignmentSchedule> Assignments = new ObservableCollection<AssignmentSchedule>();

        public List<ClassRoomChangeSchedule> ClassRoomChanges = new List<ClassRoomChangeSchedule>();

        public List<NoClassSchedule> NoClassSchedules = new List<NoClassSchedule>();

        private static ApplicationDataContainer SettingFolder
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.LocalSettings;
            }
        }


        public static async Task<ScheduleKey> ToScheduleKey(TableKey key)
        {
            return Instance.GetScheduleKey(await Instance.GetKey(key.NumberOfDay, key.TableNumber));
        }

        private static string getDataString()
        {
            if (SettingFolder.Values["DATA-COUNT"] != null)
            {
                int count = (int)SettingFolder.Values["DATA-COUNT"];
                List<byte> decompressed = new List<byte>();
                for (int i = 0; i < count; i++)
                {
                    byte[] data = SettingFolder.Values["DATA-" + i] as byte[];
                    decompressed.AddRange(data);
                }
                return Encoding.Unicode.GetString(decompressed.ToArray(), 0, decompressed.Count);
            }
            else
            {
                return null;
            }
        }

        private static void setDataString(string data)
        {
            char[] saveChars = data.ToCharArray();
            byte[] dataAsBytes = Encoding.Unicode.GetBytes(saveChars);
            byte[] buffer = new byte[1024];
            using (MemoryStream ms = new MemoryStream(dataAsBytes))
            {
                int sum = 0;
                int count = 0;
                int itr = 0;
                while ((count = ms.Read(buffer, 0, 1024)) != 0)
                {
                    byte[] cpBuf = new byte[count];
                    Array.Copy(buffer, cpBuf, count);
                    SettingFolder.Values["DATA-" + itr] = cpBuf;
                    sum += count;
                    itr++;
                }
                SettingFolder.Values["DATA-COUNT"] = itr;
            }
        }

        public void CheckAndRemoveRow()
        {
            ISet<ScheduleKey> RemoveCandidate=new HashSet<ScheduleKey>();
            foreach (var scheduleKey in Keys)
            {
                if (scheduleKey.TableNumber > Configuration.TableCount)
                {
                    RemoveCandidate.Add(scheduleKey);
                }
            }
            foreach (var scheduleKey in RemoveCandidate)
            {
                Keys.Remove(scheduleKey);
            }
        }
        /// <summary>
        /// データをでシリアライズして取得
        /// </summary>
        /// <returns></returns>
        public static ApplicationData LoadData()
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter importer = new StreamWriter(ms))
            {
                string castedData = getDataString();
                if (string.IsNullOrWhiteSpace(castedData)) return new ApplicationData();
                Debug.WriteLine(castedData);
                importer.WriteAsync(castedData);
                importer.FlushAsync();
                ms.Seek(0, SeekOrigin.Begin);
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));
                XmlReader reader = XmlReader.Create(ms);
                try
                {
                    if (serializer.CanDeserialize(reader))
                    {
                        return serializer.Deserialize(reader) as ApplicationData;
                    }
                }
                catch (XmlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return new ApplicationData();
            }
        }

        /// <summary>
        /// データをシリアライズ
        /// </summary>
        /// <returns></returns>
        public static void SaveData()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));
                serializer.Serialize(ms, Instance);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms))
                {
                    setDataString(reader.ReadToEnd());
                }
            }
        }

        private async Task<Guid> GetKey(int dayofWeek, int tableNumber)
        {
            if (dayofWeek == 0) dayofWeek = 7;
            foreach (var guid in from k in Keys where k.DayOfWeek == dayofWeek && k.TableNumber == tableNumber select k.DataId)
            {
                return guid;
            }
            return Guid.Empty;
        }

        public ScheduleKey GetScheduleKey(Guid id)
        {
            return (from k in Keys where k.DataId == id select k).FirstOrDefault();
        }

        /// <summary>
        /// 指定したキーからスケジュールを検索
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <param name="tableNumber"></param>
        /// <returns></returns>
        public ScheduleData GetSchedule(int dayOfWeek, int tableNumber)
        {
            Guid key = GetKey(dayOfWeek, tableNumber).Result;
            if (key == Guid.Empty)
            {
                return null;
            }
            else
            {
                foreach (var scheduleData in from d in Data where d.ScheduleId.Equals(key) select d)
                {
                    return scheduleData;
                }
                return null;
            }
        }



        public IEnumerable<AssignmentSchedule> GetAssignments(ScheduleData data)
        {
            foreach (var assignmentSchedule in Assignments)
            {
                if (data.ScheduleId.Equals(assignmentSchedule.ScheduleId)) yield return assignmentSchedule;
            }
        }

        public ClassRoomChangeSchedule GetClassRoomChangeSchedule(DateTime t, TableKey key)
        {
            var sc = GetSchedule(key.NumberOfDay, key.TableNumber);
            if (sc == null) return null;
            foreach (var schedule in from cc in ClassRoomChanges where cc.ScheduleId.Equals(sc.ScheduleId) && cc.ChangedDay.Equals(t) select cc)
            {
                return schedule;
            }
            return null; 
        }

        public NoClassSchedule GetNoClassSchedule(DateTime t, TableKey key)
        {
            var sc = GetSchedule(key.NumberOfDay, key.TableNumber);
            if (sc == null) return null;
            foreach (var schedule in from cc in NoClassSchedules where cc.ScheduleId.Equals(sc.ScheduleId) && cc.Day.Equals(t) select cc)
            {
                return schedule;
            }
            return null;
        }

        public void RemoveSchedule(ScheduleData getCurrentSchedule)
        {
            ScheduleKey key = null;
            foreach (var scheduleKey in Keys)
            {
                if (scheduleKey.DataId == getCurrentSchedule.ScheduleId)
                {
                    key = scheduleKey;
                }
            }
            if (key != null) Keys.Remove(key);
            Data.Remove(getCurrentSchedule);
        }
    }

    public class NoClassSchedule
    {
        public Guid ScheduleId;

        public DateTime Day;
    }

    public class ClassRoomChangeSchedule
    {
        public DateTime ChangedDay;

        public Guid ScheduleId;

        public string ChangedTo;

        public ClassRoomChangeSchedule()
        {

        }

        public static ClassRoomChangeSchedule Generagte(DateTime next, ScheduleData targetLecture, string changedTo)
        {
            next = new DateTime(next.Year, next.Month, next.Day, 0, 0, 0);
            return new ClassRoomChangeSchedule()
            {
                ChangedDay = next,
                ChangedTo = changedTo,
                ScheduleId = targetLecture.ScheduleId
            };
        }
    }

    public class ScheduleKey
    {
        public ScheduleKey()
        {

        }

        public int DayOfWeek;

        public int TableNumber;

        public Guid DataId;

        public static ScheduleKey Generate(int dayOfWeek, int tableNumber, ScheduleData data)
        {
            return new ScheduleKey() { DayOfWeek = dayOfWeek, TableNumber = tableNumber, DataId = data.ScheduleId };
        }

                public bool IsNoClassThisWeek()
                {
                    return ApplicationData.Instance.GetNoClassSchedule(
                        DateTimeUtil.NextKeyDay(DateTime.Now, (DayOfWeek) DayOfWeek),
                        new TableKey(TableNumber, TableNumber)) != null;
                }
    }

    public class ScheduleData
    {
        public ScheduleData()
        {

        }

        public Guid ScheduleId;

        public string TableName = "";

        public string Place = "";

        public string FreeFormText = "";

        public string Description = "";

        public string OneNoteId = "";

        public DateTime CreationDate;
        public Color ColorData;

        public static ScheduleData GenerateEmpty()
        {
            return new ScheduleData() { ScheduleId = Guid.NewGuid(), ColorData = Color.FromArgb(255, 128, 57, 123), CreationDate = DateTime.Now };
        }

        public AssignmentSchedule GenerateAssignmentEmpty()
        {
            return new AssignmentSchedule() { ScheduleId = ScheduleId };
        }
    }

    public class ScheduleTimeSpan
    {
        public DateTime FromTime;
        public DateTime ToTime;

        public static ScheduleTimeSpan GenerateFromHourMinute(int fromHour, int fromMinute, int toHour, int toMinute)
        {
            return new ScheduleTimeSpan()
            {
                FromTime = new DateTime(2015, 1, 1, fromHour, fromMinute, 0),
                ToTime = new DateTime(2015, 1, 1, toHour, toMinute, 0)
            };
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", FromTime.ToString("HH:mm"), ToTime.ToString("HH:mm"));
        }
    }

    public class AssignmentSchedule
    {
        public DateTime DueTime;
        public Guid ScheduleId;
        public string AssignmentName = "";
        public string AssignmentDetail = "";
        public bool IsCompleted;
    }
}
