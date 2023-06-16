namespace OvertimeHour;

public class OvertimeSetting
{
    public OvertimeSetting(Period period, Rate rate)
    {
        Period = period;
        Rate = rate;
    }

    public OvertimeSetting((Period period, Rate rate) daySetting,
                           (Period period, Rate rate) nightSetting,
                           EnumOvertimeSettingType type)
    {
        DaySetting = daySetting;
        NightSetting = nightSetting;
        Type = type;
    }

    public (Period period, Rate rate) DaySetting { get; }

    public (Period period, Rate rate) NightSetting { get; }

    public EnumOvertimeSettingType Type { get; }

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