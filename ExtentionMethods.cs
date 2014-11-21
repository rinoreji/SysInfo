using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace SysInfo
{
    public static class ExtentionMethods
    {
        public static string UseFormat(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        public static void Serialize(this List<BatteryInfo> batteryInfos)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var dir = new FileInfo(location).Directory;

            using (var stream = File.OpenWrite(Path.Combine(dir.FullName, "BatteryInfos.txt")))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<BatteryInfo>));
                serializer.Serialize(stream, batteryInfos);
            }
        }

        public static List<BatteryInfo> DeserializeBatteryInfo()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var dir = new FileInfo(location).Directory;

            using (var stream = File.OpenRead(Path.Combine(dir.FullName, "BatteryInfos.txt")))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<BatteryInfo>));
                return (List<BatteryInfo>)serializer.Deserialize(stream);
            }
        }
    }
}
