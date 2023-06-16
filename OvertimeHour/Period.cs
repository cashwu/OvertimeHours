using System.Globalization;

namespace OvertimeHour;

public class Period
{
    public Period(DateTime baseDate, string start, string end)
    {
        BaseDate = baseDate;
        OriginStart = start;
        OriginEnd = end;

        var endDate = end == "00:00" ? baseDate.AddDays(1) : baseDate;

        Start = ParseToDateTime(baseDate, start);
        End = ParseToDateTime(endDate, end);
    }

    public Period(DateTime start, DateTime end)
    {
        BaseDate = start;
        OriginStart = start.ToString("HH:mm");
        OriginEnd = end.ToString("HH:mm");
        
        Start = start;
        End = end;
    }

    public Period(string start, string end)
    {
        OriginStart = start;
        OriginEnd = end;
    }

    public DateTime BaseDate { get; set; }

    public string OriginStart { get; set; }

    public string OriginEnd { get; set; }

    public DateTime Start { get; }

    public DateTime End { get; }

    public bool IsCrossDay => End <= Start;

    public Period OverlapPeriod(Period another)
    {
        if (IsTimeOverlap(another) == false)
        {
            return default;
        }

        var start = Start > another.Start ? OriginStart : another.OriginStart;
        var end = End < another.End ? OriginEnd : another.OriginEnd;

        return new Period(BaseDate, start, end);
    }

    private static DateTime ParseToDateTime(DateTime date, string start)
    {
        return DateTime.ParseExact($"{date:yyyy/MM/dd} {start}", "yyyy/MM/dd HH:mm", new DateTimeFormatInfo());
    }

    private bool IsTimeOverlap(Period another)
    {
        return Start < another.End && another.Start < End;
    }
}