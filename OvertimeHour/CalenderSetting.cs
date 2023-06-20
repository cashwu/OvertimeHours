using OvertimeHour.Enums;

#pragma warning disable CS8524

namespace OvertimeHour;

public class CalenderSetting
{
    private readonly EnumCalenderType _type;

    public CalenderSetting(DateTime date, EnumCalenderType type)
    {
        Date = date;
        _type = type;
    }

    public DateTime Date { get; }

    public EnumOvertimeSettingType OvertimeSettingType
    {
        get
        {
            var overtimeSettingType = _type switch
            {
                EnumCalenderType.Workday => EnumOvertimeSettingType.Workday,
                EnumCalenderType.Holiday => EnumOvertimeSettingType.Holiday,
            };

            return overtimeSettingType;
        }
    }
}