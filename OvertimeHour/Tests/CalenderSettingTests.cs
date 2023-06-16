using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class CalenderSettingTests
{
    [Fact]
    public void ctor()
    {
        var setting0602 = new CalenderSetting(new DateTime(2023, 06, 01), EnumCalenderType.Workday);
        var setting0601 = new CalenderSetting(new DateTime(2023, 06, 02), EnumCalenderType.Workday);

        var calenderSettings = new CalenderSettings(setting0601, setting0602);

        calenderSettings.Count.Should().Be(2);
    }
}