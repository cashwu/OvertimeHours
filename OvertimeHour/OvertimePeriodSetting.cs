namespace OvertimeHour;

public class OvertimePeriodSetting
{
    public OvertimePeriodSetting(Period period, OvertimeRate overtimeRate)
    {
        Period = period;
        OvertimeRate = overtimeRate;
    }

    public Period Period { get; }

    public OvertimeRate OvertimeRate { get; }
}