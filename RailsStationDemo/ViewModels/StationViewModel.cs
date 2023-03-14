using RailStationDemoApp.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RailStationDemoApp.ViewModels;

public class StationViewModel : INotifyPropertyChanged
{
    private readonly RailStation railStation;
    private List<RailSegment> railroads;
    private string railParkEntry = string.Empty;
    private List<string> railParkEntries;
    private List<RailPoint> railParkArea;
    private List<string> colors;
    private string railParkAreaColor = string.Empty;
    private List<string> fromRailPoints;
    private List<string> toRailPoints;
    private string fromPoint;
    private string toPoint;
    private List<RailSegment> railPath;

    public event PropertyChangedEventHandler? PropertyChanged;

    public StationViewModel() {
        railStation = new RailStation();

        Railroads = railStation.AllRailSegmets;

        RailParkEntries = railStation.RailParks.Select(item => item.Name).ToList();

        RailParkAreaColors = new List<string>() { "Green", "Blue", "Yellow" };
        RailParkAreaColor = RailParkAreaColors.First();

        FromRailPoints = railStation.GetRailPointsCollection();
        ToRailPoints = railStation.GetRailPointsCollection();

        fromPoint = string.Empty;
        toPoint = string.Empty;
    }

    public List<RailSegment> Railroads {
        get => railroads;

        set {
            railroads = value;

            OnPropertyChanged(nameof(Railroads));
        }
    }

    #region: Combo Boxes data resources and binded data
    public List<string> RailParkEntries {
        get => railParkEntries;
        set {
            railParkEntries = value;

            OnPropertyChanged(nameof(RailParkEntries));
        }
    }

    public string RailParkEntry {
        get => railParkEntry;
        set {
            if (railParkEntry == value) {
                return;
            }

            railParkEntry = value;

            OnPropertyChanged(nameof(RailParkEntry));

            SetRailParkArea(railParkEntry);
        }
    }

    public List<RailPoint> RailParkArea {
        get => railParkArea;
        set { railParkArea = value; OnPropertyChanged(nameof(RailParkArea)); }

    }

    public List<string> RailParkAreaColors {
        get => colors;
        set {
            colors = value;

            OnPropertyChanged(nameof(RailParkAreaColors));
        }
    }

    public string RailParkAreaColor {
        get => railParkAreaColor;
        set {
            railParkAreaColor = value;

            OnPropertyChanged(nameof(RailParkAreaColor));
        }
    }

    public List<string> FromRailPoints {
        get => fromRailPoints;
        set {
            fromRailPoints = value;

            OnPropertyChanged(nameof(FromRailPoints));
        }
    }

    public List<string> ToRailPoints {
        get => toRailPoints;
        set {
            toRailPoints = value;

            OnPropertyChanged(nameof(ToRailPoints));
        }
    }

    public string FromRailPoint {
        get => fromPoint;
        set {
            fromPoint = value;

            OnPropertyChanged(nameof(FromRailPoint));

            TryToBuildRailPath();
        }
    }

    public string ToRailPoint {
        get => toPoint;
        set {
            toPoint = value;

            OnPropertyChanged(nameof(ToRailPoint));

            TryToBuildRailPath();
        }
    }

    public List<RailSegment> RailPath {
        get => railPath;
        set {
            railPath = value;
            
            OnPropertyChanged(nameof(RailPath));
        }
    }

    #endregion

    public void OnPropertyChanged(string prop = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private void SetRailParkArea(string parkName) {
        var railPark = railStation.RailParks.FirstOrDefault(item => item.Name == parkName);

        RailParkArea = railPark.Area;
    }

    private void TryToBuildRailPath() {
        if (string.IsNullOrEmpty(FromRailPoint) || string.IsNullOrEmpty(ToRailPoint)) {
            return;
        }

        RailPath = railStation.BuildRailPath(FromRailPoint, ToRailPoint);
    }
}
