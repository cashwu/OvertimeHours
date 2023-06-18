namespace OvertimeHour;

public class OvertimeSetting
{
    public OvertimeSetting(Period period, Rate rate)
    {
        Period = period;
        Rate = rate;
    }

    public Period Period { get; }

    public Rate Rate { get; }
}