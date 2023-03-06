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
///     xmlns:MyNamespace="clr-namespace:RailStationDemoApp.Controls;assembly=RailsStationDemoApp.Controls"
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
///     <MyNamespace:StationControl/>
///
/// </summary>
public class StationControl : Control
{
    public static Canvas? StationView;
    
    private static readonly DependencyProperty RailroadsProperty = DependencyProperty.Register(
                                                                "Railroads", typeof(List<RailSegment>),
                                                                typeof(StationControl),
                                                                new PropertyMetadata(null, OnRailroadsChanged));

    private static void OnRailroadsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs evArgs) {
        var control = (StationControl) obj;

        control.Railroads = (List<RailSegment>) evArgs.NewValue;

        control.HandleRailroadsChanged();
    }

    static StationControl() {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StationControl), new FrameworkPropertyMetadata(typeof(StationControl)));
    }

    public List<RailSegment> Railroads {
        get => (List<RailSegment>) GetValue(RailroadsProperty);
        set => SetValue(RailroadsProperty, value);
    }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();

        if (Template.FindName("StationView", this) is Canvas canvas) {
            StationView = canvas;
        }

        HandleRailroadsChanged();
    }

    public void HandleRailroadsChanged() {
        if (Railroads != null & StationView != null) {
            StationView.Children.Clear();

            var drawingVisual = new DrawingVisual();

            using (var drawingContext = drawingVisual.RenderOpen()) {
                Railroads.ForEach(segment => drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point { X = segment.StartPoint.X, Y = segment.StartPoint.Y }, new Point { X = segment.EndPoint.X, Y = segment.EndPoint.Y }));
            }
            
            var bitmap = new RenderTargetBitmap(1095, 745, 96, 96, PixelFormats.Default);

            bitmap.Render(drawingVisual);
            
            StationView.Children.Add(new Image { Source = bitmap });
        }
    }
}
