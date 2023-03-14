using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RailStationDemoApp.Models;
public class RailStation
{
    

    private readonly int yOffset = 350;
    private RailPoint[] allRailPoints;
    private Dictionary<string, List<RailPoint>> railMap;
    private Dictionary<string, List<RailSegment>> railPaths;

    public List<RailPark> RailParks { get; private set; }

    public List<RailSegment> AllRailSegmets { get; private set; }

    public List<RailPoint> AllRailPoints { get; private set; }

    public RailStation() {
        railMap = new Dictionary<string, List<RailPoint>>();
        railPaths = new Dictionary<string, List<RailSegment>>();

        GenerateStation();

        AllRailSegmets = GetAllRailSegments();
        AllRailPoints = allRailPoints.ToList();

        BuildRailMap();
    }

    public List<RailSegment> BuildRailPath(string fromId, string toId) {
        var empty = new List<RailSegment>();

        if (string.IsNullOrEmpty(fromId) || string.IsNullOrEmpty(toId)) {
            return empty;
        }

        if (allRailPoints.FirstOrDefault(point => point.Id == fromId) == null || 
            allRailPoints.FirstOrDefault(point => point.Id == toId) == null) {
            return empty;
        }
        
        var key = $"{ fromId }->{ toId }";

        if (railPaths.ContainsKey(key)) {
            return railPaths[key];
        }
        
        var possibleSequences = new List<List<RailPoint>>();
        var possiblePaths = new List<List<RailSegment>>();
        var from = allRailPoints.First(railPoint => railPoint.Id == fromId);
        var to = allRailPoints.First(railPoint => railPoint.Id == toId);
        var sequence = new List<RailPoint>() { from };

        FindAllRailPaths(from, to, possibleSequences, sequence);

        foreach (var points in possibleSequences) {
            var possiblePath = new List<RailSegment>();

            possiblePaths.Add(possiblePath);

            for (var i = 0; i < points.Count - 1; i++) {
                possiblePath.Add(GetNewRailSegment(points[i], points[i + 1]));
            }
        }

        var minPathDistance = possiblePaths.Min(path => path.Sum(segment => segment.Distance));
        var path = possiblePaths.First(path => path.Sum(segment => segment.Distance) <= minPathDistance);

        railPaths[key] = path ?? empty;

        return railPaths[key];
    }

    public List<string> GetRailPointsCollection() {
        var result = new List<string>();
        var random = new Random();

        for (var i = 0; i < 3; i++) {
            var index = random.Next(AllRailPoints.Count());

            result.Add(AllRailPoints[index].Id);
        }

        return result;
    }

    private void BuildRailMap() {
        foreach(var railPoint in AllRailPoints) {
            railMap[railPoint.Id] = AllRailSegmets
                .Where(railSegment => railSegment.StartPoint == railPoint || railSegment.EndPoint == railPoint)
                .Select(railSegment => railSegment.StartPoint.Id != railPoint.Id ? railSegment.StartPoint : railSegment.EndPoint)
                .ToList();
        }
    }

