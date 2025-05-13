namespace air_quality_monitoring.Interfaces;
using air_quality_monitoring.Models;
using System.Collections.Generic;

public interface IFileService
{
    void Save(string path, IEnumerable<AirMeasurement> measurements);
    IEnumerable<AirMeasurement> Load(string path);
}
