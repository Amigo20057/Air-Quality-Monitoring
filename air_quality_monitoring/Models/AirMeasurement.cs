using System.ComponentModel;

namespace air_quality_monitoring.Models;

using System;

public class AirMeasurement : INotifyPropertyChanged
    {
        private string _location = "";
        private double _temperature;
        private double _humidity;
        private double _co2Level;
        private DateTime _timestamp;

        public string Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        public double Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        public double Humidity
        {
            get => _humidity;
            set
            {
                if (_humidity != value)
                {
                    _humidity = value;
                    OnPropertyChanged(nameof(Humidity));
                }
            }
        }

        public double CO2Level
        {
            get => _co2Level;
            set
            {
                if (_co2Level != value)
                {
                    _co2Level = value;
                    OnPropertyChanged(nameof(CO2Level));
                }
            }
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged(nameof(Timestamp));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public static AirMeasurement operator +(AirMeasurement a, AirMeasurement b)
        {
            return new AirMeasurement
            {
                Location = $"{a.Location} + {b.Location}",
                Timestamp = a.Timestamp > b.Timestamp ? a.Timestamp : b.Timestamp,
                Temperature = (a.Temperature + b.Temperature) / 2.0,
                Humidity = (a.Humidity + b.Humidity) / 2.0,
                CO2Level = (a.CO2Level + b.CO2Level) / 2.0
            };
        }
    }