    private void GenerateStation() {
        // Hardcoding model
        allRailPoints = new RailPoint[] {
            GetNewRailPoint("P0", 25, 0),
            GetNewRailPoint("P1", 75, 0),
            GetNewRailPoint("P2", 100, -25),
            GetNewRailPoint("P3", 175, -25),
            GetNewRailPoint("P4", 200, -50),
            GetNewRailPoint("P5", 250, -50),
            GetNewRailPoint("P6", 275, -75),
            GetNewRailPoint("P7", 325, -75),
            GetNewRailPoint("P8", 350, -100),
            GetNewRailPoint("P9", 375, -125),
            GetNewRailPoint("P10", 450, -125),
            GetNewRailPoint("P11", 475, -150),
            GetNewRailPoint("P12", 600, -150),
            GetNewRailPoint("P13", 625, -125),
            GetNewRailPoint("P14", 725, -125),
            GetNewRailPoint("P15", 750, -100),
            GetNewRailPoint("P16", 775, -100),
            GetNewRailPoint("P17", 800, -75),
            GetNewRailPoint("P18", 825, -50),
            GetNewRailPoint("P19", 900, -50),
            GetNewRailPoint("P20", 925, -25),
            GetNewRailPoint("P21", 1000, -25),
            GetNewRailPoint("P22", 1025, 0),
            GetNewRailPoint("P23", 1050, 0),
            GetNewRailPoint("P24", 750, 0),
            GetNewRailPoint("P25", 725, 25),
            GetNewRailPoint("P26", 700, 50),
            GetNewRailPoint("P27", 675, 50),
            GetNewRailPoint("P28", 650, 75),
            GetNewRailPoint("P29", 600, 75),
            GetNewRailPoint("P30", 575, 100),
            GetNewRailPoint("P31", 350, 100),
            GetNewRailPoint("P32", 325, 75),
            GetNewRailPoint("P33", 300, 75),
            GetNewRailPoint("P34", 275, 50),
            GetNewRailPoint("P35", 250, 50),
            GetNewRailPoint("P36", 225, 25),
            GetNewRailPoint("P37", 200, 25),
            GetNewRailPoint("P38", 175, 0),
            GetNewRailPoint("P39", 325, 25),
            GetNewRailPoint("P40", 500, 25),
            GetNewRailPoint("P41", 600, 25),
            GetNewRailPoint("P42", 575, 50),
            GetNewRailPoint("P43", 550, 75)
        };

        var northRailPark = new RailPark(Enum.GetName(typeof(RailParkEnum), RailParkEnum.North), "North Park", new List<RailPoint>() { allRailPoints[4], allRailPoints[11], allRailPoints[12], allRailPoints[19], allRailPoints[4] });
        var middleRailPark = new RailPark(Enum.GetName(typeof(RailParkEnum), RailParkEnum.Middle), "Middle Park", new List<RailPoint>() { allRailPoints[2], allRailPoints[21], allRailPoints[22], allRailPoints[1], allRailPoints[2] });
        var southRailPark = new RailPark(Enum.GetName(typeof(RailParkEnum), RailParkEnum.South), "South Park", new List<RailPoint>() { allRailPoints[38], allRailPoints[24], allRailPoints[26], allRailPoints[30], allRailPoints[31], allRailPoints[38] });

        RailParks = new List<RailPark> {
            northRailPark,
            middleRailPark,
            southRailPark
        };

        middleRailPark.RailSegments.AddRange(new RailSegment[] {
            GetNewRailSegment(allRailPoints[0], allRailPoints[1]),
            GetNewRailSegment(allRailPoints[1], allRailPoints[2]),
            GetNewRailSegment(allRailPoints[2], allRailPoints[3]),
            GetNewRailSegment(allRailPoints[3], allRailPoints[20]),
            GetNewRailSegment(allRailPoints[20], allRailPoints[21]),
            GetNewRailSegment(allRailPoints[21], allRailPoints[22]),
            GetNewRailSegment(allRailPoints[22], allRailPoints[23]),
            GetNewRailSegment(allRailPoints[1], allRailPoints[38]),
            GetNewRailSegment(allRailPoints[38], allRailPoints[24]),
            GetNewRailSegment(allRailPoints[24], allRailPoints[22])
        });

        northRailPark.RailSegments.AddRange(new RailSegment[] {
            GetNewRailSegment(allRailPoints[3], allRailPoints[4]),
            GetNewRailSegment(allRailPoints[4], allRailPoints[5]),
            GetNewRailSegment(allRailPoints[5], allRailPoints[18]),
            GetNewRailSegment(allRailPoints[18], allRailPoints[19]),
            GetNewRailSegment(allRailPoints[19], allRailPoints[20]),
            GetNewRailSegment(allRailPoints[5], allRailPoints[6]),
            GetNewRailSegment(allRailPoints[6], allRailPoints[7]),
            GetNewRailSegment(allRailPoints[7], allRailPoints[17]),
            GetNewRailSegment(allRailPoints[17], allRailPoints[18]),
            GetNewRailSegment(allRailPoints[7], allRailPoints[8]),
            GetNewRailSegment(allRailPoints[8], allRailPoints[15]),
            GetNewRailSegment(allRailPoints[15], allRailPoints[16]),
            GetNewRailSegment(allRailPoints[16], allRailPoints[17]),
            GetNewRailSegment(allRailPoints[8], allRailPoints[9]),
            GetNewRailSegment(allRailPoints[9], allRailPoints[10]),
            GetNewRailSegment(allRailPoints[10], allRailPoints[13]),
            GetNewRailSegment(allRailPoints[13], allRailPoints[14]),
            GetNewRailSegment(allRailPoints[14], allRailPoints[15]),
            GetNewRailSegment(allRailPoints[10], allRailPoints[11]),
            GetNewRailSegment(allRailPoints[11], allRailPoints[12]),
            GetNewRailSegment(allRailPoints[12], allRailPoints[13])
        });

        southRailPark.RailSegments.AddRange(new RailSegment[] {
            GetNewRailSegment(allRailPoints[38], allRailPoints[37]),
            GetNewRailSegment(allRailPoints[37], allRailPoints[36]),
            GetNewRailSegment(allRailPoints[36], allRailPoints[39]),
            GetNewRailSegment(allRailPoints[36], allRailPoints[35]),
            GetNewRailSegment(allRailPoints[35], allRailPoints[34]),
            GetNewRailSegment(allRailPoints[34], allRailPoints[42]),
            GetNewRailSegment(allRailPoints[42], allRailPoints[41]),
            GetNewRailSegment(allRailPoints[40], allRailPoints[41]),
            GetNewRailSegment(allRailPoints[41], allRailPoints[25]),
            GetNewRailSegment(allRailPoints[25], allRailPoints[24]),
            GetNewRailSegment(allRailPoints[34], allRailPoints[33]),
            GetNewRailSegment(allRailPoints[33], allRailPoints[32]),
            GetNewRailSegment(allRailPoints[32], allRailPoints[43]),
            GetNewRailSegment(allRailPoints[43], allRailPoints[42]),
            GetNewRailSegment(allRailPoints[32], allRailPoints[31]),
            GetNewRailSegment(allRailPoints[31], allRailPoints[30]),
            GetNewRailSegment(allRailPoints[30], allRailPoints[29]),
            GetNewRailSegment(allRailPoints[29], allRailPoints[28]),
            GetNewRailSegment(allRailPoints[28], allRailPoints[27]),
            GetNewRailSegment(allRailPoints[27], allRailPoints[26]),
            GetNewRailSegment(allRailPoints[26], allRailPoints[25])
        });
    }

    private List<RailSegment> GetAllRailSegments() {
        var result = new List<RailSegment>();

        RailParks.ForEach(railPark => result.AddRange(railPark.RailSegments));

        return result;
    }

    private RailPoint GetNewRailPoint(string id, int x, int y) { 
        return new RailPoint(id, x, yOffset + y);
    }

    private RailSegment GetNewRailSegment(RailPoint startPoint, RailPoint endPoint) {
        return new RailSegment(startPoint, endPoint);
    }

    private void FindAllRailPaths(RailPoint from, RailPoint to, List<List<RailPoint>> possiblePaths, List<RailPoint> sequence) {
        if (from.Id == to.Id) {
            possiblePaths.Add(sequence);

            return;
        }

        foreach (var next in railMap[from.Id]) {
            if (sequence.Contains(next)) {
                continue;
            }

            FindAllRailPaths(next, to, possiblePaths, new List<RailPoint>(sequence) { next });
        }
    }
}
