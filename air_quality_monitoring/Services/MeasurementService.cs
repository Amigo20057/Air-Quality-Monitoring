using air_quality_monitoring.Interfaces;
using air_quality_monitoring.Models;

namespace air_quality_monitoring.Services;

public class MeasurementService : IMeasurementService
{
    private readonly List<AirMeasurement> _measurements = new();

    public IEnumerable<AirMeasurement> GetAll()
    {
        return _measurements;
    }

    public void Add(AirMeasurement measurement)
    {
        if (measurement == null)
            throw new ArgumentNullException(nameof(measurement));

        _measurements.Add(measurement);
    }

    public void Remove(AirMeasurement measurement)
    {
        if (measurement == null)
            throw new ArgumentNullException(nameof(measurement));

        _measurements.Remove(measurement);
    }

    public void Clear()
    {
        _measurements.Clear();
    }
    
    public IEnumerable<AirMeasurement> Filter(Func<AirMeasurement, bool> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return _measurements.Where(predicate);
    }
}