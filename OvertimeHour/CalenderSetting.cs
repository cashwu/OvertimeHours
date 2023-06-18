using OvertimeHour.Enums;

#pragma warning disable CS8524

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

    public EnumOvertimeSettingType OvertimeSettingType
    {
        get
        {
            var overtimeSettingType = Type switch
            {
                EnumCalenderType.Workday => EnumOvertimeSettingType.Workday,
                EnumCalenderType.Holiday => EnumOvertimeSettingType.Holiday,
            };

            return overtimeSettingType;
        }
    }
}