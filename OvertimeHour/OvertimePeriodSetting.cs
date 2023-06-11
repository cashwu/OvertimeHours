namespace OvertimeHour;

public class OvertimePeriodSetting
{
    public OvertimePeriodSetting(Period period, int dayRate, int nightRate, int nightRateDayWithOvertime = 0)
    {
        Period = period;
        DayRate = dayRate;
        NightRate = nightRate;
        NightRateDayWithOvertime = nightRateDayWithOvertime;
    }

    public Period Period { get; }

    public int DayRate { get; }

    public int NightRate { get; }

    public int NightRateDayWithOvertime { get; }
}