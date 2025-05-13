using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using air_quality_monitoring.Models;
using air_quality_monitoring.ViewModel;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Windows.Media;
using System.Windows.Shapes;

namespace air_quality_monitoring.Views;

public partial class AddMeasurementWindow : Window
{
    public AirMeasurement? Result { get; private set; }

    private readonly AddMeasurementViewModel _viewModel;
    private GMapMarker? _currentMarker;

    public AddMeasurementWindow()
    {
        InitializeComponent();
        _viewModel = new AddMeasurementViewModel();
        DataContext = _viewModel;

        InitMap();
    }

    private void InitMap()
    {
        GMaps.Instance.Mode = AccessMode.ServerOnly;
        MapControl.MapProvider = GMapProviders.OpenStreetMap;
        MapControl.Position = new PointLatLng(50.4501, 30.5234); 
        MapControl.MinZoom = 2;
        MapControl.MaxZoom = 18;
        MapControl.Zoom = 5;
        MapControl.ShowCenter = false;
    }

    private async void MapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var point = e.GetPosition(MapControl);
        var latlng = MapControl.FromLocalToLatLng((int)point.X, (int)point.Y);

        string location = await ReverseGeocodeAsync(latlng.Lat, latlng.Lng); 

        if (!string.IsNullOrWhiteSpace(location) && location != "Невідомо")
        {
            _viewModel.Location = location;
        }
        else
        {
            MessageBox.Show("Не вдалося визначити назву міста за координатами", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async Task<string> ReverseGeocodeAsync(double lat, double lng)
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WPF-App");

            string url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat.ToString(CultureInfo.InvariantCulture)}&lon={lng.ToString(CultureInfo.InvariantCulture)}&zoom=12&addressdetails=1";
            string response = await httpClient.GetStringAsync(url);

            using var document = JsonDocument.Parse(response);
            var root = document.RootElement;

            if (root.TryGetProperty("address", out JsonElement address))
            {
                if (address.TryGetProperty("city", out JsonElement city))
                    return city.GetString()!;
                if (address.TryGetProperty("town", out JsonElement town))
                    return town.GetString()!;
                if (address.TryGetProperty("village", out JsonElement village))
                    return village.GetString()!;
                if (address.TryGetProperty("hamlet", out JsonElement hamlet))
                    return hamlet.GetString()!;
                if (address.TryGetProperty("municipality", out JsonElement municipality))
                    return municipality.GetString()!;
                if (address.TryGetProperty("county", out JsonElement county))
                    return county.GetString()!;
                if (address.TryGetProperty("state", out JsonElement state))
                    return state.GetString()!;
            }

            return "Невідомо";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка при геокодуванні: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            return "Невідомо";
        }
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_viewModel.Location))
        {
            MessageBox.Show("Вкажіть локацію", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!double.TryParse(_viewModel.Temperature, NumberStyles.Float, CultureInfo.InvariantCulture, out double temperature))
        {
            MessageBox.Show("Некоректна температура", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!double.TryParse(_viewModel.Humidity, NumberStyles.Float, CultureInfo.InvariantCulture, out double humidity))
        {
            MessageBox.Show("Некоректна вологість", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!double.TryParse(_viewModel.CO2Level, NumberStyles.Float, CultureInfo.InvariantCulture, out double co2))
        {
            MessageBox.Show("Некоректний рівень CO₂", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Result = new AirMeasurement
        {
            Location = _viewModel.Location,
            Timestamp = _viewModel.Timestamp,
            Temperature = temperature,
            Humidity = humidity,
            CO2Level = co2
        };

        DialogResult = true;
        Close();
    }
}
