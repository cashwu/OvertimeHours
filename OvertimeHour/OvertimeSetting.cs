using OvertimeHour.Enums;

namespace OvertimeHour;

public class OvertimeSetting
{
    public OvertimeSetting(Period period, Rate rate, EnumOvertimeSettingType type)
    {
        Period = period;
        Rate = rate;
        Type = type;
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

    public OvertimeSettings SetBaseDate(DateTime baseDate)
    {
        var dayPeriod = new Period(baseDate, DaySetting.period.OriginStart, DaySetting.period.OriginEnd);
        var daySetting = new OvertimeSetting(dayPeriod, DaySetting.rate, Type);

        var nightPeriod = new Period(baseDate, NightSetting.period.OriginStart, NightSetting.period.OriginEnd);
        var nightSetting = new OvertimeSetting(nightPeriod, NightSetting.rate, Type);

        return new OvertimeSettings(daySetting, nightSetting);
    }
}