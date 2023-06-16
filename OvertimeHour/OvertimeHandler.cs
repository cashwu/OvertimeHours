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
        var overtimeSettingType = calenderSetting.ToOvertimeSettingType();
        var overtimeSetting = _overtimeSettings.FirstOrDefault(a => a.Type == overtimeSettingType);

        // set base date
        overtimeSetting.SetBaseDate(overtimeForm.Period.Start.Date);

        // split setting
        var newOvertimeSetting = overtimeSetting.SplitSetting();

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