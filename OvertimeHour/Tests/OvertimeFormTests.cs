using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace OvertimeHour.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class OvertimeFormTests
{
    [Fact]
    public void ctor()
    {
        var overtimeForm = new OvertimeForm(new DateTime(2023, 06, 01, 18, 00, 00),
                                            new DateTime(2023, 06, 01, 20, 00, 00));

        overtimeForm.Start.Should().Be(new DateTime(2023, 06, 01, 18, 00, 00));
        overtimeForm.End.Should().Be(new DateTime(2023, 06, 01, 20, 00, 00));
    }
}