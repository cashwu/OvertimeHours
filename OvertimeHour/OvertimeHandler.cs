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
        var calenderSetting01 = _calenderSettings.FirstOrDefault(a => a.Date == overtimeForm.Period.Start.Date);

        var calenderSetting02 = default(CalenderSetting);

        if (overtimeForm.Period.Start.Date != overtimeForm.Period.End.Date)
        {
            calenderSetting02 = _calenderSettings.FirstOrDefault(a => a.Date == overtimeForm.Period.End.Date);
        }

        // get overtime setting by type
        var overtimeSettingType01 = calenderSetting01.ToOvertimeSettingType();
        var overtimeSetting01 = _overtimeSettings.FirstOrDefault(a => a.Type == overtimeSettingType01);

        var overtimeSetting02 = default(OvertimeSetting);

        if (calenderSetting02 is not null)
        {
            var overtimeSettingType02 = calenderSetting02.ToOvertimeSettingType();
            overtimeSetting02 = _overtimeSettings.FirstOrDefault(a => a.Type == overtimeSettingType02);
        }

        // set base date
        overtimeSetting01 = overtimeSetting01.SetBaseDate(overtimeForm.Period.Start.Date);

        if (overtimeSetting02 is not null)
        {
            overtimeSetting02 = overtimeSetting02.SetBaseDate(overtimeForm.Period.End.Date);
        }

        // split setting
        var newOvertimeSetting01 = overtimeSetting01.SplitSetting();
        var newOvertimeSetting02 = overtimeSetting02?.SplitSetting() ?? Enumerable.Empty<OvertimeSetting>();

        var newOvertimeSetting = newOvertimeSetting01.Concat(newOvertimeSetting02);

        // get real overtime
        foreach (var setting in newOvertimeSetting)
        {
            var period = setting.Period.OverlapPeriod(overtimeForm.Period);

            if (period == null)
            {
                continue;
            }

            var anyDayOvertime = result.Any(a => a.Type == EnumRateType.Day);

            result.Add(new Overtime
            {
                Start = period.Start,
                End = period.End,
                Rate = setting.RealRate(anyDayOvertime),
                Type = setting.Rate.Type
            });
        }

        return result;
    }
}