using ClockV2.Model;
using System;
using System.Timers;
using System.Windows.Forms;

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

    }
}
