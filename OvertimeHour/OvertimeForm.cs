namespace OvertimeHour;

public class OvertimeForm
{
    public OvertimeForm(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime Start { get; }

    public DateTime End { get; }
}