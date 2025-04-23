using ClockV2.Helpers;
using ClockV2.Model;
using ClockV2.Presenter;
using ClockV2.View;
using PriorityQueue; // Add this to access HeapPriorityQueue
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClockV2
{
    public partial class ClockView : Form
    {
        private Button setAlarmButton;
        private ClockPresenter presenter;
        private readonly ClockDrawingHelper drawingHelper = new ClockDrawingHelper();
        private DateTime currentTime;
        private ListBox alarmListBox;

        public ClockView()
        {
            InitializeComponent();

            Panel_Clock.GetType()
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(Panel_Clock, true, null);

            currentTime = DateTime.Now;

            InitialiseAlarmUI();
            //Asks users if they want to load alarm preset first
            var result = MessageBox.Show("Load saved alarms?", "Load", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var ofd = new OpenFileDialog { Filter = "iCalendar Files|*.ics" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        this.Load += (s, e) =>
                        {
                            presenter.LoadAlarms(ofd.FileName);
                            DisplayAlarms(presenter.GetAllAlarms());
                        };
                    }
                }
            }
        }
        public void SetPresenter(ClockPresenter presenter)
        {
            this.presenter = presenter;
        }

        public void UpdateClock(DateTime currentTime)
        {
            this.currentTime = currentTime;
            Panel_Clock.Invalidate();
        }   

        private void Panel_Clock_Paint(object sender, PaintEventArgs e)
        {
            if (presenter == null) return;

            var g = e.Graphics;
            drawingHelper.DrawClock(g, currentTime, Panel_Clock.Width, Panel_Clock.Height);
        }

        private void InitialiseAlarmUI()
        {
            setAlarmButton = new Button
            {
                Text = "Set Alarm",
                Location = new Point(10, Panel_Clock.Bottom + 10),
                Size = new Size(100, 30)
            };
            setAlarmButton.Click += SetAlarmButton_Click;
            this.Controls.Add(setAlarmButton);

            alarmListBox = new ListBox
            {
                Location = new Point(setAlarmButton.Right + 20, setAlarmButton.Top),
                Size = new Size(200, 100)
            };

            this.Controls.Add(alarmListBox);
        }

        public void DisplayAlarms(List<Alarm> alarms)
        {
            alarmListBox.Items.Clear();

            foreach (var alarm in alarms)
            {
                alarmListBox.Items.Add($"{alarm.Time:hh\\:mm} - {alarm.Label}");
            }
        }
        private void SetAlarmButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new AlarmDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var selectedTime = dialog.AlarmTime;
                    var label = dialog.AlarmLabel;

                    // Add the alarm through the presenter
                    presenter.AddAlarm(DateTime.Today + selectedTime, label);

                    // Refresh the display
                    DisplayAlarms(presenter.GetAllAlarms());
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //Asks user if they want to save current alarms as preset for next time
            var result = MessageBox.Show("Save alarms before exiting?", "Save", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                using (var sfd = new SaveFileDialog { Filter = "iCalendar Files|*.ics" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        presenter.SaveAlarms(sfd.FileName);
                    }
                }
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }

            base.OnFormClosing(e);
        }
    }
}