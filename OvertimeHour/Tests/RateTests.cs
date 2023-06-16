using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using OvertimeHour.Enums;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class RateTests
{
    [Fact]
    public void day()
    {
        var rate = new Rate(150);

        rate.Type.Should().Be(EnumRateType.Day);
        rate.Day.Should().Be(150);
        rate.Night.Should().Be(0);
        rate.NightWithDayOvertime.Should().Be(0);
    }

    [Fact]
    public void night()
    {
        var rate = new Rate(200, 210);

        rate.Type.Should().Be(EnumRateType.Night);
        rate.Day.Should().Be(0);
        rate.Night.Should().Be(200);
        rate.NightWithDayOvertime.Should().Be(210);
    }
}