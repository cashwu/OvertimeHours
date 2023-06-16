using OvertimeHour.Enums;

namespace OvertimeHour;

public class Rate
{
    public Rate(int day)
    {
        Day = day;
        Night = 0;
        NightWithDayOvertime = 0;
        Type = EnumRateType.Day;
    }

    public Rate(int night, int nightWithDayOvertime)
    {
        Day = 0;
        Night = night;
        NightWithDayOvertime = nightWithDayOvertime;
        Type = EnumRateType.Night;
    }

    public Rate(int day, int night, int nightWithDayOvertime, EnumRateType type)
    {
        Day = day;
        Night = night;
        NightWithDayOvertime = nightWithDayOvertime;
        Type = type;
    }

    public EnumRateType Type { get; set; }

    public int Day { get; private set; }

    public int Night { get; private set; }

    public int NightWithDayOvertime { get; private set; }
}