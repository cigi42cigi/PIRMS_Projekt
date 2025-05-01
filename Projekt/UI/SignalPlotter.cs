using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using System.Windows.Forms.DataVisualization.Charting;

namespace Projekt.UI
{
    public class SignalPlotter
    {
        private readonly Chart chart;
        private readonly Timer liveTimer;
        private readonly List<double> xBuffer = new List<double>();
        private readonly List<double> yBuffer = new List<double>();
        private double time = 0;
        private const double dt = 0.01; // krok 10 ms
        private Func<double, double> signalGenerator = null;
        private Dictionary<string, Series> liveSeries = new Dictionary<string, Series>();

        public SignalPlotter(Chart targetChart)
        {
            chart = targetChart;

            // Timer pro živý režim
            liveTimer = new Timer();
            liveTimer.Interval = (int)(dt * 1000);
            liveTimer.Tick += LiveTimer_Tick;
        }

        /// <summary>
        /// Zahájí živé vykreslování. signalFunc dostane čas a vrací hodnotu signálu.
        /// </summary>
        public void StartLive(Func<double, double> signalFunc)
        {
            signalGenerator = signalFunc;
            xBuffer.Clear();
            yBuffer.Clear();
            time = 0;
            liveTimer.Start();
        }

        /// <summary>
        /// Zastaví živé vykreslování.
        /// </summary>
        public void StopLive()
        {
            liveTimer.Stop();
        }

        private void LiveTimer_Tick(object sender, EventArgs e)
        {
            if (signalGenerator == null) return;

            double y = signalGenerator(time);
            xBuffer.Add(time);
            yBuffer.Add(y);
            time += dt;

            if (xBuffer.Count > 500)
            {
                xBuffer.RemoveAt(0);
                yBuffer.RemoveAt(0);
            }

            chart.Series.Clear();
            var series = new Series("Live") { ChartType = SeriesChartType.Line };
            for (int i = 0; i < xBuffer.Count; i++)
                series.Points.AddXY(xBuffer[i], yBuffer[i]);

            chart.Series.Add(series);
            chart.ChartAreas[0].AxisX.Minimum = xBuffer[0];
            chart.ChartAreas[0].AxisX.Maximum = xBuffer[xBuffer.Count - 1];
            chart.ChartAreas[0].RecalculateAxesScale();
        }

        public void Plot(double[] x, double[] y, string seriesName)
        {
            var series = new Series(seriesName)
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2
            };

            for (int i = 0; i < x.Length && i < y.Length; i++)
            {
                series.Points.AddXY(x[i], y[i]);
            }

            chart.Series.Add(series);
            chart.ChartAreas[0].RecalculateAxesScale();
        }

        public void SetAxisLabels(string xLabel, string yLabel)
        {
            chart.ChartAreas[0].AxisX.Title = xLabel;
            chart.ChartAreas[0].AxisY.Title = yLabel;
        }

        public void Clear()
        {
            if (chart == null) return;
            chart.Series.Clear();
        }

        public void StartLiveDual(Func<double, double> rawSignalFunc, Func<double, double> filteredSignalFunc)
        {
            chart.Series.Clear();
            liveSeries.Clear();

            var raw = new Series("Raw") { ChartType = SeriesChartType.Line };
            var filtered = new Series("Filtered") { ChartType = SeriesChartType.Line };

            chart.Series.Add(raw);
            chart.Series.Add(filtered);

            liveSeries["Raw"] = raw;
            liveSeries["Filtered"] = filtered;

            xBuffer.Clear();
            yBuffer.Clear();
            time = 0;

            liveTimer.Tick -= LiveTimer_Tick;
            liveTimer.Tick += (s, e) =>
            {
                double rawVal = rawSignalFunc(time);
                double filteredVal = filteredSignalFunc(time);
                time += dt;

                // Přidání do grafu
                liveSeries["Raw"].Points.AddXY(time, rawVal);
                liveSeries["Filtered"].Points.AddXY(time, filteredVal);

                if (liveSeries["Raw"].Points.Count > 500)
                {
                    liveSeries["Raw"].Points.RemoveAt(0);
                    liveSeries["Filtered"].Points.RemoveAt(0);
                }

                chart.ChartAreas[0].AxisX.Minimum = time - 5;
                chart.ChartAreas[0].AxisX.Maximum = time;
                chart.ChartAreas[0].RecalculateAxesScale();
            };

            liveTimer.Start();
        }
    }
}
