using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using OvertimeHour.Enums;

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

        var overtimeSetting = new OvertimeSetting(period, overtimeRate, EnumOvertimeSettingType.Workday);

        overtimeSetting.Period.Should().BeEquivalentTo(period);
        overtimeSetting.Rate.Should().BeEquivalentTo(overtimeRate);
        overtimeSetting.Type.Should().Be(EnumOvertimeSettingType.Workday);
    }

    [Fact]
    public void ctor_day_night_type()
    {
        var dayPeriod = new Period("06:00", "22:00");
        var dayOvertimeRate = new Rate(150);
        var nightPeriod = new Period("22:00", "06:00");
        var nightOvertimeRate = new Rate(200, 210);

        var overtimeSetting = new OvertimeSetting((dayPeriod, dayOvertimeRate),
                                                  (nightPeriod, nightOvertimeRate),
                                                  EnumOvertimeSettingType.Workday);

        overtimeSetting.DaySetting.Should().BeEquivalentTo((dayPeriod, dayOvertimeRate));
        overtimeSetting.NightSetting.Should().BeEquivalentTo((nightPeriod, nightOvertimeRate));
        overtimeSetting.Type.Should().Be(EnumOvertimeSettingType.Workday);
    }
}