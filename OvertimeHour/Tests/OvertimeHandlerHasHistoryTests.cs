using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using OvertimeHour.Enums;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeHandlerHasHistoryTests
{
    private readonly OvertimeHandler _overtimeHandler;

    public OvertimeHandlerHasHistoryTests()
    {
        _overtimeHandler = new OvertimeHandler(GivenOvertimeSettings(), GivenCalenderSettings());
    }

    /// <summary>
    /// workday
    /// 
    /// history
    /// 20 - 22 (150)
    /// 
    /// overtime
    /// 22 - 23
    ///
    /// real overtime rate
    /// 22 - 23 (210) 
    /// </summary>
    [Fact]
    public void history_day_overtime_and_workday_night_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 01, 20, 00, 00),
                End = new DateTime(2023, 06, 01, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
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
    public void history_night_overtime_and_workday_day_overlap()
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

        var (insertOvertime, updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

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

        updateOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
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
    /// holiday
    /// 
    /// history
    /// 20 - 22 (300)
    /// 
    /// overtime
    /// 22 - 23
    ///
    /// real overtime rate
    /// 22 - 23 (390) 
    /// </summary>
    [Fact]
    public void history_day_overtime_and_holiday_night_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 20, 00, 00),
                End = new DateTime(2023, 06, 03, 22, 00, 00),
                Rate = 300,
                Type = EnumRateType.Day
            }
        };

        var overtimePeriod = GivenOvertimePeriod(06, 03, 22,
                                                 06, 03, 23);

        var (insertOvertime, updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

        insertOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 22, 00, 00),
                End = new DateTime(2023, 06, 03, 23, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            }
        });

        updateOvertime.Should().BeEmpty();
    }

    /// <summary>
    /// holiday
    /// 
    /// history
    /// 22 - 23 (390)
    /// 
    /// overtime
    /// 20 - 22
    ///
    /// real overtime rate
    /// 20 - 22 (300)
    ///
    /// history
    /// 22 - 23 (390 -> 390 not change)
    /// </summary>
    [Fact]
    public void history_night_overtime_and_holiday_day_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 22, 00, 00),
                End = new DateTime(2023, 06, 03, 23, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            }
        };

        var overtimePeriod = GivenOvertimePeriod(06, 03, 20,
                                                 06, 03, 22);

        var (insertOvertime, updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

        insertOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 20, 00, 00),
                End = new DateTime(2023, 06, 03, 22, 00, 00),
                Rate = 300,
                Type = EnumRateType.Day
            },
        });

        updateOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 22, 00, 00),
                End = new DateTime(2023, 06, 03, 23, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// workday -> workday
    /// 
    /// history
    /// 22 - 00 (200), 00 - 01 (200)
    /// 
    /// overtime
    /// 20 - 22
    ///
    /// real overtime rate
    /// 20 - 22 (150)
    ///
    /// history
    /// 22 - 00 (200 -> 210), 00 - 01 (200 -> 210)
    /// </summary>
    /// 
    [Fact]
    public void history_night_cross_workday_overtime_and_workday_day_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
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
            },
        };

        var overtimePeriod = GivenOvertimePeriod(06, 01, 20,
                                                 06, 01, 22);

        var (insertOvertime, updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

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

        updateOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
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
            },
        });
    }

    /// <summary>
    /// workday -> holiday
    /// 
    /// history
    /// 22 - 00 (200), 00 - 01 (390)
    /// 
    /// overtime
    /// 20 - 22
    ///
    /// real overtime rate
    /// 20 - 22 (150)
    ///
    /// history
    /// 22 - 00 (200 -> 210), 00 - 01 (390 -> 390 not change)
    /// </summary>
    [Fact]
    public void history_night_cross_holiday_overtime_and_workday_day_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
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
            },
        };

        var overtimePeriod = GivenOvertimePeriod(06, 02, 20,
                                                 06, 02, 22);

        var (insertOvertime, updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

        insertOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 02, 20, 00, 00),
                End = new DateTime(2023, 06, 02, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            },
        });

        updateOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
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
            },
        });
    }

    /// <summary>
    /// holiday -> workday 
    /// 
    /// history
    /// workday 21 - 22 (150), 22 - 23 (210)
    /// 
    /// overtime
    /// holiday 23 - 02
    ///
    /// real overtime rate
    /// 23 - 00 (390), 00 - 02 (210, not 200)
    ///
    /// history
    /// workday 21 - 22 (150 not change), 22 - 23 (210 not change)
    /// </summary>
    [Fact]
    public void history_day_night_workday_overtime_and_holiday_night_cross_day_overlap()
    {
        var historyOvertimePeriod = new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 04, 21, 00, 00),
                End = new DateTime(2023, 06, 04, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = new DateTime(2023, 06, 04, 22, 00, 00),
                End = new DateTime(2023, 06, 04, 23, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            },
        };

        var overtimePeriod = GivenOvertimePeriod(06, 03, 23,
                                                 06, 04, 02);

        var (insertOvertime, updateOvertime) = _overtimeHandler.Handler(overtimePeriod, historyOvertimePeriod);

        insertOvertime.Should().BeEquivalentTo(new List<OvertimePeriod>
        {
            new()
            {
                Start = new DateTime(2023, 06, 03, 23, 00, 00),
                End = new DateTime(2023, 06, 04, 00, 00, 00),
                Rate = 390,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 04, 00, 00, 00),
                End = new DateTime(2023, 06, 04, 02, 00, 00),
                Rate = 210,
                Type = EnumRateType.Night
            },
        });

        updateOvertime.Should().BeEmpty();
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