using RailStationDemoApp.Services;

namespace RailsStationDemo.Tests;

public class DrawServiceTests
{
    private DrawService drawService;

    [SetUp]
    public void Setup()
    {
        drawService = new DrawService(640, 480);
    }

    [Test]
    public void Test()
    {
        Assert.Pass();
    }
}