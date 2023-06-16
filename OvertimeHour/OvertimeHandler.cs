using OvertimeHour.Enums;

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

    public List<Overtime> Handler(OvertimeForm overtimeForm)
    {
        var result = new List<Overtime>();

        var overtime = new Overtime
        {
            Start = new DateTime(2023, 06, 01, 18, 00, 00),
            End = new DateTime(2023, 06, 01, 20, 00, 00),
            Rate = 150,
            Type = EnumRateType.Day
        };

        result.Add(overtime);

        return result;
    }
}