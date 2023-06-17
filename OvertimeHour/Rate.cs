using OvertimeHour.Enums;

namespace OvertimeHour;

public class Rate
{
    /// <summary>
    /// for day
    /// </summary>
    /// <param name="day"></param>
    public Rate(int day)
    {
        Day = day;
        Night = 0;
        NightWithDayOvertime = 0;
        Type = EnumRateType.Day;
    }

    /// <summary>
    /// for night
    /// </summary>
    /// <param name="night"></param>
    /// <param name="nightWithDayOvertime"></param>
    public Rate(int night, int nightWithDayOvertime)
    {
        Day = 0;
        Night = night;
        NightWithDayOvertime = nightWithDayOvertime;
        Type = EnumRateType.Night;
    }

    public EnumRateType Type { get; set; }

    public int Day { get; private set; }

    public int Night { get; private set; }

    public int NightWithDayOvertime { get; private set; }
}