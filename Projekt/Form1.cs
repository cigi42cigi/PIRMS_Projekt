using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

using Projekt.UI;
using Projekt.filters;

namespace Projekt
{
    public partial class Form1 : Form
    {
        private SignalPlotter plotter;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;

        private FIR_filter fir;
        private Queue<double> inputBuffer = new Queue<double>();
        private int filterOrder = 40;
        private double sampleRate = 100; // 100 Hz
        private double cutoffFreq = 5;   // např. 5 Hz filtr
        private Random rand = new Random(); // sdílený generátor

        public Form1()
        {
            InitializeComponent();
            chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chart.Dock = DockStyle.Fill;
            chart.ChartAreas.Add(new System.Windows.Forms.DataVisualization.Charting.ChartArea());
            this.Controls.Add(chart);

        }
     
        private void Form1_Load(object sender, EventArgs e)
        {
            chart.ChartAreas[0].AxisY.Minimum = -2;
            chart.ChartAreas[0].AxisY.Maximum = 2;

            fir = new FIR_filter(40, 100, 5);
            plotter = new SignalPlotter(chart);

            plotter.StartLiveDual(
                t =>
                {
                    // Generuj původní signál
                    double input = Math.Sin(2 * Math.PI * 2 * t) + 0.3 * (rand.NextDouble() - 0.5);
                    inputBuffer.Enqueue(input);
                    if (inputBuffer.Count > fir.Coefficients.Length)
                        inputBuffer.Dequeue();
                    return input;
                },
                t =>
                {
                    // Aplikuj FIR filtr na aktuální buffer
                    double[] inputArray = inputBuffer.ToArray();
                    double filtered = 0;
                    for (int i = 0; i < inputArray.Length; i++)
                    {
                        filtered += fir.Coefficients[i] * inputArray[inputArray.Length - 1 - i];
                    }
                    return filtered;
                });
        }

       
    }
}
