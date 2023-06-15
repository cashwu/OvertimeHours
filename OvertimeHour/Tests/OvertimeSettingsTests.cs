using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeSettingsTests
{
    /// <summary>
    /// setting
    /// 06 - 22
    ///
    /// split setting
    /// 06 - 22
    /// </summary>
    [Fact]
    public void ctor_1_setting()
    {
        var baseDate = new DateTime(2023, 06, 01);

        var overtimeSetting = new OvertimeSetting(new Period(baseDate, "06:00", "22:00"),
                                                  new Rate(150));

        var overtimeSettings = new OvertimeSettings(overtimeSetting);

        overtimeSettings.Count.Should().Be(1);
    }

    /// <summary>
    /// setting
    /// 22 - 06
    ///
    /// split setting
    /// 22 - 00, 22 - 06
    /// </summary>
    [Fact]
    public void ctor_1_setting_cross_day()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var rate = new Rate(200, 210);

        var overtimeSetting = new OvertimeSetting(new Period(baseDate, "22:00", "06:00"),
                                                  rate);

        var overtimeSettings = new OvertimeSettings(overtimeSetting);

        overtimeSettings.Count.Should().Be(2);

        var overtimeSettingFirst = overtimeSettings[0];
        overtimeSettingFirst.Period.Start.Should().Be(new DateTime(2023, 06, 01, 22, 00, 00));
        overtimeSettingFirst.Period.End.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));
        overtimeSettingFirst.Rate.Should().Be(rate);

        var overtimeSettingSecond = overtimeSettings[1];
        overtimeSettingSecond.Period.Start.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));
        overtimeSettingSecond.Period.End.Should().Be(new DateTime(2023, 06, 02, 06, 00, 00));
        overtimeSettingSecond.Rate.Should().Be(rate);
    }

    /// <summary>
    /// setting
    /// 06 - 22, 22 - 06
    ///
    /// split setting
    /// 06 - 22, 22 - 00, 00 - 06
    /// </summary>
    [Fact]
    public void ctor_2_setting()
    {
        var baseDate = new DateTime(2023, 06, 01);

        var dayRate = new Rate(150);
        var overtimePeriodSetting01 = new OvertimeSetting(new Period(baseDate, "06:00", "22:00"), dayRate);

        var nightRate = new Rate(200, 210);
        var overtimePeriodSetting02 = new OvertimeSetting(new Period(baseDate, "22:00", "06:00"), nightRate);

        var overtimeSettings = new OvertimeSettings(overtimePeriodSetting01, overtimePeriodSetting02);

        overtimeSettings.Count.Should().Be(3);

        var overtimeSettingFirst = overtimeSettings[0];
        overtimeSettingFirst.Period.Start.Should().Be(new DateTime(2023, 06, 01, 06, 00, 00));
        overtimeSettingFirst.Period.End.Should().Be(new DateTime(2023, 06, 01, 22, 00, 00));
        overtimeSettingFirst.Rate.Should().Be(dayRate);

        var overtimeSettingSecond = overtimeSettings[1];
        overtimeSettingSecond.Period.Start.Should().Be(new DateTime(2023, 06, 01, 22, 00, 00));
        overtimeSettingSecond.Period.End.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));

        var overtimeSettingThird = overtimeSettings[2];
        overtimeSettingThird.Period.Start.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));
        overtimeSettingThird.Period.End.Should().Be(new DateTime(2023, 06, 02, 06, 00, 00));
    }

    /// <summary>
    /// setting
    /// 06 - 22 (150), 22 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 22, 22 - 00, 00 - 06
    ///
    /// overtime
    /// 18 - 20
    ///
    /// split overtime
    /// 18 - 20 (150)
    /// </summary>
    [Fact]
    public void split_period_have_1_overlap_not_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 18, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 01, 20, 00, 00);

        var dayRate = new Rate(150);
        var overtimeSetting01 = new OvertimeSetting(new Period(overtimeStart, "06:00", "22:00"), dayRate);
        var nightRate = new Rate(200, 210);
        var overtimeSetting02 = new OvertimeSetting(new Period(overtimeStart, "22:00", "06:00"), nightRate);
        var overtimeSettings = new OvertimeSettings(overtimeSetting01, overtimeSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overtimes = overtimeSettings.SplitPeriod(overTimePeriod);

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
    /// overtime
    /// 22 - 01
    ///
    /// split overtime
    /// 22 - 00 (200), 00 - 01 (200)
    /// </summary>
    [Fact]
    public void split_period_have_1_overlap_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 22, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 02, 01, 00, 00);

        var dayRate = new Rate(150);
        var overtimeSetting01 = new OvertimeSetting(new Period(overtimeStart, "06:00", "22:00"), dayRate);
        var nightRate = new Rate(200, 210);
        var overtimeSetting02 = new OvertimeSetting(new Period(overtimeStart, "22:00", "06:00"), nightRate);

        var overtimeSettings = new OvertimeSettings(overtimeSetting01, overtimeSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overTimePeriods = overtimeSettings.SplitPeriod(overTimePeriod).ToList();

        var crossDay = new DateTime(2023, 06, 02, 00, 00, 00);

        overTimePeriods.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
                End = crossDay,
                Rate = 200,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = crossDay,
                End = overtimeEnd,
                Rate = 200,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// setting
    /// 06 - 17 (150), 17 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 17, 17 - 00, 00 - 06
    ///
    /// overtime
    /// 16 - 22
    ///
    /// split overtime
    /// 16 - 17 (150), 17 - 22 (210)
    /// </summary>
    [Fact]
    public void split_period_have_2_overlap_not_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 16, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 01, 22, 00, 00);

        var dayRate = new Rate(150);
        var overtimeSetting01 = new OvertimeSetting(new Period(overtimeStart, "06:00", "17:00"), dayRate);
        var nightRate = new Rate(200, 210);
        var overtimeSetting02 = new OvertimeSetting(new Period(overtimeStart, "17:00", "06:00"), nightRate);

        var overtimeSettings = new OvertimeSettings(overtimeSetting01, overtimeSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);
        var firstSettingEnd = new DateTime(2023, 06, 01, 17, 00, 00);

        var overTimePeriods = overtimeSettings.SplitPeriod(overTimePeriod).ToList();

        overTimePeriods.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
                End = firstSettingEnd,
                Rate = 150,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = firstSettingEnd,
                End = overtimeEnd,
                Rate = 210,
                Type = EnumRateType.Night
            }
        });
    }

    /// <summary>
    /// setting
    /// 06 - 17 (150), 17 - 06 (200, 210)
    ///
    /// split setting
    /// 06 - 17, 17 - 00, 00 - 06
    ///
    /// overtime
    /// 16 - 01
    ///
    /// split overtime
    /// 16 - 17 (150), 17 - 00 (210), 00 - 01 (210)
    /// </summary>
    [Fact]
    public void split_period_have_2_overlap_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 16, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 02, 01, 00, 00);

        var dayRate = new Rate(150);
        var overtimeSetting01 = new OvertimeSetting(new Period(overtimeStart, "06:00", "17:00"), dayRate);
        var nightRate = new Rate(200, 210);
        var overtimeSetting02 = new OvertimeSetting(new Period(overtimeStart, "17:00", "06:00"), nightRate);

        var overtimeSettings = new OvertimeSettings(overtimeSetting01, overtimeSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overTimePeriods = overtimeSettings.SplitPeriod(overTimePeriod).ToList();

        var crossDay = new DateTime(2023, 06, 02, 00, 00, 00);
        var firstSettingEnd = new DateTime(2023, 06, 01, 17, 00, 00);

        overTimePeriods.Should().BeEquivalentTo(new List<Overtime>
        {
            new()
            {
                Start = overtimeStart,
                End = firstSettingEnd,
                Rate = 150,
                Type = EnumRateType.Day
            },
            new()
            {
                Start = firstSettingEnd,
                End = crossDay,
                Rate = 210,
                Type = EnumRateType.Night
            },
            new()
            {
                Start = crossDay,
                End = overtimeEnd,
                Rate = 210,
                Type = EnumRateType.Night
            }
        });
    }
}