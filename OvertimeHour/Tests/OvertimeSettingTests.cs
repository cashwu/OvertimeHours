using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeSettingTests
{
    [Fact]
    public void ctor()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "06:00", "20:00");
        var overtimeRate = new Rate(150);

        var overtimeSetting = new OvertimeSetting(period, overtimeRate);

        overtimeSetting.Period.Should().BeEquivalentTo(period);
        overtimeSetting.Rate.Should().BeEquivalentTo(overtimeRate);
    }
}