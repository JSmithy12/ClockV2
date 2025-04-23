using System;

public class Alarm : IComparable<Alarm>
{
    public TimeSpan Time { get; set; }
    public string Label { get; set; }

    public Alarm(TimeSpan time, string label)
    {
        Time = time;
        Label = label;
    }

    public double Priority => Time.TotalSeconds;

    public int CompareTo(Alarm other)
    {
        return Time.CompareTo(other.Time);
    }
   
    // Return a string representation of the alarm in the format "hh:mm - Label"
    public override string ToString()
    {
        return $"{Time:hh\\:mm} - {Label}";
    }
}
