using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using air_quality_monitoring.ViewModel;

namespace air_quality_monitoring.Views;

public partial class GraphWindow : Window
{
    private readonly GraphViewModel _viewModel;

    public GraphWindow(GraphViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        _viewModel.OnRedrawRequested += DrawGraph;
        DrawGraph();
    }

    private void DrawGraph()
    {
        GraphCanvas.Children.Clear();

        var width = GraphCanvas.ActualWidth;
        var height = GraphCanvas.ActualHeight;

        var points = _viewModel.GetGraphPoints(width, height);
        var rawData = _viewModel.Measurements.ToList();

        if (points.Count < 2) return;

        DrawGrid(width, height, 5, 5); // 5 вертикальних і 5 горизонтальних ліній
        DrawAxes(width, height);

        var polyline = new Polyline
        {
            Stroke = Brushes.Blue,
            StrokeThickness = 2
        };

        for (int i = 0; i < points.Count; i++)
        {
            polyline.Points.Add(points[i]);

            // Підпис до точки
            var label = new TextBlock
            {
                Text = _viewModel.GetSelectedValue(rawData[i]).ToString("0.##"),
                FontSize = 10,
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(label, points[i].X + 2);
            Canvas.SetTop(label, points[i].Y - 15);
            GraphCanvas.Children.Add(label);
        }

        GraphCanvas.Children.Add(polyline);
    }
    
    private void DrawGrid(double width, double height, int verticalLines, int horizontalLines)
    {
        double spacingX = width / verticalLines;
        double spacingY = height / horizontalLines;

        for (int i = 0; i <= verticalLines; i++)
        {
            var line = new Line
            {
                X1 = spacingX * i,
                X2 = spacingX * i,
                Y1 = 0,
                Y2 = height,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1
            };
            GraphCanvas.Children.Add(line);
        }

        for (int i = 0; i <= horizontalLines; i++)
        {
            var line = new Line
            {
                X1 = 0,
                X2 = width,
                Y1 = spacingY * i,
                Y2 = spacingY * i,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1
            };
            GraphCanvas.Children.Add(line);
        }
    }

    private void DrawAxes(double width, double height)
    {
        var xAxis = new Line
        {
            X1 = 0,
            X2 = width,
            Y1 = height,
            Y2 = height,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        var yAxis = new Line
        {
            X1 = 0,
            X2 = 0,
            Y1 = 0,
            Y2 = height,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        GraphCanvas.Children.Add(xAxis);
        GraphCanvas.Children.Add(yAxis);
    }
}