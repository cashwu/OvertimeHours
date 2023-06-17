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
        var overtimeForm = new OvertimeForm(new DateTime(2023, 06, 01, 18, 00, 00),
                                            new DateTime(2023, 06, 01, 20, 00, 00));

        var overtimes = _overtimeHandler.Handler(overtimeForm);

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
        var overtimeForm = new OvertimeForm(new DateTime(2023, 06, 01, 22, 00, 00),
                                            new DateTime(2023, 06, 01, 23, 00, 00));

        var overtimes = _overtimeHandler.Handler(overtimeForm);

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
        var overtimeForm = new OvertimeForm(new DateTime(2023, 06, 01, 22, 00, 00),
                                            new DateTime(2023, 06, 02, 01, 00, 00));

        var overtimes = _overtimeHandler.Handler(overtimeForm);

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
        var overtimeForm = new OvertimeForm(new DateTime(2023, 06, 01, 20, 00, 00),
                                            new DateTime(2023, 06, 01, 23, 00, 00));

        var overtimes = _overtimeHandler.Handler(overtimeForm);

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
        var overtimeForm = new OvertimeForm(new DateTime(2023, 06, 01, 20, 00, 00),
                                            new DateTime(2023, 06, 02, 01, 00, 00));

        var overtimes = _overtimeHandler.Handler(overtimeForm);

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
    /// 22 - 00 (200), 00 - 01 (200)
    /// </summary>
    [Fact(Skip = "skip")]
    public void workday_night_overlap_cross_holiday()
    {
        var overtimeForm = new OvertimeForm(new DateTime(2023, 06, 02, 22, 00, 00),
                                            new DateTime(2023, 06, 03, 01, 00, 00));

        var overtimes = _overtimeHandler.Handler(overtimeForm);

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
                Rate = 350,
                Type = EnumRateType.Night
            }
        });
    }

    private static CalenderSettings GivenCalenderSettings()
    {
        return new CalenderSettings(new CalenderSetting(new DateTime(2023, 06, 01), EnumCalenderType.Workday),
                                    new CalenderSetting(new DateTime(2023, 06, 02), EnumCalenderType.Workday),
                                    new CalenderSetting(new DateTime(2023, 06, 03), EnumCalenderType.Holiday));
    }

    private static OvertimeSettings GivenOvertimeSettings()
    {
        var workDaySetting = new OvertimeSetting((new Period("06:00", "22:00"), new Rate(150)),
                                                 (new Period("22:00", "06:00"), new Rate(200, 210)),
                                                 EnumOvertimeSettingType.Workday);

        var holidaySetting = new OvertimeSetting((new Period("06:00", "22:00"), new Rate(300)),
                                                 (new Period("22:00", "06:00"), new Rate(350, 0)),
                                                 EnumOvertimeSettingType.Holiday);

        return new OvertimeSettings(workDaySetting, holidaySetting);
    }
}