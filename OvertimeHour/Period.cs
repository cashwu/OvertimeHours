namespace OvertimeHour;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime Start { get; }

    public DateTime End { get; }

    public bool IsCrossDay => Start.Date != End.Date;

    public Period OverlapPeriod(Period another)
    {
        if (IsOverlap(another) == false)
        {
            return default;
        }

        var start = Start > another.Start ? Start : another.Start;
        var end = End < another.End ? End : another.End;

        return new Period(start, end);
    }

    private bool IsOverlap(Period another)
    {
        return Start < another.End && another.Start < End;
    }
}