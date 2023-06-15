using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeSettingsTests
{
    /// <summary>
    /// setting
    /// 06 - 22 (150), 22 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 22, 22 - 00, 00 - 06
    ///
    /// work
    /// 08 - 18
    ///
    /// overtime
    /// 18 - 20
    ///
    /// split overtime
    /// 18 - 20 (150)
    /// </summary>
    [Fact]
    public void day_overlap_not_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 18, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 01, 20, 00, 00);
        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overtimeSettings = GivenOvertimeSettings(overtimeStart.Date);
        var overtimes = overtimeSettings.CreateOvertime(overTimePeriod);

        overtimes.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
                End = overtimeEnd,
                Rate = 150,
                Type = EnumRateType.Day
            }
        });
    }

    /// <summary>
    /// setting
    /// 06 - 22 (150), 22 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 22, 22 - 00, 00 - 06
    ///
    /// work
    /// 08 - 18
    ///
    /// overtime
    /// 22 - 23
    ///
    /// split overtime
    /// 22 - 23 (200)
    /// </summary>
    [Fact]
    public void night_overlap_not_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 22, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 01, 23, 00, 00);
        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overtimeSettings = GivenOvertimeSettings(overtimeStart.Date);
        var overtimes = overtimeSettings.CreateOvertime(overTimePeriod);

        overtimes.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
                End = overtimeEnd,
                Rate = 200,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// setting
    /// 06 - 22 (150), 22 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 22, 22 - 00, 00 - 06
    /// 
    /// work
    /// 08 - 18
    ///
    /// overtime
    /// 22 - 01
    ///
    /// split overtime
    /// 22 - 00 (200), 00 - 01 (200)
    /// </summary>
    [Fact]
    public void night_overlap_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 22, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 02, 01, 00, 00);
        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overtimeSettings = GivenOvertimeSettings(overtimeStart.Date);

        var overTimePeriods = overtimeSettings.CreateOvertime(overTimePeriod).ToList();

        overTimePeriods.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
                End = new DateTime(2023, 06, 02, 00, 00, 00),
                Rate = 200,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = new DateTime(2023, 06, 02, 00, 00, 00),
                End = overtimeEnd,
                Rate = 200,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// setting
    /// 06 - 22 (150), 22 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 22, 22 - 00, 00 - 06
    /// 
    /// work
    /// 08 - 18
    ///
    /// overtime
    /// 20 - 23
    ///
    /// split overtime
    /// 20 - 22 (150), 22 - 23 (210)
    /// </summary>
    [Fact]
    public void day_and_night_overlap_not_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 20, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 01, 23, 00, 00);
        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overtimeSettings = GivenOvertimeSettings(overtimeStart.Date);
        var overTimePeriods = overtimeSettings.CreateOvertime(overTimePeriod);

        overTimePeriods.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
                End = new DateTime(2023, 06, 01, 22, 00, 00),
                Rate = 150,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = new DateTime(2023, 06, 01, 22, 00, 00),
                End = overtimeEnd,
                Rate = 210,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// setting
    /// 06 - 22 (150), 22 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 22, 22 - 00, 00 - 06
    ///
    /// overtime
    /// 20 - 01
    ///
    /// split overtime
    /// 20 - 22 (150), 22 - 00 (210), 00 - 01 (210)
    /// </summary>
    [Fact]
    public void day_and_night_overlap_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 20, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 02, 01, 00, 00);
        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overtimeSettings = GivenOvertimeSettings(overtimeStart.Date);
        var overTimePeriods = overtimeSettings.CreateOvertime(overTimePeriod);

        overTimePeriods.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
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
                End = overtimeEnd,
                Rate = 210,
                Type = EnumRateType.Night
            }
        });
    }

    private static OvertimeSettings GivenOvertimeSettings(DateTime baseDate)
    {
        var daySetting = new OvertimeSetting(new Period(baseDate, "06:00", "22:00"),
                                             new Rate(150));

        var nightSetting = new OvertimeSetting(new Period(baseDate, "22:00", "06:00"),
                                               new Rate(200, 210));

        return new OvertimeSettings(daySetting, nightSetting);
    }
}