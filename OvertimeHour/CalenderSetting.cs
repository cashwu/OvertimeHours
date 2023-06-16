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
}