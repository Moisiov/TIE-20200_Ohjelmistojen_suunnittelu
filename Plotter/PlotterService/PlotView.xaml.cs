// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Avalonia;
using OxyPlot.Series;
using CategoryAxis = OxyPlot.Axes.CategoryAxis;
using ColumnSeries = OxyPlot.Series.ColumnSeries;
using LineSeries = OxyPlot.Series.LineSeries;
using PieSeries = OxyPlot.Series.PieSeries;
using PointAnnotation = OxyPlot.Annotations.PointAnnotation;
using TimeSpanAxis = OxyPlot.Axes.TimeSpanAxis;

namespace PlotterService
{
    public class PlotView : Avalonia.Controls.Window
    {
        private PlotModel m_plotModel;
        private string m_title;

        public PlotView()
        {
            this.InitializeComponent();
        }

        private void FinalizePlot(PlotModel plotModel)
        {
            var customController = new PlotController();
            customController.UnbindMouseDown(OxyMouseButton.Left);
            customController.BindMouseEnter(PlotCommands.HoverSnapTrack);
            DataContext = new { Plot = plotModel, Controller = customController };
        }

        public void SetBarPlot(List<DoubleDataPoint> dataPoints, string title)
        {
            m_title = title;
            m_plotModel = new PlotModel { Title = m_title };
            
            m_plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom, 
                ItemsSource = dataPoints, 
                LabelField = "Label",
                Angle = 90,
            });
            
            m_plotModel.Series.Add(new ColumnSeries
            {
                ItemsSource = dataPoints,
                ValueField="Value",
            });
            
            for (var i = 0; i < dataPoints.Count; i++)
            {
                var pointAnnotation = new PointAnnotation()
                {
                    X = i,
                    Y = dataPoints[i].Value,
                    Text = dataPoints[i].Value.ToString(),
                    Shape = 0,
                };
                m_plotModel.Annotations.Add(pointAnnotation);
            }

            FinalizePlot(m_plotModel);
        }
        
        public void SetTimeSpanBarPlot(List<TimeSpanDataPoint> dataPoints, string title)
        {
            m_title = title;
            m_plotModel = new PlotModel { Title = m_title };
            
            m_plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom, 
                ItemsSource = dataPoints, 
                LabelField = "Label",
                Angle = 90,
            });

            m_plotModel.Axes.Add(new TimeSpanAxis()
            {
                Position = AxisPosition.Left,
            });
            
            for (var i = 0; i < dataPoints.Count; i++)
            {
                var pointAnnotation = new PointAnnotation()
                {
                    X = i,
                    Y = dataPoints[i].Value.TotalSeconds,
                    Text = dataPoints[i].Value.ToString(),
                    Shape = 0,
                };
                
                m_plotModel.Annotations.Add(pointAnnotation);
            }
            
            m_plotModel.Series.Add(new ColumnSeries
            {
                ItemsSource = m_plotModel.Annotations,
                ValueField="Y",
            });

            FinalizePlot(m_plotModel);
        }
        
        public void SetLinePlot(List<DoubleDataPoint> dataPoints, string title)
        {
            m_title = title;
            m_plotModel = new PlotModel { Title = m_title };
            
            m_plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom, 
                ItemsSource = dataPoints, 
                LabelField = "Label",
                Angle = 90,
            });
            
            for (var i = 0; i < dataPoints.Count; i++)
            {
                var pointAnnotation = new PointAnnotation()
                {
                    X = i,
                    Y = dataPoints[i].Value,
                    Text = dataPoints[i].Value.ToString(),
                };
                m_plotModel.Annotations.Add(pointAnnotation);
            }

            m_plotModel.Series.Add(new LineSeries
            {
                DataFieldY = "Y",
                DataFieldX = "X",
                ItemsSource = m_plotModel.Annotations,
                StrokeThickness=2,
                Color=OxyColors.Black
            });

            FinalizePlot(m_plotModel);
        }
        
        public void SetTimeSpanLinePlot(List<TimeSpanDataPoint> dataPoints, string title)
        {
            m_title = title;
            m_plotModel = new PlotModel { Title = m_title };
            
            m_plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom, 
                ItemsSource = dataPoints, 
                LabelField = "Label",
                Angle = 90,
            });

            m_plotModel.Axes.Add(new TimeSpanAxis()
            {
                Position = AxisPosition.Left,
            });
            
            for (var i = 0; i < dataPoints.Count; i++)
            {
                var pointAnnotation = new PointAnnotation()
                {
                    X = i,
                    Y = dataPoints[i].Value.TotalSeconds,
                    Text = dataPoints[i].Value.ToString(),
                };
                
                m_plotModel.Annotations.Add(pointAnnotation);
            }

            m_plotModel.Series.Add(new LineSeries
            {
                DataFieldY = "Y",
                DataFieldX = "X",
                ItemsSource = m_plotModel.Annotations,
                StrokeThickness=2,
                Color=OxyColors.Black
            });

            FinalizePlot(m_plotModel);
        }
        
        public void SetPiePlot(List<DoubleDataPoint> dataPoints, string title)
        {
            m_title = title;
            m_plotModel = new PlotModel { Title = m_title };
            
            var pieSeries = new PieSeries();

            foreach (var point in dataPoints)
            {
                pieSeries.Slices.Add(new PieSlice(point.Label, point.Value) { IsExploded = true });
            }
            
            for (var i = 0; i < dataPoints.Count; i++)
            {
                var pointAnnotation = new PointAnnotation()
                {
                    X = i,
                    Y = dataPoints[i].Value,
                    Text = dataPoints[i].Value.ToString(),
                    Shape = 0,
                };
                m_plotModel.Annotations.Add(pointAnnotation);
            }
            
            m_plotModel.Series.Add(pieSeries);

            FinalizePlot(m_plotModel);
        }

        public async void ExportToPng(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog();
            var folderName = await dlg.ShowAsync(this);
            if (folderName != null)
            {
                using (Stream output = new FileStream($"{folderName}/{m_title}.png", FileMode.OpenOrCreate))
                {
                    PngExporter.Export(m_plotModel, output, 600, 400, OxyColors.White);   
                }
            }
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
    
    public class DoubleDataPoint
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
    
    public class TimeSpanDataPoint
    {
        public string Label { get; set; }
        public TimeSpan Value { get; set; }
    }
    
}