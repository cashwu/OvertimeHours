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

    public int RealRate(bool anyDayOvertime)
    {
        var rateNight = anyDayOvertime
                            ? Rate.NightWithDayOvertime
                            : Rate.Night;

        return Rate.Type == EnumRateType.Day
                   ? Rate.Day
                   : rateNight;
    }
}