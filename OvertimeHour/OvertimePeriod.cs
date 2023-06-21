using OvertimeHour.Enums;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace OvertimeHour;

public class OvertimePeriod
{
    public OvertimePeriod()
    {
    }

    public OvertimePeriod(Period period, Rate rate, bool anyDayOvertime)
    {
        Start = period.Start;
        End = period.End;
        Rate = rate.RealRate(anyDayOvertime);
        Type = rate.Type;
    }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public int Rate { get; set; }

    public EnumRateType Type { get; init; }

    public Period ToPeriod()
    {
        return new Period(Start, End);
    }
}