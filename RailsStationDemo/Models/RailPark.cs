using System.Collections.Generic;
using System.ComponentModel;

namespace RailStationDemoApp.Models;

public class RailPark
{
    public RailPark(string id, string name, List<RailPoint> area) {
        Id = id;
        Name = name;
        Area = area;
        RailSegments = new List<RailSegment>();
    }

    public string Id {
        get; private set;
    }

    public string Name {
        get; private set;
    }

    public List<RailSegment> RailSegments {
        get; private set;
    }

    public List<RailPoint> Area {get; private set; }
}
