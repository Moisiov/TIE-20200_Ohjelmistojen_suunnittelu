using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PlotterService
{
    public interface IPlotService
    {
         void GetPlot(IEnumerable<PlotDataPoint> dataPoints, PlotType plotType, string title = "");
    }
}