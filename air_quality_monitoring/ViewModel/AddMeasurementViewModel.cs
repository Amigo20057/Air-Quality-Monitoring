using System;
using System.ComponentModel;

namespace air_quality_monitoring.ViewModel;

public class AddMeasurementViewModel : INotifyPropertyChanged
{
    public DateTime Timestamp { get; set; } = DateTime.Now;

    private string _location = "";
    public string Location
    {
        get => _location;
        set { _location = value; OnPropertyChanged(nameof(Location)); }
    }

    public string Temperature { get; set; } = "";
    public string Humidity { get; set; } = "";
    public string CO2Level { get; set; } = "";

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}