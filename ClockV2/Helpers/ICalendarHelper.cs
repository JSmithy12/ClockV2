using ClockV2.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ClockV2.Helpers
{
    public static class ICalendarHelper
    {
        public static void SaveAlarms(List<Alarm> alarms, string filePath)
        {
            if (alarms == null || alarms.Count == 0)
            {
                throw new InvalidOperationException("Cannot save iCalendar file: No alarms to save. At least one alarm is required.");
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//ClockV2//EN");

            foreach (var alarm in alarms)
            {
                sb.AppendLine("BEGIN:VEVENT");

                sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");

                sb.AppendLine($"UID:{Guid.NewGuid()}");

                sb.AppendLine($"SUMMARY:{alarm.Label}");
                sb.AppendLine($"DTSTART:{alarm.Time.ToUniversalTime():yyyyMMddTHHmmssZ}");
                sb.AppendLine("BEGIN:VALARM");
                sb.AppendLine("TRIGGER:-PT0M");
                sb.AppendLine("ACTION:DISPLAY");
                sb.AppendLine($"DESCRIPTION:{alarm.Label}");
                sb.AppendLine("END:VALARM");
                sb.AppendLine("END:VEVENT");
            }

            sb.AppendLine("END:VCALENDAR");

            File.WriteAllText(filePath, sb.ToString());
        }

        public static List<Alarm> LoadAlarms(string filePath)
        {
            List<Alarm> alarms = new List<Alarm>();
            string[] lines = File.ReadAllLines(filePath);
            string label = "";
            DateTime time = DateTime.MinValue;

            foreach (var line in lines)
            {
                if (line.StartsWith("SUMMARY:"))
                {
                    label = line.Substring("SUMMARY:".Length);
                }
                else if (line.StartsWith("DTSTART:"))
                {
                    string timeStr = line.Substring("DTSTART:".Length);
                    time = DateTime.ParseExact(timeStr, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                }
                else if (line == "END:VEVENT")
                {
                    if (time > DateTime.Now)
                        alarms.Add(new Alarm(time, label));
                    label = "";
                    time = DateTime.MinValue;
                }
            }

            return alarms;
        }
    }
}