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

        var insertOvertime = new List<OvertimePeriod>();

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
                anyDayOvertime = insertOvertime.Any(a => a.Type == EnumRateType.Day);
            }
            else
            {
                anyDayOvertime = insertOvertime.Concat(historyOvertimePeriod).Any(a => a.Type == EnumRateType.Day);
            }

            insertOvertime.Add(new OvertimePeriod(period, setting.Rate, anyDayOvertime));
        }

        // recover 
        var updateOvertime = new List<OvertimePeriod>();

        if (historyOvertimePeriod != null)
        {
            var historyNightRateOvertimes = historyOvertimePeriod.Where(a => a.Type == EnumRateType.Night).ToList();
            var historyHasNotDayRate = historyOvertimePeriod.Any(a => a.Type == EnumRateType.Day) == false;
            var currentHasDayRate = insertOvertime.Any(a => a.Type == EnumRateType.Day);

            if (historyNightRateOvertimes.Any()
                && historyHasNotDayRate
                && currentHasDayRate)
            {
                // check setting have history cross day
                var maxHistoryOvertimeEnd = historyNightRateOvertimes.Max(a => a.End);
                var currentHistoryOvertimeEnd = insertOvertime.Max(a => a.End);

                if (maxHistoryOvertimeEnd.Date != currentHistoryOvertimeEnd.Date)
                {
                    overtimeSettings.AddRange(OvertimeSettings(maxHistoryOvertimeEnd));
                }

                foreach (var nightRateOvertime in historyNightRateOvertimes)
                {
                    updateOvertime.AddRange(overtimeSettings.Select(a => new
                                                            {
                                                                Setting = a,
                                                                NewPeriod = a.Period.OverlapPeriod(nightRateOvertime.ToPeriod())
                                                            })
                                                            .Where(a => a.NewPeriod != null)
                                                            .Select(a => new OvertimePeriod(a.NewPeriod, a.Setting.Rate, true))
                                                            .ToList());
                }
            }
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