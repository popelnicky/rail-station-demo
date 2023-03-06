using RailStationDemoApp.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

//TODO: Needs to resolve a problem with Canvas container
public class RailParksControl : StationControl
{
    private Dictionary<string, Color> highlightColors = new() {
                                                            { "Green", Colors.Green },
                                                            { "Blue", Colors.Blue },
                                                            { "Yellow", Colors.Yellow }
                                                        };
    public Image HighLightLayer;

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
            if (HighLightLayer != null) {
                StationView.Children.Remove(HighLightLayer);
            }
            var drawingVisual = new DrawingVisual();

            using (var drawingContext = drawingVisual.RenderOpen()) {
                var figure = new PathFigure {
                    IsClosed = true,
                    IsFilled = true
                };

                HighLightArea.ForEach(point => figure.Segments.Add(new LineSegment { Point = new Point { X = point.X, Y = point.Y } }));

                figure.Freeze();

                var geometry = new PathGeometry();

                geometry.Figures.Add(figure);
                geometry.Freeze();

                drawingContext.DrawGeometry(new SolidColorBrush { Color = highlightColors[HighLightColor], Opacity = 0.3 }, null, geometry);
            }

            var bitmap = new RenderTargetBitmap(1095, 745, 96, 96, PixelFormats.Default);

            bitmap.Render(drawingVisual);

            HighLightLayer = new Image { Source = bitmap };

            StationView.Children.Add(HighLightLayer);
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
