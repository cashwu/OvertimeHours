using System.Diagnostics.CodeAnalysis;
using OvertimeHour.Enums;

namespace OvertimeHour;

public class OvertimeHandler
{
    private readonly List<OvertimeSettingFromDb> _overtimeSettingFromDbs;
    private readonly List<CalenderSetting> _calenderSettings;

    public OvertimeHandler(List<OvertimeSettingFromDb> overtimeSettingFromDbFromDbs, List<CalenderSetting> calenderSettings)
    {
        _overtimeSettingFromDbs = overtimeSettingFromDbFromDbs;
        _calenderSettings = calenderSettings;
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public (List<OvertimePeriod> insertOvertime, List<OvertimePeriod> updateOvertime) Handler(Period overtimePeriod, List<OvertimePeriod> historyOvertimePeriod = null)
    {
        var overtimeSettings = OvertimeSettings(overtimePeriod.Start);

        if (overtimePeriod.IsCrossDay)
        {
            overtimeSettings.AddRange(OvertimeSettings(overtimePeriod.End));
        }

        if (historyOvertimePeriod != null)
        {
            // TODO check history type data 
        }

        var result = new List<OvertimePeriod>();

        // get real overtime
        foreach (var setting in overtimeSettings)
        {
            var period = setting.Period.OverlapPeriod(overtimePeriod);

            if (period == null)
            {
                continue;
            }

            bool anyDayOvertime;

            if (historyOvertimePeriod == null)
            {
                anyDayOvertime = result.Any(a => a.Type == EnumRateType.Day);
            }
            else
            {
                anyDayOvertime = result.Concat(historyOvertimePeriod).Any(a => a.Type == EnumRateType.Day);
            }

            result.Add(new OvertimePeriod(period, setting.Rate, anyDayOvertime));
        }

        return (result, new List<OvertimePeriod>());
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    private List<OvertimeSetting> OvertimeSettings(DateTime dateTime)
    {
        var calenderSetting = _calenderSettings.FirstOrDefault(a => a.Date == dateTime.Date);

        return _overtimeSettingFromDbs.Where(a => a.Type == calenderSetting.OvertimeSettingType)
                                      .Select(a => a.ToOvertimeSetting(dateTime.Date))
                                      .SelectMany(a => a)
                                      .ToList();
    }
}