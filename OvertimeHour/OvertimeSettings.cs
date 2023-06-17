namespace OvertimeHour;

public class OvertimeSettings : List<OvertimeSetting>
{
    public OvertimeSettings(params OvertimeSetting[] overtimePeriodSettings)
    {
        foreach (var overtimePeriodSetting in overtimePeriodSettings)
        {
            if (overtimePeriodSetting.Period is not null
                && overtimePeriodSetting.Period.IsSettingCrossDay)
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
}