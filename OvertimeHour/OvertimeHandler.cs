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
    public (List<OvertimePeriod> insertOvertime, IEnumerable<OvertimePeriod> updateOvertime) Handler(Period overtimePeriod, List<OvertimePeriod> historyOvertimePeriod = null)
    {
        var overtimeSettings = OvertimeSettings(overtimePeriod.Start);

        var secondDate = overtimePeriod.IsCrossDay
                             ? overtimePeriod.End
                             : overtimePeriod.Start.Date.AddDays(1);

        overtimeSettings.AddRange(OvertimeSettings(secondDate));

        var insertOvertime = new List<OvertimePeriod>();

        // get real overtime
        foreach (var setting in overtimeSettings)
        {
            var period = setting.Period.OverlapPeriod(overtimePeriod);

            if (period == null)
            {
                continue;
            }

            var overtimePeriods = historyOvertimePeriod == null
                                      ? insertOvertime
                                      : insertOvertime.Concat(historyOvertimePeriod);

            var anyDayOvertime = overtimePeriods.Any(a => a.Type == EnumRateType.Day);

            insertOvertime.Add(new OvertimePeriod(period, setting.Rate, anyDayOvertime));
        }

        // recover 
        if (historyOvertimePeriod == null)
        {
            return (insertOvertime, Enumerable.Empty<OvertimePeriod>());
        }

        var currentHasDayRate = insertOvertime.Any(a => a.Type == EnumRateType.Day);

        if (currentHasDayRate == false)
        {
            return (insertOvertime, Enumerable.Empty<OvertimePeriod>());
        }

        var historyNightRateOvertimes = historyOvertimePeriod.Where(a => a.Type == EnumRateType.Night).ToList();

        if (historyNightRateOvertimes.Any() == false)
        {
            return (insertOvertime, Enumerable.Empty<OvertimePeriod>());
        }

        var updateOvertime = new List<OvertimePeriod>();

        // check every history
        foreach (var nightRateOvertime in historyNightRateOvertimes)
        {
            updateOvertime.AddRange(overtimeSettings.Select(a => new
                                                    {
                                                        Setting = a,
                                                        NewPeriod = a.Period.OverlapPeriod(nightRateOvertime.ToNewPeriod())
                                                    })
                                                    .Where(a => a.NewPeriod != null)
                                                    .Select(a => new OvertimePeriod(a.NewPeriod, a.Setting.Rate, true))
                                                    .ToList());
        }

        return (insertOvertime, updateOvertime);
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