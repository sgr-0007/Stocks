using Xunit;

public class HelloWorldTests
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        Assert.Equal(2, 1 + 1);
    }
}