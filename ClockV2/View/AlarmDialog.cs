using System.Windows.Forms;
using System;
using ClockV2.Model;
using PriorityQueue;

namespace ClockV2.View
{
    public partial class AlarmDialog : Form
    {
        public TimeSpan AlarmTime => dateTimePicker.Value.TimeOfDay;
        public string AlarmLabel => textBoxLabel.Text;

        public AlarmDialog()
        {
            InitializeComponent();

            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.textBoxLabel = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.CustomFormat = "HH:mm";
            this.dateTimePicker.Location = new System.Drawing.Point(15, 15);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowUpDown = true;
            this.dateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker.TabIndex = 0;
            // 
            // textBoxLabel
            // 
            this.textBoxLabel.Location = new System.Drawing.Point(15, 50);
            this.textBoxLabel.Name = "textBoxLabel";
            this.textBoxLabel.Size = new System.Drawing.Size(200, 20);
            this.textBoxLabel.TabIndex = 1;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(15, 90);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(100, 90);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            // 
            // AlarmDialog
            // 
            this.ClientSize = new System.Drawing.Size(230, 130);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.textBoxLabel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Name = "AlarmDialog";
            this.Text = "Set Alarm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private DateTimePicker dateTimePicker;
        private TextBox textBoxLabel;
        private Button buttonOk;
        private Button buttonCancel;
    }
}