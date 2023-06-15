using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeRateTests
{
    [Fact]
    public void day()
    {
        var overtimeRate = new OvertimeRate(150);

        overtimeRate.Type.Should().Be(EnumOverTimeRateType.Day);
        overtimeRate.Day.Should().Be(150);
        overtimeRate.Night.Should().Be(0);
        overtimeRate.NightWithDayOvertime.Should().Be(0);
    }

    [Fact]
    public void night()
    {
        var overtimeRate = new OvertimeRate(200, 210);

        overtimeRate.Type.Should().Be(EnumOverTimeRateType.Night);
        overtimeRate.Day.Should().Be(0);
        overtimeRate.Night.Should().Be(200);
        overtimeRate.NightWithDayOvertime.Should().Be(210);
    }
}