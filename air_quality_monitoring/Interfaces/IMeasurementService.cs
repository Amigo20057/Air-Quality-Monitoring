using air_quality_monitoring.Models;

namespace air_quality_monitoring.Interfaces;
using System;
using System.Collections.Generic;

public interface IMeasurementService
{
    IEnumerable<AirMeasurement> GetAll();
    void Add(AirMeasurement measurement);
    void Remove(AirMeasurement measurement);
    void Clear();
    IEnumerable<AirMeasurement> Filter(Func<AirMeasurement, bool> predicate);
}
