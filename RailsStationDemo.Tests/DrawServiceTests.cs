using RailStationDemoApp.Models;
using RailStationDemoApp.Services;
using System.Windows.Controls;

namespace RailsStationDemo.Tests;

public class DrawServiceTests
{
    private RailStation railStationModel;
    private DrawService drawService;
    private int width;
    private int height;

    [SetUp]
    public void Setup()
    {
        width = 640;
        height = 480;

        railStationModel = new RailStation();
        drawService = new DrawService(width, height);
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void GetDrawedRailroadsTest()
    {
        var drawedRailroads = drawService.GetDrawedRailroads(null);

        Assert.NotNull(drawedRailroads, "Returned result should not be null");

        drawedRailroads = drawService.GetDrawedRailroads(new List<RailSegment>());

        Assert.NotNull(drawedRailroads, "Returned result should not be null");

        drawedRailroads = drawService.GetDrawedRailroads(railStationModel.AllRailSegmets);

        CheckDrawedImage(drawedRailroads);
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void GetDrawedHighLightAreaTest() {
        List<RailPoint> highLightArea = null;
        var highLightColor = string.Empty;
        var drawedHighLightArea = drawService.GetDrawedHighLightArea(highLightArea, highLightColor);

        Assert.NotNull(drawedHighLightArea, "Returned result should not be null");

        highLightArea = new List<RailPoint>();
        highLightColor = "Blue";
        drawedHighLightArea = drawService.GetDrawedHighLightArea(highLightArea, highLightColor);

        Assert.NotNull(drawedHighLightArea, "Returned result should not be null");

        var defaultRailPark = railStationModel.RailParks[0];
        highLightArea = defaultRailPark.Area;
        drawedHighLightArea = drawService.GetDrawedHighLightArea(highLightArea, highLightColor);

        CheckDrawedImage(drawedHighLightArea);
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void GetDrawedRailPathTest() {
        var drawedRailPath = drawService.GetDrawedRailPath(null);

        Assert.NotNull(drawedRailPath, "Returned result should not be null");

        drawedRailPath = drawService.GetDrawedRailPath(new List<RailSegment>());

        Assert.NotNull(drawedRailPath, "Returned result should not be null");

        var railPath = railStationModel.BuildRailPath("P1", "P40");

        drawedRailPath = drawService.GetDrawedRailPath(railPath);

        CheckDrawedImage(drawedRailPath);
    }

    private void CheckDrawedImage(Image actual) {
        Assert.NotNull(actual, "Returned result  should not be null");
        Assert.NotNull(actual.Source, "A source for the result should not be null");

        Assert.That(actual.Source.Width, Is.EqualTo(width));
        Assert.That(actual.Source.Height, Is.EqualTo(height));
    }
}