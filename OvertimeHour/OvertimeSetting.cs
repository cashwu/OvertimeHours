using OvertimeHour.Enums;

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

    public (Period period, Rate rate) DaySetting { get; private set; }

    public (Period period, Rate rate) NightSetting { get; private set; }

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

    public void SetBaseDate(DateTime baseDate)
    {
        var dayPeriod = new Period(baseDate, DaySetting.period.OriginStart, DaySetting.period.OriginEnd);
        DaySetting = (dayPeriod, DaySetting.rate);

        var nightPeriod = new Period(baseDate, NightSetting.period.OriginStart, NightSetting.period.OriginEnd);
        NightSetting = (nightPeriod, NightSetting.rate);
    }

    public void SplitSetting()
    {
        throw new NotImplementedException();
    }
}