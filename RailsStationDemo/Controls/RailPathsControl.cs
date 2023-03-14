using RailStationDemoApp.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RailStationDemoApp.Controls;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:RailStationDemoApp.Controls"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:RailStationDemoApp.Controls;assembly=RailStationDemoApp.Controls"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Browse to and select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:RailPathsControl/>
///
/// </summary>

public class RailPathsControl : StationControl
{
    private Image railPathsLayer;

    static RailPathsControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RailPathsControl), new FrameworkPropertyMetadata(typeof(RailPathsControl)));
    }

    private static readonly DependencyProperty RailPathProperty = DependencyProperty.Register(
                                                                "RailPath", typeof(List<RailSegment>),
                                                                typeof(RailPathsControl),
                                                                new PropertyMetadata(null, OnRailPathChanged));

    private static void OnRailPathChanged(DependencyObject obj, DependencyPropertyChangedEventArgs evArgs) {
        var control = (RailPathsControl) obj;

        control.RailPath = (List<RailSegment>) evArgs.NewValue;

        control.HandleRailPathChanged();
    }

    public List<RailSegment> RailPath {
        get => (List<RailSegment>) GetValue(RailPathProperty);
        set => SetValue(RailPathProperty, value);
    }

    public void HandleRailPathChanged() {
        if (railPathsLayer != null) {
            StationView.Children.Remove(railPathsLayer);
        }

        railPathsLayer = DrawService.GetDrawedRailPath(RailPath);

        StationView.Children.Add(railPathsLayer);
    }
}
