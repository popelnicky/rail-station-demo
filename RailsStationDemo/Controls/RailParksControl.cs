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
///     <MyNamespace:RailParksControl/>
///
/// </summary>

public class RailParksControl : StationControl
{
    
    private Image highLightLayer;

    static RailParksControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RailParksControl), new FrameworkPropertyMetadata(typeof(RailParksControl)));
    }

    #region: HighLightArea property

    private static readonly DependencyProperty HighLightAreaProperty = DependencyProperty.Register(
                                                                "HighLightArea", typeof(List<RailPoint>),
                                                                typeof(RailParksControl),
                                                                new PropertyMetadata(null, OnRailParkAreaChanged));

    private static void OnRailParkAreaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs evArgs) {
        var control = (RailParksControl) obj;

        control.HighLightArea = (List<RailPoint>) evArgs.NewValue;

        control.HandleHighLightAreaChanged();
    }

    public List<RailPoint> HighLightArea {
        get => (List<RailPoint>) GetValue(HighLightAreaProperty);
        set => SetValue(HighLightAreaProperty, value);
    }

    public void HandleHighLightAreaChanged() {
        if (HighLightArea != null & HighLightColor != null) {
            if (highLightLayer != null) {
                StationView.Children.Remove(highLightLayer);
            }

            highLightLayer = DrawService.GetDrawedHighLightArea(HighLightArea, HighLightColor);

            StationView.Children.Add(highLightLayer);
        }
    }

    #endregion

    #region: HighLightColor property
    private static readonly DependencyProperty HighLightColorProperty = DependencyProperty.Register(
                                                                "HighLightColor", typeof(string),
                                                                typeof(RailParksControl),
                                                                new PropertyMetadata(null, OnRailParkAreaColorChanged));

    private static void OnRailParkAreaColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs evArgs) {
        var control = (RailParksControl) obj;

        control.HighLightColor = (string) evArgs.NewValue;

        control.HandleHighLightAreaChanged();
    }

    public string HighLightColor {
        get => (string) GetValue(HighLightColorProperty);
        set => SetValue(HighLightColorProperty, value);
    }
    #endregion
}
