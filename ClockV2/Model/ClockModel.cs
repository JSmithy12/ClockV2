using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ClockV2.Model;
using PriorityQueue;

namespace ClockV2.Model
{
    public class ClockModel
    {
        private HeapPriorityQueue<Alarm> alarmQueue;

        public ClockModel()
        {
            alarmQueue = new HeapPriorityQueue<Alarm>(100);
        }

        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public void AddAlarm(Alarm alarm)
        {
            //Prioritises alarms based on lowest time first
            int priority = (int)(alarm.Time - DateTime.Now.TimeOfDay).TotalSeconds;

            if (priority < 0)
                priority += 86400; 

            alarmQueue.Add(alarm, priority);
        }

        public Alarm GetNextAlarm()
        {
            return alarmQueue.IsEmpty() ? null : alarmQueue.Head();
        }

        public void RemoveNextAlarm()
        {
            if (!alarmQueue.IsEmpty())
                alarmQueue.Remove();
        }

        public bool HasAlarms()
        {
            return !alarmQueue.IsEmpty();
        }

        public Alarm PopIfDue(TimeSpan currentTimeOfDay)
        {
            if (!alarmQueue.IsEmpty())
            {
                var nextAlarm = alarmQueue.Head();
                if (currentTimeOfDay >= nextAlarm.Time)
                {
                    alarmQueue.Remove();
                    return nextAlarm;
                }
            }
            return null;
        }

        public List<Alarm> GetAllAlarms()
        {
            var alarms = new List<Alarm>();

            var field = typeof(HeapPriorityQueue<Alarm>).GetField("heap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var internalHeap = (ValueTuple<Alarm, int>[])field.GetValue(alarmQueue);

            var sizeField = typeof(HeapPriorityQueue<Alarm>).GetField("size", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            int heapSize = (int)sizeField.GetValue(alarmQueue);

            for (int i = 0; i < heapSize; i++)
            {
                alarms.Add(internalHeap[i].Item1);
            }

            alarms.Sort();
            return alarms;
        }

        public void SetAlarms(List<Alarm> alarms)
        {
            alarmQueue = new HeapPriorityQueue<Alarm>(100);
            foreach (var alarm in alarms)
            {
                AddAlarm(alarm);
            }
        }
    }
}