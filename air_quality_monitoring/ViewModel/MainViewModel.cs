using System.Collections.ObjectModel;
using System.Windows.Input;
using air_quality_monitoring.Abstract;
using air_quality_monitoring.Interfaces;
using air_quality_monitoring.Models;
using air_quality_monitoring.Services;
using Microsoft.Win32; 
using System.Windows;
using air_quality_monitoring.Commands;
using air_quality_monitoring.Views;

namespace air_quality_monitoring.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly IMeasurementService _measurementService;
    private readonly IFileService _fileService;

    public ObservableCollection<AirMeasurement> Measurements { get; set; }
    public ObservableCollection<AirMeasurement> SelectedMeasurements { get; set; } = new ObservableCollection<AirMeasurement>(); 

    private AirMeasurement? _selectedMeasurement;
    public AirMeasurement? SelectedMeasurement
    {
        get => _selectedMeasurement;
        set
        {
            _selectedMeasurement = value;
            OnPropertyChanged(nameof(SelectedMeasurement));
        }
    }
    
    private string _selectedSortOption;
    public string SelectedSortOption
    {
        get => _selectedSortOption;
        set
        {
            _selectedSortOption = value;
            OnPropertyChanged(nameof(SelectedSortOption));
            SortMeasurements(); 
        }
    }

    public List<string> SortOptions { get; } = new List<string>
    {
        "Дата ↑",
        "Дата ↓",
        "Локація A→Z",
        "Локація Z→A",
        "CO₂ ↑",
        "CO₂ ↓"
    };
    
    public ICommand ShowGraphCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand LoadCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand MergeCommand { get; }

    public MainViewModel()
    {
        _measurementService = new MeasurementService();
        _fileService = new FileService();
        Measurements = new ObservableCollection<AirMeasurement>(_measurementService.GetAll());
        AddCommand = new RelayCommand(_ => AddMeasurement());
        RemoveCommand = new RelayCommand(_ => RemoveMeasurement(), _ => SelectedMeasurement != null);
        SaveCommand = new RelayCommand(_ => SaveToFile());
        LoadCommand = new RelayCommand(_ => LoadFromFile());
        EditCommand = new RelayCommand(_ => EditMeasurement(), _ => SelectedMeasurement != null);
        ShowGraphCommand = new RelayCommand(_ => ShowGraph());
        MergeCommand = new RelayCommand(MergeMeasurements);
    }

    private void ShowGraph()
    {
        var graphViewModel = new GraphViewModel(Measurements);
        var graphWindow = new GraphWindow(graphViewModel);
        graphWindow.Owner = Application.Current.MainWindow;
        graphWindow.ShowDialog();
    }

    private void AddMeasurement()
    {
        var window = new AddMeasurementWindow();
        if (window.ShowDialog() == true && window.Result != null)
        {
            _measurementService.Add(window.Result);
            Measurements.Add(window.Result);
        }
    }

    private void RemoveMeasurement()
    {
        if (SelectedMeasurement != null)
        {
            _measurementService.Remove(SelectedMeasurement);
            Measurements.Remove(SelectedMeasurement);
        }
    }

    private void SaveToFile()
    {
        var dialog = new SaveFileDialog
        {
            Filter = "JSON файли (*.json)|*.json",
            DefaultExt = ".json"
        };

        if (dialog.ShowDialog() == true)
        {
            _fileService.Save(dialog.FileName, _measurementService.GetAll());
            MessageBox.Show("Файл збережено успішно.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void LoadFromFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "JSON файли (*.json)|*.json",
            DefaultExt = ".json"
        };

        if (dialog.ShowDialog() == true)
        {
            var loaded = _fileService.Load(dialog.FileName).ToList();

            _measurementService.Clear();
            Measurements.Clear();

            foreach (var item in loaded)
            {
                _measurementService.Add(item);
                Measurements.Add(item);
            }
        }
    }

    private void EditMeasurement()
    {
        if (SelectedMeasurement == null) return;

        var window = new EditMeasurementWindow(SelectedMeasurement)
        {
            Owner = Application.Current.MainWindow
        };

        if (window.ShowDialog() == true)
        {
            var updated = window.UpdatedMeasurement;

            SelectedMeasurement.Location = updated.Location;
            SelectedMeasurement.Temperature = updated.Temperature;
            SelectedMeasurement.Humidity = updated.Humidity;
            SelectedMeasurement.CO2Level = updated.CO2Level;

            OnPropertyChanged(nameof(Measurements));
        }
    }

    private void MergeMeasurements(object? parameter)
    {
        if (SelectedMeasurements == null || SelectedMeasurements.Count != 2)
        {
            MessageBox.Show("Оберіть рівно 2 записи для об'єднання.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var first = SelectedMeasurements[0];
        var second = SelectedMeasurements[1];

        if (first == null || second == null)
        {
            MessageBox.Show("Один з обраних елементів є некоректним.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var merged = first + second;
        _measurementService.Add(merged);
        Measurements.Add(merged);

        _measurementService.Remove(first);
        _measurementService.Remove(second);
        Measurements.Remove(first);
        Measurements.Remove(second);
        

        MessageBox.Show("Записи об'єднано!", "Інфо", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    
    private void SortMeasurements()
    {
        var items = Measurements.ToList();

        IEnumerable<AirMeasurement> sorted = items;

        switch (SelectedSortOption)
        {
            case "Дата ↑":
                sorted = items.OrderBy(m => m.Timestamp);
                break;
            case "Дата ↓":
                sorted = items.OrderByDescending(m => m.Timestamp);
                break;
            case "Локація A→Z":
                sorted = items.OrderBy(m => m.Location);
                break;
            case "Локація Z→A":
                sorted = items.OrderByDescending(m => m.Location);
                break;
            case "CO₂ ↑":
                sorted = items.OrderBy(m => m.CO2Level);
                break;
            case "CO₂ ↓":
                sorted = items.OrderByDescending(m => m.CO2Level);
                break;
            default:
                return;
        }

        Measurements.Clear();
        foreach (var item in sorted)
        {
            Measurements.Add(item);
        }
    }
}