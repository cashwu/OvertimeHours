using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using OvertimeHour.Enums;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeHandlerForPreOvertimeTests
{
    private readonly OvertimeHandler _overtimeHandler;

    public OvertimeHandlerForPreOvertimeTests()
    {
        _overtimeHandler = new OvertimeHandler(GivenOvertimeSettings(), GivenCalenderSettings());
    }

    /// <summary>
    /// workday
    /// 
    /// history
    /// 20 - 22
    /// 
    /// overtime
    /// 22 - 23
    ///
    /// real overtime rate
    /// 22 - 23 (210) 
    /// </summary>
    [Fact]
    public void history_day_overtime_workday_night_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 20, 00, 00),
                End = new DateTime(2023, 06, 01, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day // TODO history not type data ??
            }
        };

        var overtimePeriod = GivenOvertimePeriod(06, 01, 22,
                                                 06, 01, 23);

        var (insertOvertime, updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

        insertOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 22, 00, 00),
                End = new DateTime(2023, 06, 01, 23, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            }
        });

        updateOvertime.Should().BeEmpty();
    }

    /// <summary>
    /// workday
    /// 
    /// history
    /// 22 - 23 (200)
    /// 
    /// overtime
    /// 20 - 22
    ///
    /// real overtime rate
    /// 20 - 22 (150)
    ///
    /// history
    /// 22 - 23 (200 -> 210)
    /// </summary>
    [Fact]
    public void history_night_overtime_workday_day_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 22, 00, 00),
                End = new DateTime(2023, 06, 01, 23, 00, 00),
                Rate = 200,
                Type = EnumRateType.Night
            }
        };

        var overtimePeriod = GivenOvertimePeriod(06, 01, 20,
                                                 06, 01, 22);

        (var insertOvertime, var updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

        insertOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 20, 00, 00),
                End = new DateTime(2023, 06, 01, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            },
        });
    }

    private static Period GivenOvertimePeriod(int startMonth, int startDay, int startHour,
                                              int endMonth, int endDay, int endHour)
    {
        return new Period(new DateTime(2023, startMonth, startDay, startHour, 00, 00),
                          new DateTime(2023, endMonth, endDay, endHour, 00, 00));
    }

    private static List<CalenderSetting> GivenCalenderSettings()
    {
        return new List<CalenderSetting>
        {
            new(new DateTime(2023, 06, 01), EnumCalenderType.Workday),
            new(new DateTime(2023, 06, 02), EnumCalenderType.Workday),
            new(new DateTime(2023, 06, 03), EnumCalenderType.Holiday),
            new(new DateTime(2023, 06, 04), EnumCalenderType.Workday)
        };
    }

    private static List<OvertimeSettingFromDb> GivenOvertimeSettings()
    {
        return new List<OvertimeSettingFromDb>
        {
            new("06:00", "22:00", new Rate(150), EnumOvertimeSettingType.Workday),
            new("22:00", "06:00", new Rate(200, 210), EnumOvertimeSettingType.Workday),
            new("06:00", "22:00", new Rate(300), EnumOvertimeSettingType.Holiday),
            new("22:00", "06:00", new Rate(390, 390), EnumOvertimeSettingType.Holiday)
        };
    }
}