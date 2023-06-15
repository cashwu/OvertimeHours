namespace OvertimeHour;

public class OvertimeRate
{
    public OvertimeRate(int day)
    {
        Day = day;
        Night = 0;
        NightWithDayOvertime = 0;
        Type = EnumOverTimeRateType.Day;
    }

    public OvertimeRate(int night, int nightWithDayOvertime)
    {
        Day = 0;
        Night = night;
        NightWithDayOvertime = nightWithDayOvertime;
        Type = EnumOverTimeRateType.Night;
    }

    public OvertimeRate(int day, int night, int nightWithDayOvertime, EnumOverTimeRateType type)
    {
        Day = day;
        Night = night;
        NightWithDayOvertime = nightWithDayOvertime;
        Type = type;
    }

    public EnumOverTimeRateType Type { get; set; }

    public int Day { get; private set; }

    public int Night { get; private set; }

    public int NightWithDayOvertime { get; private set; }
}