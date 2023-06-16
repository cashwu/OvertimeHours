using OvertimeHour.Enums;

namespace OvertimeHour;

public class Overtime
{
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public int Rate { get; set; }

    public EnumRateType Type { get; set; }
}