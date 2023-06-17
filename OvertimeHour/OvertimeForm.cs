namespace OvertimeHour;

public class OvertimeForm
{
    public OvertimeForm(DateTime start, DateTime end)
    {
        Period = new Period(start, end);
    }

    public Period Period { get; }

    public bool IsCrossDay => Period.IsCrossDay;
}