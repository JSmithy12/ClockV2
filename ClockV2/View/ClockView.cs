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

        private HeapPriorityQueue<Alarm> alarmQueue = new HeapPriorityQueue<Alarm>(20); // or another capacity

        public ClockView()
        {
            InitializeComponent();

            Panel_Clock.GetType()
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(Panel_Clock, true, null);

            currentTime = DateTime.Now;

            InitialiseAlarmUI();
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
        }

        private void SetAlarmButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new AlarmDialog(alarmQueue))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    presenter.AddAlarm(dialog.AlarmTime, dialog.AlarmLabel);
                }
            }
        }
    }
}
