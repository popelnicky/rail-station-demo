using RailStationDemoApp.ViewModels;
using System.Windows;

namespace RailsStationDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{    
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new StationViewModel();
    }
}
