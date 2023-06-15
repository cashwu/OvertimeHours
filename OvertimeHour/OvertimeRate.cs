namespace OvertimeHour;

public class OvertimeRate
{
    public OvertimeRate(int dayRate, int nightRate, int nightRateWithDayOvertime = 0)
    {
        DayRate = dayRate;
        NightRate = nightRate;
        NightRateWithDayOvertime = nightRateWithDayOvertime;
    }

    public int DayRate { get; private set; }

    public int NightRate { get; private set; }

    public int NightRateWithDayOvertime { get; private set; }
}