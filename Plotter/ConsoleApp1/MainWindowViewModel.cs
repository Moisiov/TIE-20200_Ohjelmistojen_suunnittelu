using System;
using System.Collections.ObjectModel;
using PlotterService;

namespace ConsoleApp1
{
    public class MainWindowViewModel
    {
        private PlotService m_plotService;
        public MainWindowViewModel()
        {
            m_plotService = new PlotService();
        }
        
        public void GetBarPlot()
        {
            var items = new Collection<PlotDataPoint>
            {
                new PlotDataPoint {Label = "Apples", Value = new TimeSpan(2,1,0)},
                new PlotDataPoint {Label = "Pears", Value = new TimeSpan(1,1,0)},
                new PlotDataPoint {Label = "Bananas", Value = new TimeSpan(3,1,0)}
            };
            m_plotService.GetPlot(items, PlotType.TimeSpanBarPlot, "Fruit Bar Plot");
        }
        
        public void GetLinePlot()
        {
            var items = new Collection<PlotDataPoint>
            {
                new PlotDataPoint {Label = "Apples", Value = 22},
                new PlotDataPoint {Label = "Pears", Value = 30},
                new PlotDataPoint {Label = "Bananas", Value = 16}
            };
            m_plotService.GetPlot(items, PlotType.LinePlot, "Fruit Line Plot");
        }
        
        public void GetLinePlotTimeSpan()
        {
            var items = new Collection<PlotDataPoint>
            {
                new PlotDataPoint {Label = "Apples", Value = new TimeSpan(2,1,0)},
                new PlotDataPoint {Label = "Pears", Value = new TimeSpan(1,1,0)},
                new PlotDataPoint {Label = "Bananas", Value = new TimeSpan(3,1,0)}
            };
            m_plotService.GetPlot(items, PlotType.TimeSpanLinePlot, "Fruit Line Plot");
        }
        public void GetPiePlot()
        {
            var items = new Collection<PlotDataPoint>
            {
                new PlotDataPoint {Label = "Apples", Value = 22},
                new PlotDataPoint {Label = "Pears", Value = 30},
                new PlotDataPoint {Label = "Bananas", Value = 16}
            };
            m_plotService.GetPlot(items, PlotType.PiePlot, "Fruit Pie Plot");
        }
    }
}