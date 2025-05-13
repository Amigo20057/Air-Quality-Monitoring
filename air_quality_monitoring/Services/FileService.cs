using System.IO;
using System.Text.Json;
using air_quality_monitoring.Interfaces;
using air_quality_monitoring.Models;

namespace air_quality_monitoring.Services;

public class FileService : IFileService
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public void Save(string path, IEnumerable<AirMeasurement> measurements)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Шлях до файлу не задано.");

        var json = JsonSerializer.Serialize(measurements, _options);
        File.WriteAllText(path, json);
    }

    public IEnumerable<AirMeasurement> Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Файл не знайдено.", path);

        var json = File.ReadAllText(path);
        var result = JsonSerializer.Deserialize<List<AirMeasurement>>(json);

        return result ?? new List<AirMeasurement>();
    }
}