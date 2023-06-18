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
                Rate = setting.RealRate(anyDayOvertime, setting.Type),
                Type = setting.Rate.Type
            });
        }

        return result;
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    private IEnumerable<OvertimeSetting> OvertimeSetting(OvertimeForm overtimeForm)
    {
        // get overtime day type
        var startDateCalenderSetting = _calenderSettings.FirstOrDefault(a => a.Date == overtimeForm.Period.Start.Date);
        var overtimeSettingForStartDate = OvertimeSetting(overtimeForm.Period.Start.Date, startDateCalenderSetting.OvertimeSettingType);

        if (overtimeForm.IsCrossDay == false)
        {
            return overtimeSettingForStartDate;
        }

        var endDateCalenderSetting = _calenderSettings.FirstOrDefault(a => a.Date == overtimeForm.Period.End.Date);

        // cross day and difference type 
        if (startDateCalenderSetting.Type != endDateCalenderSetting.Type)
        {
            // // remove cross day data
            overtimeSettingForStartDate.RemoveAll(a => a.Period.Start.Date == overtimeForm.Period.End.Date);

            var overtimeSettingForEndDate = OvertimeSetting(overtimeForm.Period.End.Date, endDateCalenderSetting.OvertimeSettingType);

            overtimeSettingForEndDate.ForEach(setting =>
            {
                if (setting.Period.Start.Date == overtimeForm.Period.End.Date.AddDays(1))
                {
                    setting.Period.Start = setting.Period.Start.AddDays(-1);
                    setting.Period.End = setting.Period.End.AddDays(-1);
                }
            });

            return overtimeSettingForStartDate.Concat(overtimeSettingForEndDate);
        }
        else
        {
            var overtimeSettingForEndDate = OvertimeSetting(overtimeForm.Period.End.Date, endDateCalenderSetting.OvertimeSettingType);

            return overtimeSettingForStartDate.Concat(overtimeSettingForEndDate);
        }
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    private OvertimeSettings OvertimeSetting(DateTime date, EnumOvertimeSettingType overtimeSettingType)
    {
        // get overtime setting by type
        var overtimeSetting = _overtimeSettings.FirstOrDefault(a => a.Type == overtimeSettingType);

        return overtimeSetting.SetBaseDate(date);
    }
}