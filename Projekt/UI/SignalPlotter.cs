using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace Projekt.UI
{
    class SignalPlotter
    {
        private readonly Chart chart;

        public SignalPlotter(Chart targetChart)
        {
            chart = targetChart;
            if (chart.Series.Count == 0)
            {
                var series = new Series
                {
                    ChartType = SeriesChartType.Line,
                    Name = "Signal"
                };
                chart.Series.Add(series);
            }
        }

        public void Plot(double[] x, double[] y, string seriesName)
        {
            var series = chart.Series[0];
            series.Points.Clear();

            for (int i = 0; i < x.Length && i < y.Length; i++)
            {
                series.Points.AddXY(x[i], y[i]);
            }

            if (!string.IsNullOrEmpty(seriesName))
                series.Name = seriesName;

            chart.ChartAreas[0].RecalculateAxesScale();
        }

        public void SetAxisLabels(string xLabel, string yLabel)
        {
            chart.ChartAreas[0].AxisX.Title = xLabel;
            chart.ChartAreas[0].AxisY.Title = yLabel;
        }
    }
}
