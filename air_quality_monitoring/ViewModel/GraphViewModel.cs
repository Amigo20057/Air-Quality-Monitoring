using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using air_quality_monitoring.Abstract;
using air_quality_monitoring.Models;

namespace air_quality_monitoring.ViewModel;

public class GraphViewModel : ViewModelBase
{
    public ObservableCollection<AirMeasurement> Measurements { get; }

    public List<string> AvailableParameters { get; } = new() { "Temperature", "Humidity", "CO2Level" };

    private string _selectedParameter = "Temperature";
    public string SelectedParameter
    {
        get => _selectedParameter;
        set
        {
            _selectedParameter = value;
            OnPropertyChanged(nameof(SelectedParameter));
            OnRedrawRequested?.Invoke();
        }
    }

    public event Action? OnRedrawRequested;

    public GraphViewModel(IEnumerable<AirMeasurement> measurements)
    {
        Measurements = new ObservableCollection<AirMeasurement>(measurements.OrderBy(m => m.Timestamp));
    }

    public List<Point> GetGraphPoints(double width, double height)
    {
        var values = Measurements.ToList();
        if (!values.Any()) return new();

        double minVal = values.Min(GetSelectedValue);
        double maxVal = values.Max(GetSelectedValue);
        double range = maxVal - minVal == 0 ? 1 : maxVal - minVal;

        double spacingX = width / Math.Max(1, values.Count - 1);

        var points = new List<Point>();
        for (int i = 0; i < values.Count; i++)
        {
            double x = i * spacingX;
            double y = height - ((GetSelectedValue(values[i]) - minVal) / range * height);
            points.Add(new Point(x, y));
        }

        return points;
    }

    public double GetSelectedValue(AirMeasurement m)
    {
        return SelectedParameter switch
        {
            "Temperature" => m.Temperature,
            "Humidity" => m.Humidity,
            "CO2Level" => m.CO2Level,
            _ => 0
        };
    }
}