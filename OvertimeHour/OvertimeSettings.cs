namespace OvertimeHour;

public class OvertimeSettings : List<OvertimeSetting>
{
    public OvertimeSettings(params OvertimeSetting[] overtimePeriodSettings)
    {
        foreach (var overtimePeriodSetting in overtimePeriodSettings)
        {
            if (overtimePeriodSetting.Period is not null
                && overtimePeriodSetting.Period.IsCrossDay)
            {
                var period = new Period(overtimePeriodSetting.Period.BaseDate, overtimePeriodSetting.Period.OriginStart, "00:00");

                Add(new OvertimeSetting(period, overtimePeriodSetting.Rate));

                var crossDayPeriod = new Period(overtimePeriodSetting.Period.BaseDate.AddDays(1), "00:00", overtimePeriodSetting.Period.OriginEnd);

                Add(new OvertimeSetting(crossDayPeriod, overtimePeriodSetting.Rate));
            }
            else
            {
                Add(overtimePeriodSetting);
            }
        }
    }

    public List<Overtime> CreateOvertime(Period overTimePeriod)
    {
        var result = new List<Overtime>();

        foreach (var overtimeSetting in this)
        {
            var period = overtimeSetting.Period.OverlapPeriod(overTimePeriod);

            if (period == null)
            {
                continue;
            }

            var anyDayOvertime = result.Any(a => a.Type == EnumRateType.Day);

            result.Add(new Overtime
            {
                Start = period.Start,
                End = period.End,
                Rate = overtimeSetting.RealRate(anyDayOvertime),
                Type = overtimeSetting.Rate.Type
            });
        }

        return result;
    }
}