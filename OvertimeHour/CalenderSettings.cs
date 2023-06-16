namespace OvertimeHour;

public class CalenderSettings : List<CalenderSetting>
{
    public CalenderSettings(params CalenderSetting[] calenderSettings)
    {
        AddRange(calenderSettings);
    }
}