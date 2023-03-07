using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using RailStationDemoApp.Models;
using System.Collections.Generic;

namespace RailStationDemoApp.Services;
public class DrawService
{
    private readonly Dictionary<string, Color> highlightColors = new() {
                                                            { "Green", Colors.Green },
                                                            { "Blue", Colors.Blue },
                                                            { "Yellow", Colors.Yellow },
                                                            { "Red", Colors.Red },
                                                            { "Black", Colors.Black },
                                                            { "White", Colors.White },
                                                        };

    public int ImageWidth { get; private set; }

    public int ImageHeight { get; private set; }

    public DrawService(int pictureWidth, int pictureHeight) {
        ImageWidth = pictureWidth;
        ImageHeight = pictureHeight;
    }

    public Image GetDrawedRailroads(List<RailSegment> railroads) {
        var result = new Image();

        if (railroads == null || railroads.Count == 0) {
            return result;
        }
        
        var drawingVisual = new DrawingVisual();

        using (var drawingContext = drawingVisual.RenderOpen()) {
            railroads.ForEach(segment => drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point { X = segment.StartPoint.X, Y = segment.StartPoint.Y }, new Point { X = segment.EndPoint.X, Y = segment.EndPoint.Y }));
        }

        var bitmap = new RenderTargetBitmap(ImageWidth, ImageHeight, 96, 96, PixelFormats.Default);

        bitmap.Render(drawingVisual);

        result.Source = bitmap;

        return result;
    }

    public Image GetDrawedHighLightArea(List<RailPoint> highLightPolygon, string highlightColor) {
        var result = new Image();

        if (highLightPolygon == null || highLightPolygon.Count == 0) {
            return result;
        }

        var drawingVisual = new DrawingVisual();

        using (var drawingContext = drawingVisual.RenderOpen()) {
            var figure = new PathFigure {
                IsClosed = true,
                IsFilled = true
            };

            highLightPolygon.ForEach(point => figure.Segments.Add(new LineSegment { Point = new Point { X = point.X, Y = point.Y } }));

            figure.Freeze();

            var geometry = new PathGeometry();

            geometry.Figures.Add(figure);
            geometry.Freeze();

            drawingContext.DrawGeometry(new SolidColorBrush { Color = highlightColors[highlightColor], Opacity = 0.3 }, null, geometry);
        }

        var bitmap = new RenderTargetBitmap(1100, 750, 96, 96, PixelFormats.Default);

        bitmap.Render(drawingVisual);

        result.Source = bitmap;

        return result;
    }
}
