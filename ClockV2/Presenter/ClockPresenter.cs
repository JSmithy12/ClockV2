using ClockV2.Model;
using ClockV2.Helpers;
using System;
using System.Timers;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace ClockV2.Presenter
{
    public class ClockPresenter
    {
        private readonly ClockModel model;
        private readonly ClockView view;
        private readonly System.Timers.Timer timer;

        public ClockPresenter(ClockModel model, ClockView view)
        {
            this.model = model;
            this.view = view;

            view.SetPresenter(this);

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += (s, e) => UpdateClock();
            timer.Start();
        }

        private void UpdateClock()
        {
            DateTime currentTime = model.GetCurrentTime();

            Alarm dueAlarm = model.PopIfDue(currentTime);
            if (dueAlarm != null)
            {
                view.Invoke(new Action(() =>
                {
                    MessageBox.Show($"Alarm: {dueAlarm.Label}", "Alarm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }));
            }

            view.Invoke(new Action(() => view.UpdateClock(currentTime)));
        }

        public void AddAlarm(DateTime time, string label)
        {
            model.AddAlarm(new Alarm(time, label));
        }

        public void SaveAlarms(string filePath)
        {
            var alarms = model.GetAllAlarms();
            ICalendarHelper.SaveAlarms(alarms, filePath);
        }

        public void LoadAlarms(string filePath)
        {
            var alarms = ICalendarHelper.LoadAlarms(filePath); 
            model.SetAlarms(alarms);
        }

        public List<Alarm> GetAllAlarms()
        {
            return model.GetAllAlarms();
        }
    }
}