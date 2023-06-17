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
    public List<OvertimePeriod> Handler(OvertimeForm overtimeForm)
    {
        var result = new List<OvertimePeriod>();

        var overtimeSettings = OvertimeSetting(overtimeForm);

        // get real overtime
        foreach (var setting in overtimeSettings)
        {
            var period = setting.Period.OverlapPeriod(overtimeForm.Period);

            if (period == null)
            {
                continue;
            }

            var anyDayOvertime = result.Any(a => a.Type == EnumRateType.Day);

            result.Add(new OvertimePeriod
            {
                Start = period.Start,
                End = period.End,
                Rate = setting.RealRate(anyDayOvertime),
                Type = setting.Rate.Type
            });
        }

        return result;
    }

    private IEnumerable<OvertimeSetting> OvertimeSetting(OvertimeForm overtimeForm)
    {
        var overtimeSettingForStartDate = OvertimeSetting(overtimeForm.Period.Start.Date);

        if (overtimeForm.IsCrossDay == false)
        {
            return overtimeSettingForStartDate;
        }

        var overtimeSettingForEndDate = OvertimeSetting(overtimeForm.Period.End.Date);

        return overtimeSettingForStartDate.Concat(overtimeSettingForEndDate);
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    private OvertimeSettings OvertimeSetting(DateTime date)
    {
        // get overtime day type
        var calenderSetting = _calenderSettings.FirstOrDefault(a => a.Date == date);
        var overtimeSettingType = calenderSetting.ToOvertimeSettingType();

        // get overtime setting by type
        var overtimeSetting = _overtimeSettings.FirstOrDefault(a => a.Type == overtimeSettingType);

        return overtimeSetting.SetBaseDate(date);
    }
}