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
            // Set max capacity to 100 alarms, or choose as needed
            alarmQueue = new HeapPriorityQueue<Alarm>(100);
        }

        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public void AddAlarm(Alarm alarm)
        {
            // Priority: how soon it is (lower = sooner)
            int priority = (int)(alarm.Time - DateTime.Now).TotalSeconds;

            // Ensure future alarms only
            if (priority < 0)
                throw new ArgumentException("Alarm time must be in the future.");

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

        public Alarm PopIfDue(DateTime currentTime)
        {
            if (!alarmQueue.IsEmpty())
            {
                var nextAlarm = alarmQueue.Head();
                if (nextAlarm.Time <= currentTime)
                {
                    alarmQueue.Remove();
                    return nextAlarm;
                }
            }
            return null;
        }
    }
}
