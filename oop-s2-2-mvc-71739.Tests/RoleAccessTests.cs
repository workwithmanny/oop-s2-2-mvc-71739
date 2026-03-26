using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Tests;

public class RoleAccessTests
{
    [Fact]
    public void ViewerRole_IsDistinctFromInspector()
    {
        Assert.NotEqual(RoleNames.Viewer, RoleNames.Inspector);
    }
}
