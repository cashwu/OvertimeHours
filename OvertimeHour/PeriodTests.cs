using FluentAssertions;

namespace OvertimeHour;

public class PeriodTests
{
   [Fact]
   public void ctor_2_string_and_overtime_startDateTime()
   {
        var startDateTime = new DateTime(2023, 06, 01);
        
        var period = new Period(startDateTime, "01:00", "02:00");

        period.StartTimeSpan.Should().Be(TimeSpan.Parse("01:00"));
        period.EndTimeSpan.Should().Be(TimeSpan.Parse("02:00"));
        period.StartDateTime.Should().Be(new DateTime(2023, 06, 01, 01, 00, 00));
        period.EndDateTime.Should().Be(new DateTime(2023, 06, 01, 02, 00, 00));
   }
}