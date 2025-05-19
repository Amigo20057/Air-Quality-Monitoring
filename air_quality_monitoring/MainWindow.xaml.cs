using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using air_quality_monitoring.Models;
using air_quality_monitoring.ViewModel;

namespace air_quality_monitoring;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool _isDarkTheme = false;
    public MainWindow()
    {
        InitializeComponent();
        dataGrid.SelectionChanged += DataGrid_SelectionChanged;
    }
    
    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.SelectedMeasurements.Clear();
            foreach (var item in dataGrid.SelectedItems)
            {
                if (item is AirMeasurement measurement)
                    viewModel.SelectedMeasurements.Add(measurement);
            }
        }
    }
    private void ToggleTheme_Click(object sender, RoutedEventArgs e)
    {
        var dict = new ResourceDictionary();
        if (_isDarkTheme)
            dict.Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative);
        else
            dict.Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);

        var mergedDicts = Application.Current.Resources.MergedDictionaries;
        mergedDicts.Clear();
        mergedDicts.Add(dict);

        _isDarkTheme = !_isDarkTheme;
    }
    
}