using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
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
        public ScheduleKey[] Keys = new ScheduleKey[0];

        public ScheduleData[] Data = new ScheduleData[0];

        private static ApplicationDataContainer getDataContainer()
        {
            return Windows.Storage.ApplicationData.Current.LocalSettings;
        }
        /// <summary>
        /// データをでシリアライズして取得
        /// </summary>
        /// <returns></returns>
        public async static Task<ApplicationData> LoadData()
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter importer = new StreamWriter(ms))
            {
                object rawData = getDataContainer().Values["Data"];
                string castedData = rawData as string;
                if (castedData == null)
                {
                    return new ApplicationData();
                }
                else
                {
                    Debug.WriteLine(castedData);
                    await importer.WriteAsync(castedData);
                    await importer.FlushAsync();
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

        }

        /// <summary>
        /// データをシリアライズ
        /// </summary>
        /// <param name="data">シリアライズするデータ</param>
        /// <returns></returns>
        public static async Task SaveData(ApplicationData data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));
                serializer.Serialize(ms, data);
                await ms.FlushAsync();
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms))
                {
                    getDataContainer().Values["Data"] = await reader.ReadToEndAsync();
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
        public async Task<ScheduleData> GetSchedule(int dayOfWeek, int tableNumber)
        {
            Guid key = await GetKey(dayOfWeek, tableNumber);
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

        public string ClassName;

        public string Place;

        public string FreeFormText;

        public string Description;

        public static ScheduleData GenerateEmpty()
        {
            return new ScheduleData() { ScheduleId = Guid.NewGuid() };
        }
    }
}
