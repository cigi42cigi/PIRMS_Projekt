using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.filters
{
    class FIR_filter
    {
        public double[] Coefficients { get; private set; }

        public FIR_filter(int order, double sampleRate, double cutoffFrequency)
        {
            Coefficients = DesignLowPass(order, sampleRate, cutoffFrequency);
        }

        private double[] DesignLowPass(int N, double Fs, double fc)
        {
            double f = fc / Fs;
            double[] h = new double[N + 1];
            int M = N / 2;

            for (int n = 0; n <= N; n++)
            {
                double x = n - M;
                double sinc = x == 0 ? 2 * Math.PI * f : Math.Sin(2 * Math.PI * f * x) / (x * Math.PI);
                double window = 0.54 - 0.46 * Math.Cos(2 * Math.PI * n / N); 
                h[n] = sinc * window;
            }

            return h;
        }


        public double[] Apply(double[] input)
        {
            int N = Coefficients.Length;
            double[] output = new double[input.Length];

            for (int n = 0; n < input.Length; n++)
            {
                double y = 0.0;
                for (int k = 0; k < N; k++)
                {
                    if (n - k >= 0)
                        y += Coefficients[k] * input[n - k];
                }
                output[n] = y;
            }

            return output;
        }
    }
}
