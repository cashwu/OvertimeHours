using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using OvertimeHour.Enums;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class CalenderSettingsTests
{
    [Fact]
    public void ctor()
    {
        var calenderSetting = new CalenderSetting(new DateTime(2023, 06, 01), EnumCalenderType.Workday);

        calenderSetting.Date.Should().Be(new DateTime(2023, 06, 01));
        calenderSetting.Type.Should().Be(EnumCalenderType.Workday);
    }
}