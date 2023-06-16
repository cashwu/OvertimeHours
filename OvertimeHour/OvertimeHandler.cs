using System.Diagnostics.CodeAnalysis;
using OvertimeHour.Enums;

namespace OvertimeHour;

public class OvertimeHandler
{
    private readonly OvertimeSettings _overtimeSettings;
    private readonly CalenderSettings _calenderSettings;

    public OvertimeHandler(OvertimeSettings overtimeSettings, CalenderSettings calenderSettings)
    {
        _overtimeSettings = overtimeSettings;
        _calenderSettings = calenderSettings;
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public List<Overtime> Handler(OvertimeForm overtimeForm)
    {
        var result = new List<Overtime>();

        // get overtime day type
        var calenderSetting = _calenderSettings.FirstOrDefault(a => a.Date == overtimeForm.Period.Start.Date);

        // get overtime setting by type

        var calenderSettingType = calenderSetting.Type;
        EnumOvertimeSettingType overtimeSettingType = 0;

        if (calenderSettingType == EnumCalenderType.Workday)
        {
            overtimeSettingType = EnumOvertimeSettingType.Workday;
        }

        var overtimeSetting = _overtimeSettings.FirstOrDefault(a => a.Type == overtimeSettingType);

        // set base date
        overtimeSetting.SetBaseDate(overtimeForm.Period.Start.Date);

        // split setting

        // how ??

        // get real overtime

        var period = overtimeSetting.DaySetting.period.OverlapPeriod(overtimeForm.Period);

        if (period != null)
        {
            // var anyDayOvertime = result.Any(a => a.Type == EnumRateType.Day);

            result.Add(new Overtime
            {
                Start = period.Start,
                End = period.End,
                Rate = overtimeSetting.DaySetting.rate.Day,
                Type = overtimeSetting.DaySetting.rate.Type
            });
        }

        return result;
    }
}