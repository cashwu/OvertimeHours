namespace OvertimeHour;

public class OvertimeHandler
{
    private readonly OvertimeSettings _overtimeSettings;
    private readonly CalenderSettings _calenderSettings;

    public OvertimeHandler(OvertimeSettings overtimeSettings, CalenderSettings calenderSettings)
    {
        _overtimeSettings = overtimeSettings;
        _calenderSettings = calenderSettings;
    }
    
    public void Handler(OvertimeForm overtimeForm)
    {
        
    }
}