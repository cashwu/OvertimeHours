using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using OvertimeHour.Enums;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeHandlerTests
{
    private readonly OvertimeHandler _overtimeHandler;

    public OvertimeHandlerTests()
    {
        _overtimeHandler = new OvertimeHandler(GivenOvertimeSettings(), GivenCalenderSettings());
    }

    /// <summary>
    /// workday
    /// 
    /// overtime
    /// 18 - 20
    ///
    /// real overtime rate
    /// 18 - 20 (150)
    /// </summary>
    [Fact]
    public void workday_day_overlap_not_cross_day()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 01, 18,
                                                 06, 01, 20);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 18, 00, 00),
                End = new DateTime(2023, 06, 01, 20, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            }
        });
    }

    /// <summary>
    /// workday
    /// 
    /// overtime
    /// 22 - 23
    ///
    /// real overtime rate
    /// 22 - 23 (200)
    /// </summary>
    [Fact]
    public void workday_night_overlap_not_cross_day()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 01, 22,
                                                 06, 01, 23);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 22, 00, 00),
                End = new DateTime(2023, 06, 01, 23, 00, 00),
                Rate = 200,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// workday -> workday
    /// 
    /// overtime
    /// 22 - 01
    ///
    /// real overtime rate
    /// 22 - 00 (200), 00 - 01 (200)
    /// </summary>
    [Fact]
    public void workday_night_overlap_cross_workday()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 01, 22,
                                                 06, 02, 01);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 22, 00, 00),
                End = new DateTime(2023, 06, 02, 00, 00, 00),
                Rate = 200,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 02, 00, 00, 00),
                End = new DateTime(2023, 06, 02, 01, 00, 00),
                Rate = 200,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// workday
    /// 
    /// overtime
    /// 20 - 23
    ///
    /// real overtime rate
    /// 20 - 22 (150), 22 - 23 (210)
    /// </summary>
    [Fact]
    public void workday_day_and_night_overlap_not_cross_day()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 01, 20,
                                                 06, 01, 23);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 20, 00, 00),
                End = new DateTime(2023, 06, 01, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = new DateTime(2023, 06, 01, 22, 00, 00),
                End = new DateTime(2023, 06, 01, 23, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// workday -> workday
    /// 
    /// overtime
    /// 20 - 01
    ///
    /// real overtime rate
    /// 20 - 22 (150), 22 - 00 (210), 00 - 01 (210)
    /// </summary>
    [Fact]
    public void workday_day_and_night_overlap_cross_workday()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 01, 20,
                                                 06, 02, 01);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 20, 00, 00),
                End = new DateTime(2023, 06, 01, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = new DateTime(2023, 06, 01, 22, 00, 00),
                End = new DateTime(2023, 06, 02, 00, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 02, 00, 00, 00),
                End = new DateTime(2023, 06, 02, 01, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// workday -> holiday
    /// 
    /// overtime
    /// 22 - 01
    ///
    /// real overtime rate
    /// 22 - 00 (workday 200), 00 - 01 (holiday 390)
    /// </summary>
    [Fact]
    public void workday_night_overlap_cross_holiday()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 02, 22,
                                                 06, 03, 01);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 02, 22, 00, 00),
                End = new DateTime(2023, 06, 03, 00, 00, 00),
                Rate = 200,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 03, 00, 00, 00),
                End = new DateTime(2023, 06, 03, 01, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// workday -> holiday
    /// 
    /// overtime
    /// 20 - 01
    ///
    /// real overtime rate
    /// 20 - 22 (workday 150), 22 - 00 (workday 200), 00 - 01 (holiday 390)
    /// </summary>
    [Fact]
    public void workday_day_and_night_overlap_cross_holiday()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 02, 20,
                                                 06, 03, 01);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 02, 20, 00, 00),
                End = new DateTime(2023, 06, 02, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = new DateTime(2023, 06, 02, 22, 00, 00),
                End = new DateTime(2023, 06, 03, 00, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 03, 00, 00, 00),
                End = new DateTime(2023, 06, 03, 01, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// holiday -> workday
    /// 
    /// overtime
    /// 22 - 01
    ///
    /// real overtime rate
    /// 22 - 00 (holiday 390), 00 - 01 (workday 200)
    /// </summary>
    [Fact]
    public void holiday_night_overlap_cross_workday()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 03, 22,
                                                 06, 04, 01);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 22, 00, 00),
                End = new DateTime(2023, 06, 04, 00, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 04, 00, 00, 00),
                End = new DateTime(2023, 06, 04, 01, 00, 00),
                Rate = 200,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// holiday -> workday-
    /// 
    /// overtime
    /// 20 - 01
    ///
    /// real overtime rate
    /// 20 - 22 (holiday 300), 22 - 00 (holiday 390), 00 - 01 (holiday 210)
    /// </summary>
    [Fact]
    public void holiday_day_and_night_overlap_cross_workday()
    {
        var overtimePeriod = GivenOvertimePeriod(06, 03, 20,
                                                 06, 04, 01);

        var overtimes = _overtimeHandler.Handler(overtimePeriod);

        overtimes.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 20, 00, 00),
                End = new DateTime(2023, 06, 03, 22, 00, 00),
                Rate = 300,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = new DateTime(2023, 06, 03, 22, 00, 00),
                End = new DateTime(2023, 06, 04, 00, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 04, 00, 00, 00),
                End = new DateTime(2023, 06, 04, 01, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            }
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