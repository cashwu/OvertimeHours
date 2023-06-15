using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimePeriodSettingsTests
{
    /// <summary>
    /// setting
    /// 01 - 02
    ///
    /// split setting
    /// 01 - 02
    /// </summary>
    [Fact]
    public void ctor_1_setting()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var overtimePeriodSetting = new OvertimePeriodSetting(new Period(baseDate, "01:00", "02:00"), new OvertimeRate(0, 0));
        var overtimePeriodSettings = new OvertimePeriodSettings(overtimePeriodSetting);

        overtimePeriodSettings.Count.Should().Be(1);
    }

    /// <summary>
    /// setting
    /// 23 - 02
    ///
    /// split setting
    /// 23 - 00, 00 - 02
    /// </summary>
    [Fact]
    public void ctor_1_setting_cross_day()
    {
        var baseDate = new DateTime(2023, 06, 01);

        var overtimePeriodSetting = new OvertimePeriodSetting(new Period(baseDate, "23:00", "02:00"), new OvertimeRate(0, 0));
        var overtimePeriodSettings = new OvertimePeriodSettings(overtimePeriodSetting);

        overtimePeriodSettings.Count.Should().Be(2);

        var overtimePeriodSettingFirst = overtimePeriodSettings[0];
        overtimePeriodSettingFirst.Period.Start.Should().Be(new DateTime(2023, 06, 01, 23, 00, 00));
        overtimePeriodSettingFirst.Period.End.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));

        var overtimePeriodSettingSecond = overtimePeriodSettings[1];
        overtimePeriodSettingSecond.Period.Start.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));
        overtimePeriodSettingSecond.Period.End.Should().Be(new DateTime(2023, 06, 02, 02, 00, 00));
    }

    /// <summary>
    /// setting
    /// 06 - 17, 17 - 06
    ///
    /// split setting
    /// 06 - 17, 17 - 00, 00 - 06
    /// </summary>
    [Fact]
    public void ctor_2_setting()
    {
        var baseDate = new DateTime(2023, 06, 01);

        var overtimePeriodSetting01 = new OvertimePeriodSetting(new Period(baseDate, "06:00", "17:00"), new OvertimeRate(0, 0));
        var overtimePeriodSetting02 = new OvertimePeriodSetting(new Period(baseDate, "17:00", "06:00"), new OvertimeRate(0, 0));

        var overtimePeriodSettings = new OvertimePeriodSettings(overtimePeriodSetting01, overtimePeriodSetting02);

        overtimePeriodSettings.Count.Should().Be(3);

        var overtimePeriodSettingFirst = overtimePeriodSettings[0];
        overtimePeriodSettingFirst.Period.Start.Should().Be(new DateTime(2023, 06, 01, 06, 00, 00));
        overtimePeriodSettingFirst.Period.End.Should().Be(new DateTime(2023, 06, 01, 17, 00, 00));

        var overtimePeriodSettingSecond = overtimePeriodSettings[1];
        overtimePeriodSettingSecond.Period.Start.Should().Be(new DateTime(2023, 06, 01, 17, 00, 00));
        overtimePeriodSettingSecond.Period.End.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));

        var overtimePeriodSettingThird = overtimePeriodSettings[2];
        overtimePeriodSettingThird.Period.Start.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));
        overtimePeriodSettingThird.Period.End.Should().Be(new DateTime(2023, 06, 02, 06, 00, 00));
    }

    /// <summary>
    /// setting
    /// 06 - 17, 17 - 06
    ///
    /// split setting
    /// 06 - 17, 17 - 00, 00 - 06
    ///
    /// overtime
    /// 18 - 20
    ///
    /// split overtime
    /// 18 - 20
    /// </summary>
    [Fact]
    public void split_period_have_1_overlap_not_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 18, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 01, 20, 00, 00);

        var overtimePeriodSetting01 = new OvertimePeriodSetting(new Period(overtimeStart, "06:00", "17:00"), new OvertimeRate(0, 0));
        var overtimePeriodSetting02 = new OvertimePeriodSetting(new Period(overtimeStart, "17:00", "06:00"), new OvertimeRate(0, 0));

        var overtimePeriodSettings = new OvertimePeriodSettings(overtimePeriodSetting01, overtimePeriodSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overTimePeriods = overtimePeriodSettings.SplitPeriod(overTimePeriod).ToList();

        overTimePeriods.Should().BeEquivalentTo(new List<Period>
        {
            new(overtimeStart, overtimeEnd)
        });
    }

    /// <summary>
    /// setting
    /// 06 - 17, 17 - 06
    ///
    /// split setting
    /// 06 - 17, 17 - 00, 00 - 06
    ///
    /// overtime
    /// 22 - 01
    ///
    /// split overtime
    /// 22 - 00, 00 - 01
    /// </summary>
    [Fact]
    public void split_period_have_1_overlap_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 22, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 02, 01, 00, 00);

        var overtimePeriodSetting01 = new OvertimePeriodSetting(new Period(overtimeStart, "06:00", "17:00"), new OvertimeRate(0, 0));
        var overtimePeriodSetting02 = new OvertimePeriodSetting(new Period(overtimeStart, "17:00", "06:00"), new OvertimeRate(0, 0));

        var overtimePeriodSettings = new OvertimePeriodSettings(overtimePeriodSetting01, overtimePeriodSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overTimePeriods = overtimePeriodSettings.SplitPeriod(overTimePeriod).ToList();

        var crossDay = new DateTime(2023, 06, 02, 00, 00, 00);

        overTimePeriods.Should().BeEquivalentTo(new List<Period>
        {
            new(overtimeStart, crossDay),
            new(crossDay, overtimeEnd)
        }, options => options.Excluding(a => a.BaseDate));
    }

    /// <summary>
    /// setting
    /// 06 - 17, 17 - 06
    ///
    /// split setting
    /// 06 - 17, 17 - 00, 00 - 06
    ///
    /// overtime
    /// 16 - 22
    ///
    /// split overtime
    /// 16 - 17, 17 - 22
    /// </summary>
    [Fact]
    public void split_period_have_2_overlap_not_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 16, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 01, 22, 00, 00);

        var overtimePeriodSetting01 = new OvertimePeriodSetting(new Period(overtimeStart, "06:00", "17:00"), new OvertimeRate(0, 0));
        var overtimePeriodSetting02 = new OvertimePeriodSetting(new Period(overtimeStart, "17:00", "06:00"), new OvertimeRate(0, 0));

        var overtimePeriodSettings = new OvertimePeriodSettings(overtimePeriodSetting01, overtimePeriodSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overTimePeriods = overtimePeriodSettings.SplitPeriod(overTimePeriod).ToList();
        var firstSettingEnd = new DateTime(2023, 06, 01, 17, 00, 00);

        overTimePeriods.Should().BeEquivalentTo(new List<Period>
        {
            new(overtimeStart, firstSettingEnd),
            new(firstSettingEnd, overtimeEnd),
        }, options => options.Excluding(a => a.BaseDate));
    }

    /// <summary>
    /// setting
    /// 06 - 17, 17 - 06
    ///
    /// split setting
    /// 06 - 17, 17 - 00, 00 - 06
    ///
    /// overtime
    /// 16 - 01
    ///
    /// split overtime
    /// 16 - 17, 17 - 00, 00 - 01
    /// </summary>
    [Fact]
    public void split_period_have_2_overlap_cross_day()
    {
        var overtimeStart = new DateTime(2023, 06, 01, 16, 00, 00);
        var overtimeEnd = new DateTime(2023, 06, 02, 01, 00, 00);

        var overtimePeriodSetting01 = new OvertimePeriodSetting(new Period(overtimeStart, "06:00", "17:00"), new OvertimeRate(0, 0));
        var overtimePeriodSetting02 = new OvertimePeriodSetting(new Period(overtimeStart, "17:00", "06:00"), new OvertimeRate(0, 0));

        var overtimePeriodSettings = new OvertimePeriodSettings(overtimePeriodSetting01, overtimePeriodSetting02);

        var overTimePeriod = new Period(overtimeStart, overtimeEnd);

        var overTimePeriods = overtimePeriodSettings.SplitPeriod(overTimePeriod).ToList();

        var crossDay = new DateTime(2023, 06, 02, 00, 00, 00);
        var firstSettingEnd = new DateTime(2023, 06, 01, 17, 00, 00);

        overTimePeriods.Should().BeEquivalentTo(new List<Period>
        {
            new(overtimeStart, firstSettingEnd),
            new(firstSettingEnd, crossDay),
            new(crossDay, overtimeEnd)
        }, options => options.Excluding(a => a.BaseDate));
    }
}