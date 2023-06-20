using OvertimeHour.Enums;

namespace OvertimeHour;

public class OvertimeSettingFromDb
{
    public OvertimeSettingFromDb(string start, string end, Rate rate, EnumOvertimeSettingType type)
    {
        Start = start;
        End = end;
        Rate = rate;
        Type = type;
    }

    public EnumOvertimeSettingType Type { get; }

    private string End { get; }

    private string Start { get; }

    private Rate Rate { get; }

    public IEnumerable<OvertimeSetting> ToOvertimeSetting(DateTime baseDate)
    {
        var start = DateTime.Parse($"{baseDate.Date:yyyy/MM/dd} {Start}");
        var end = DateTime.Parse($"{baseDate.Date:yyyy/MM/dd} {End}");

        // setting 22:00 - 06:00
        // 0601 06:00 <= 0601 22:00
        if (end <= start)
        {
            // 0601 22:00 ~ 0602 00:00
            yield return new OvertimeSetting(new Period(start, start.AddDays(1).Date), Rate);

            // 0601 00:00 ~ 0601 06:00
            yield return new OvertimeSetting(new Period(end.Date, end), Rate);
        }
        else
        {
            yield return new OvertimeSetting(new Period(start, end), Rate);
        }
    }
}