namespace OvertimeHour;

public class OvertimePeriodSetting
{
    public OvertimePeriodSetting(Period period, int dayRate, int nightRate, int nightRateWithDayOvertime = 0)
    {
        Period = period;
        DayRate = dayRate;
        NightRate = nightRate;
        NightRateWithDayOvertime = nightRateWithDayOvertime;
    }

    public Period Period { get; }

    public int DayRate { get; }

    public int NightRate { get; }

    public int NightRateWithDayOvertime { get; }
}