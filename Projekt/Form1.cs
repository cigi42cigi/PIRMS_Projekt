using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;


namespace Projekt
{
    public partial class Form1: Form
    {
        private object chart1;

        public Form1()
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            private void NastavGraf()
            {
                // Vyčisti předchozí série
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();
                chart1.Titles.Clear();

                // Vytvoření oblasti grafu
                ChartArea area = new ChartArea("MainArea");
                area.BackColor = Color.White;
                area.AxisX.Title = "Čas";
                area.AxisY.Title = "Hodnota";
                area.AxisX.MajorGrid.LineColor = Color.LightGray;
                area.AxisY.MajorGrid.LineColor = Color.LightGray;
                area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9);
                area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9);
                chart1.ChartAreas.Add(area);

                // Vytvoření série
                serie.ChartType = SeriesChartType.Spline; // Spline = hladká křivka
                serie.Color = Color.MediumSeaGreen;
                serie.BorderWidth = 3;
                serie.MarkerStyle = MarkerStyle.Circle;
                serie.MarkerSize = 5;
                serie.MarkerColor = Color.DarkGreen;


                // Nastavení pozadí a rámečku
                chart1.BackColor = Color.WhiteSmoke;
                chart1.BorderlineColor = Color.Gray;
                chart1.BorderlineDashStyle = ChartDashStyle.Solid;
                chart1.BorderlineWidth = 1;

                // Titulek
                chart1.Titles.Add("Live Data");
                chart1.Titles[0].Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
        }
    }
}
