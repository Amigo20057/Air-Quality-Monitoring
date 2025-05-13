using System.Windows;
using air_quality_monitoring.Models;

namespace air_quality_monitoring.Views
{
    public partial class EditMeasurementWindow : Window
    {
        public EditMeasurementWindow(AirMeasurement measurement)
        {
            InitializeComponent();
            // Копія об'єкта для ізольованого редагування
            DataContext = new AirMeasurement
            {
                Location = measurement.Location,
                Temperature = measurement.Temperature,
                Humidity = measurement.Humidity,
                CO2Level = measurement.CO2Level,
                Timestamp = measurement.Timestamp
            };
        }

        public AirMeasurement UpdatedMeasurement => (AirMeasurement)DataContext;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}