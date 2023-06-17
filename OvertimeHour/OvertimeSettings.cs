namespace OvertimeHour;

public class OvertimeSettings : List<OvertimeSetting>
{
    public OvertimeSettings(params OvertimeSetting[] overtimePeriodSettings)
    {
        foreach (var setting in overtimePeriodSettings)
        {
            if (setting.Period is not null
                && setting.Period.IsSettingCrossDay)
            {
                var period = new Period(setting.Period.BaseDate, setting.Period.OriginStart, "00:00");

                Add(new OvertimeSetting(period, setting.Rate, setting.Type));

                var crossDayPeriod = new Period(setting.Period.BaseDate.AddDays(1), "00:00", setting.Period.OriginEnd);

                Add(new OvertimeSetting(crossDayPeriod, setting.Rate, setting.Type));
            }
            else
            {
                Add(setting);
            }
        }
    }
}