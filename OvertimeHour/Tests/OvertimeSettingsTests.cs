using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using OvertimeHour.Enums;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeSettingsTests
{
    [Fact]
    public void ctor()
    {
        var workDaySetting = new OvertimeSetting((new Period("06:00", "22:00"), new Rate(150)),
                                                 (new Period("22:00", "06:00"), new Rate(200, 210)),
                                                 EnumOvertimeSettingType.Workday);

        var holidaySetting = new OvertimeSetting((new Period("06:00", "22:00"), new Rate(300)),
                                                 (new Period("22:00", "06:00"), new Rate(350, 0)),
                                                 EnumOvertimeSettingType.Holiday);

        var overtimeSettings = new OvertimeSettings(workDaySetting, holidaySetting);

        overtimeSettings.Count.Should().Be(2);
    }
}