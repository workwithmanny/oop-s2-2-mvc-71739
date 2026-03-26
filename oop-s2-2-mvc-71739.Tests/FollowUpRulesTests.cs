using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Tests;

public class FollowUpRulesTests
{
    [Fact]
    public void FollowUpCannotBeClosedWithoutClosedDate()
    {
        var followUp = new FollowUp
        {
            InspectionId = 1,
            DueDate = new DateTime(2026, 3, 15),
            Status = FollowUpStatus.Closed,
            ClosedDate = null
        };

        var isValid = followUp.Status != FollowUpStatus.Closed || followUp.ClosedDate is not null;

        Assert.False(isValid);
    }
}
