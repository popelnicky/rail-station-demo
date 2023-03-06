namespace RailStationDemoApp.Models;

public class RailPoint
{
    public string Id { get; set; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public RailPoint(string id, int x, int y) {
        Id = id;
        X = x;
        Y = y;
    }

    //public RailPoint(int x, int y) {
    //    X = x;
    //    Y = y;
    //}
}
