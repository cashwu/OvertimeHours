using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimePeriodSettingTests
{
    [Fact]
    public void ctor()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "06:00", "20:00");
        var overtimeRate = new OvertimeRate(150);

        var overtimePeriodSetting = new OvertimePeriodSetting(period, overtimeRate);

        overtimePeriodSetting.Period.Should().BeEquivalentTo(period);
        overtimePeriodSetting.OvertimeRate.Should().BeEquivalentTo(overtimeRate);
    }
}