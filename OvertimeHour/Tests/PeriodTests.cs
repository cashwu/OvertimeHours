using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PeriodTests
{
    [Fact]
    public void overlap_period_start()
    {
        var period = GivenPeriod(05, 07);
        var another = GivenPeriod(05, 06);

        var overlapPeriod = period.OverlapPeriod(another);

        overlapPeriod.Start.Should().Be(new DateTime(2023, 06, 01, 05, 00, 00));
        overlapPeriod.End.Should().Be(new DateTime(2023, 06, 01, 06, 00, 00));
    }

    [Fact]
    public void overlap_period_end()
    {
        var period = GivenPeriod(05, 07);
        var another = GivenPeriod(06, 08);

        var overlapPeriod = period.OverlapPeriod(another);

        overlapPeriod.Start.Should().Be(new DateTime(2023, 06, 01, 06, 00, 00));
        overlapPeriod.End.Should().Be(new DateTime(2023, 06, 01, 07, 00, 00));
    }

    [Fact]
    public void overlap_period_cross()
    {
        var period = GivenPeriod(05, 07);
        var another = GivenPeriod(04, 08);

        var overlapPeriod = period.OverlapPeriod(another);

        overlapPeriod.Start.Should().Be(new DateTime(2023, 06, 01, 05, 00, 00));
        overlapPeriod.End.Should().Be(new DateTime(2023, 06, 01, 07, 00, 00));
    }

    [Fact]
    public void overlap_period_before_no_cross()
    {
        var period = GivenPeriod(05, 07);
        var another = GivenPeriod(04, 04);

        var overlapPeriod = period.OverlapPeriod(another);

        overlapPeriod.Should().BeNull();
    }

    [Fact]
    public void overlap_period_after_no_cross()
    {
        var period = GivenPeriod(05, 07);
        var another = GivenPeriod(07, 08);

        var overlapPeriod = period.OverlapPeriod(another);

        overlapPeriod.Should().BeNull();
    }

    private static Period GivenPeriod(int startHour, int endHour)
    {
        return new Period(new DateTime(2023, 06, 01, startHour, 00, 00),
                          new DateTime(2023, 06, 01, endHour, 00, 00));
    }
}