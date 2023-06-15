namespace OvertimeHour;

public class OvertimePeriodSetting
{
    public OvertimePeriodSetting(Period period, OvertimeRate overtimeRate)
    {
        Period = period;
        DayRate = overtimeRate.DayRate;
        NightRate = overtimeRate.NightRate;
        NightRateWithDayOvertime = overtimeRate.NightRateWithDayOvertime;
    }

    public Period Period { get; }

    public int DayRate { get; }

    public int NightRate { get; }

    public int NightRateWithDayOvertime { get; }
}