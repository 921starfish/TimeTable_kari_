using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml;
using Windows.Web.AtomPub;

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
                }
                return _instance;
            }
        }

        public List<ScheduleKey> Keys = new List<ScheduleKey>();

        public List<ScheduleData> Data =new List<ScheduleData>();

        private static ApplicationDataContainer SettingFolder
        {
            get
            {
                return Windows.Storage.ApplicationData.Current.LocalSettings;
            }
        }

        private static string getDataString()
        {
            if (SettingFolder.Values["DATA-COUNT"] != null)
            {
                int count = (int)SettingFolder.Values["DATA-COUNT"];
                List<byte> decompressed=new List<byte>();
                for (int i = 0; i < count; i++)
                {
                    byte[] data = SettingFolder.Values["DATA-" + i] as byte[];
                    decompressed.AddRange(data);
                }
                return Encoding.Unicode.GetString(decompressed.ToArray(),0,decompressed.Count);
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
            byte[] buffer=new byte[1024];
            using (MemoryStream ms=new MemoryStream(dataAsBytes))
            {
                int sum = 0;
                int count = 0;
                int itr = 0;
                while ((count=ms.Read(buffer,0,1024))!=0)
                {
                    byte[] cpBuf=new byte[count];
                    Array.Copy(buffer,cpBuf,count);
                    SettingFolder.Values["DATA-" + itr] = cpBuf;
                    sum += count;
                    itr++;
                }
                SettingFolder.Values["DATA-COUNT"] = itr;
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
        /// <param name="data">シリアライズするデータ</param>
        /// <returns></returns>
        public static void SaveData(ApplicationData data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));
                serializer.Serialize(ms, data);
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
            foreach (var guid in from k in Keys where k.DayOfWeek == dayofWeek && k.TableNumber == tableNumber select k.DataId)
            {
                return guid;
            }
            return Guid.Empty;
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
    }

    public class ScheduleData
    {
        public ScheduleData()
        {
            
        }

        public Guid ScheduleId;

        public string TableName = "";

        public string Place ="";

        public string FreeFormText = "";

        public string Description = "";

        public static ScheduleData GenerateEmpty()
        {
            return new ScheduleData() { ScheduleId = Guid.NewGuid() };
        }
    }
}
