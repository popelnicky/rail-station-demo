using RailStationDemoApp.Models;

namespace RailStationDemo.Tests;
public class RailStationTests
{
    private RailStation railStationModel;

    [SetUp]
    public void Setup() {
        railStationModel = new RailStation();
    }

    [Test]
    public void CheckGeneratedRailPointsTest() {
        Assert.NotNull(railStationModel.AllRailPoints);
        Assert.That(railStationModel.AllRailPoints.Count, Is.Not.EqualTo(0));

        var defaultRailPoint = railStationModel.AllRailPoints.FirstOrDefault();

        Assert.NotNull(defaultRailPoint);
        Assert.IsNotEmpty(defaultRailPoint.Id);
        Assert.That(defaultRailPoint.X, Is.Not.EqualTo(0));
        Assert.That(defaultRailPoint.Y, Is.Not.EqualTo(0));
    }

    [Test]
    public void CheckGeneratedRailSegmentsTest() {
        Assert.NotNull(railStationModel.AllRailSegmets);
        Assert.That(railStationModel.AllRailSegmets.Count, Is.Not.EqualTo(0));

        var defaultRailSegment = railStationModel.AllRailSegmets.FirstOrDefault();

        Assert.NotNull(defaultRailSegment.StartPoint);
        Assert.NotNull(defaultRailSegment.EndPoint);
        Assert.That(defaultRailSegment.Distance, Is.Not.EqualTo(0));

        defaultRailSegment = railStationModel.AllRailSegmets.FirstOrDefault(railSegment => railSegment.StartPoint == null || railSegment.EndPoint == null);

        Assert.Null(defaultRailSegment);
    }

    [Test]
    public void CheckGeneratedParksTest() {
        Assert.NotNull(railStationModel.RailParks);
        Assert.That(railStationModel.RailParks.Count, Is.Not.EqualTo(0));
        Assert.That(railStationModel.RailParks.Count, Is.EqualTo(3));

        var defaultRailPark = railStationModel.RailParks.FirstOrDefault();

        Assert.NotNull(defaultRailPark);

        Assert.IsNotEmpty(defaultRailPark.Id);
        Assert.IsNotEmpty(defaultRailPark.Name);
        
        Assert.NotNull(defaultRailPark.RailSegments);
        Assert.That(defaultRailPark.RailSegments.Count, Is.Not.EqualTo(0));

        var defaultRailSegmentFromPark = defaultRailPark.RailSegments.FirstOrDefault();

        Assert.NotNull(defaultRailSegmentFromPark);
        Assert.NotNull(defaultRailSegmentFromPark.StartPoint);
        Assert.NotNull(defaultRailSegmentFromPark.EndPoint);
        Assert.That(defaultRailSegmentFromPark.Distance, Is.Not.EqualTo(0));

        Assert.NotNull(defaultRailPark.Area);
        Assert.That(defaultRailPark.Area.Count, Is.Not.EqualTo(0));

        var defaultRailPointFromArea = defaultRailPark.Area.FirstOrDefault();

        Assert.NotNull(defaultRailPointFromArea);
        Assert.That(defaultRailPointFromArea.X, Is.Not.EqualTo(0));
        Assert.That(defaultRailPointFromArea.Y, Is.Not.EqualTo(0));
        Assert.IsNotEmpty(defaultRailPointFromArea.Id);
    }

    [Test]
    public void GetRailPointsCollectionTest() {
        var points = railStationModel.GetRailPointsCollection();

        Assert.NotNull(points);
        Assert.That(points.Count, Is.EqualTo(3));
    }

    [Test]
    public void BuildRailPathTest() {
        var from = string.Empty;
        var to = string.Empty;
        var railPath = railStationModel.BuildRailPath(from, to);

        CheckEmptyRailPath(railPath);

        from = "point8";
        to = "point0";

        railPath = railStationModel.BuildRailPath(from, to);

        CheckEmptyRailPath(railPath);

        from = "P9";
        to = "P99";

        railPath = railStationModel.BuildRailPath(from, to);

        CheckEmptyRailPath(railPath);

        from = "P2";
        to = "P2";

        railPath = railStationModel.BuildRailPath(from, to);

        CheckEmptyRailPath(railPath);

        from = "P17";
        to = "P25";

        railPath = railStationModel.BuildRailPath(from, to);

        var railPathDistance = railPath.Sum(railSegment => railSegment.Distance);

        Assert.NotNull(railPath);
        Assert.That(railPath.Count, Is.Not.EqualTo(0));
        Assert.That(railPathDistance, Is.Not.EqualTo(0));
    }

    private void CheckEmptyRailPath(List<RailSegment> railPath) {
        Assert.NotNull(railPath);
        Assert.That(railPath.Count, Is.EqualTo(0));
    }
}
