using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimePeriodSettingTests
{
    /// <summary>
    /// workday
    /// </summary>
    [Fact]
    public void workday()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "06:00", "20:00");

        var overtimePeriodSetting = new OvertimePeriodSetting(period, new OvertimeRate(150, 200, 210));

        overtimePeriodSetting.DayRate.Should().Be(150);
        overtimePeriodSetting.NightRate.Should().Be(200);
        overtimePeriodSetting.NightRateWithDayOvertime.Should().Be(210);
    }

    /// <summary>
    /// holiday
    /// </summary>
    [Fact]
    public void holiday()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "06:00", "20:00");

        var overtimePeriodSetting = new OvertimePeriodSetting(period, new OvertimeRate(300, 390));

        overtimePeriodSetting.DayRate.Should().Be(300);
        overtimePeriodSetting.NightRate.Should().Be(390);
        overtimePeriodSetting.NightRateWithDayOvertime.Should().Be(0);
    }
}