namespace OvertimeHour;

public class OvertimePeriodSettings : List<OvertimePeriodSetting>
{
    public OvertimePeriodSettings(params OvertimePeriodSetting[] overtimePeriodSettings)
    {
        foreach (var overtimePeriodSetting in overtimePeriodSettings)
        {
            if (overtimePeriodSetting.Period.IsCrossDay)
            {
                var period = new Period(overtimePeriodSetting.Period.BaseDate, overtimePeriodSetting.Period.OriginStart, "00:00");

                Add(new OvertimePeriodSetting(period,
                                              overtimePeriodSetting.DayRate,
                                              overtimePeriodSetting.NightRate,
                                              overtimePeriodSetting.NightRateWithDayOvertime));

                var crossDayPeriod = new Period(overtimePeriodSetting.Period.BaseDate.AddDays(1), "00:00", overtimePeriodSetting.Period.OriginEnd);

                Add(new OvertimePeriodSetting(crossDayPeriod,
                                              overtimePeriodSetting.DayRate,
                                              overtimePeriodSetting.NightRate,
                                              overtimePeriodSetting.NightRateWithDayOvertime));
            }
            else
            {
                Add(overtimePeriodSetting);
            }
        }
    }

    public IEnumerable<Period> SplitPeriod(Period overTimePeriod)
    {
        foreach (var overtimePeriodSetting in this)
        {
            var overTimeSettingPeriod = overtimePeriodSetting.Period.OverlapPeriod(overTimePeriod);

            if (overTimeSettingPeriod != null)
            {
                yield return overTimeSettingPeriod;
            }
        }
    }
}