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
        EnumOvertimeSettingType overtimeSettingType = 0;

        if (Type == EnumCalenderType.Workday)
        {
            overtimeSettingType = EnumOvertimeSettingType.Workday;
        }

        return overtimeSettingType;
    }
}