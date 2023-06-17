using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PeriodTests
{
    [Fact]
    public void ctor()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "01:00", "02:00");

        period.Start.Should().Be(new DateTime(2023, 06, 01, 01, 00, 00));
        period.End.Should().Be(new DateTime(2023, 06, 01, 02, 00, 00));

        period.OriginStart.Should().Be("01:00");
        period.OriginEnd.Should().Be("02:00");
        period.BaseDate.Should().Be(baseDate);
    }

    [Fact]
    public void end_date_is_0000()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "23:00", "00:00");

        period.Start.Should().Be(new DateTime(2023, 06, 01, 23, 00, 00));
        period.End.Should().Be(new DateTime(2023, 06, 02, 00, 00, 00));

        period.OriginStart.Should().Be("23:00");
        period.OriginEnd.Should().Be("00:00");
        period.BaseDate.Should().Be(baseDate);
    }

    [Fact]
    public void start_date_is_0000()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "00:00", "01:00");

        period.Start.Should().Be(new DateTime(2023, 06, 01, 00, 00, 00));
        period.End.Should().Be(new DateTime(2023, 06, 01, 01, 00, 00));

        period.OriginStart.Should().Be("00:00");
        period.OriginEnd.Should().Be("01:00");
        period.BaseDate.Should().Be(baseDate);
    }

    [Fact]
    public void overlap_period_start()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "05:00", "07:00");

        var overlapPeriod = period.OverlapPeriod(new Period(baseDate, "04:00", "06:00"));

        overlapPeriod.Start.Should().Be(new DateTime(2023, 06, 01, 05, 00, 00));
        overlapPeriod.End.Should().Be(new DateTime(2023, 06, 01, 06, 00, 00));
    }

    [Fact]
    public void overlap_period_end()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "05:00", "07:00");

        var overlapPeriod = period.OverlapPeriod(new Period(baseDate, "06:00", "08:00"));

        overlapPeriod.Start.Should().Be(new DateTime(2023, 06, 01, 06, 00, 00));
        overlapPeriod.End.Should().Be(new DateTime(2023, 06, 01, 07, 00, 00));
    }

    [Fact]
    public void overlap_period_cross()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "05:00", "07:00");

        var overlapPeriod = period.OverlapPeriod(new Period(baseDate, "04:00", "08:00"));

        overlapPeriod.Start.Should().Be(new DateTime(2023, 06, 01, 05, 00, 00));
        overlapPeriod.End.Should().Be(new DateTime(2023, 06, 01, 07, 00, 00));
    }

    [Fact]
    public void overlap_period_before_no_cross()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "05:00", "07:00");

        var overlapPeriod = period.OverlapPeriod(new Period(baseDate, "04:00", "05:00"));

        overlapPeriod.Should().BeNull();
    }

    [Fact]
    public void overlap_period_after_no_cross()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "05:00", "07:00");

        var overlapPeriod = period.OverlapPeriod(new Period(baseDate, "07:00", "08:00"));

        overlapPeriod.Should().BeNull();
    }

    [Fact]
    public void isCross_day_end_is_0001()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "23:00", "00:01");

        period.IsSettingCrossDay.Should().BeTrue();
    }

    [Fact]
    public void isCross_day_end_is_0000()
    {
        var baseDate = new DateTime(2023, 06, 01);
        var period = new Period(baseDate, "23:00", "00:00");

        period.IsSettingCrossDay.Should().BeFalse();
    }

    [Fact]
    public void ctor_2_datetime()
    {
        var start = new DateTime(2023, 06, 01, 22, 00, 00);
        var end = new DateTime(2023, 06, 02, 02, 00, 00);
        var period = new Period(start, end);

        period.Start.Should().Be(start);
        period.End.Should().Be(end);

        period.OriginStart.Should().Be("22:00");
        period.OriginEnd.Should().Be("02:00");
        period.BaseDate.Should().Be(start);
    }

    [Fact]
    public void ctor_2_date_string()
    {
        var period = new Period("06:00", "22:00");

        period.OriginStart.Should().Be("06:00");
        period.OriginEnd.Should().Be("22:00");
        period.Start.Should().Be(default);
        period.End.Should().Be(default);
        period.BaseDate.Should().Be(default);
    }
}