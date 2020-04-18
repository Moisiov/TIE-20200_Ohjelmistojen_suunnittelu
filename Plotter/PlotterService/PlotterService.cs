// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Example.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace PlotterService
{
    public class PlotService : IPlotService
    {
        public void GetPlot(IEnumerable<PlotDataPoint> dataPoints, PlotType plotType, string title = "")
        {
            var plotWindow = (PlotView)Activator.CreateInstance(typeof(PlotView));
            
            switch (plotType)
            {
                case PlotType.BarPlot:
                    plotWindow.SetBarPlot(dataPoints
                        .Select(x => new DoubleDataPoint
                        {
                            Value = (int)x.Value,
                            Label = x.Label
                        })
                        .ToList(), title);
                    break;
                case PlotType.TimeSpanBarPlot:
                    plotWindow.SetTimeSpanBarPlot(dataPoints
                        .Select(x => new TimeSpanDataPoint
                        {
                            Value = (TimeSpan)x.Value,
                            Label = x.Label
                        }).ToList(), title);
                    break;
                case PlotType.LinePlot:
                    plotWindow.SetLinePlot(dataPoints
                        .Select(x => new DoubleDataPoint
                        {
                            Value = (int)x.Value,
                            Label = x.Label
                        })
                        .ToList(), title);
                    break;
                
                case PlotType.TimeSpanLinePlot:
                    plotWindow.SetTimeSpanLinePlot(dataPoints
                        .Select(x => new TimeSpanDataPoint
                        {
                            Value = (TimeSpan)x.Value,
                            Label = x.Label
                        }).ToList(), title);
                    break;
                
                case PlotType.PiePlot:
                    plotWindow.SetPiePlot(dataPoints
                        .Select(x => new DoubleDataPoint
                        {
                            Value = (int)x.Value,
                            Label = x.Label
                        })
                        .ToList(), title);
                    break;
                
                default:
                    break;
            }
            
            plotWindow.Show();
        }
    }

    public enum PlotType
    {
        BarPlot,
        TimeSpanBarPlot,
        LinePlot,
        TimeSpanLinePlot,
        PiePlot
    }
    
    public class PlotDataPoint
    {
        public string Label { get; set; }
        public object Value { get; set; }
    }
}