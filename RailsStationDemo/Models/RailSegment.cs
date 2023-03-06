using System;

namespace RailStationDemoApp.Models;

public class RailSegment
{
    public int Distance { get; set; }

    public RailPoint StartPoint { get; private set; }

    public RailPoint EndPoint { get; private set; }

    public RailSegment(RailPoint startPoint, RailPoint endPoint) {
        StartPoint = startPoint;
        EndPoint = endPoint;
        
        Distance = Math.Abs(endPoint.X - startPoint.X);
    }
}
