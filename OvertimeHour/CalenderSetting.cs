using OvertimeHour.Enums;

namespace OvertimeHour;

public class CalenderSetting
{
    public CalenderSetting(DateTime date, EnumCalenderType type)
    {
        Date = date;
        Type = type;
    }

    public DateTime Date { get; }

    public EnumCalenderType Type { get; }

    public EnumOvertimeSettingType ToOvertimeSettingType()
    {
        EnumOvertimeSettingType overtimeSettingType = Type switch
        {
            EnumCalenderType.Workday => EnumOvertimeSettingType.Workday,
            EnumCalenderType.Holiday => EnumOvertimeSettingType.Holiday,
            _ => 0
        };

        return overtimeSettingType;
    }
}