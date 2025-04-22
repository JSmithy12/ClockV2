using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockV2.Model
{
    public class Alarm : IComparable<Alarm>
    {
        public DateTime Time { get; set; }
        public string Label { get; set; }

        public int Priority
        {
            get
            {
                return (int)(Time.Hour * 60 + Time.Minute);
            }
        }

        public Alarm(DateTime time, string label)
        {
            Time = time;
            Label = label;
        }

        public int CompareTo(Alarm other)
        {
            return Time.CompareTo(other.Time);
        }

        public override string ToString()
        {
            return $"{Label} at {Time}";
        }
    }
}