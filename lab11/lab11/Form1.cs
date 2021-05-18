using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab11
{
    public partial class Form1 : Form
    {

        int S = 5;
        public Form1()
        {
            InitializeComponent();
        }


        public double[] input_check()
        {
            double[] probs = new double[S];
            try
            {
                probs[0] = Convert.ToDouble(textBox1.Text);
                probs[1] = Convert.ToDouble(textBox2.Text);
                probs[2] = Convert.ToDouble(textBox3.Text);
                probs[3] = Convert.ToDouble(textBox4.Text);

                double sum = probs.Sum();
                probs[4] = 1 - sum;
                textBox5.Text = probs[4].ToString("N3");
            }
            catch
            {
                probs[0] = 0.2;
                probs[1] = 0.2;
                probs[2] = 0.2;
                probs[3] = 0.2;
                probs[4] = 0.2;
                textBox1.Text = probs[0].ToString("N3");
                textBox2.Text = probs[1].ToString("N3");
                textBox3.Text = probs[2].ToString("N3");
                textBox4.Text = probs[3].ToString("N3");
                textBox5.Text = probs[4].ToString("N3");
                label6.Text = "can't convert to double";

            }
            if (probs[4] < 0) label6.Text = "invalid probabilities";

            return probs;
        }
        public double calc_E(double[] freq)
        {
            double E = 0;
            for (int i = 0; i<S; i++)
            {
                E += (i + 1) * freq[i];
            }
            return E;
        }
        public double calc_D(double[] freq)
        {
            double E = calc_E(freq);
            double D = 0;
            for (int i = 0; i < S; i++)
            {
                D += freq[i] * ((i + 1) - E) * ((i + 1) - E);
            }
            return D;
        }
        public double calc_Chi(double[] stat, int n)    
        {
            double[] probs = input_check();
            double Chi = 0;
            for (int i = 0; i < S; i++)
            {
                if (stat[i]!=0) Chi += (stat[i] * stat[i])/(n*probs[i]);
            }
            return (Chi-n);
        }
        public double[] statistick_count(double[] probs, int number_experiment)
        {
            double[] Statistic = new double[S];
            Random rnd = new Random();
            for (int i = 0; i < number_experiment; i++)
            {
                double A = rnd.NextDouble();
                int event_id = -1;
                do
                {
                    event_id++;
                    A -= probs[event_id];
                } while (A > 0);
                Statistic[event_id]++;
            }
            return Statistic;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            double[] probs = input_check();
            int experiment_number;
            try
            {
                experiment_number = Convert.ToInt32(textBox6.Text);
            }
            catch
            {
                experiment_number = 5;
                textBox6.Text = "5";
            }

            double[] freq = statistick_count(probs, experiment_number);
            double[] stat = new double[S];
            Array.Copy(freq, stat, S);
            for (int i = 0; i < S; i++) freq[i] /= experiment_number;


            label7.Text = Convert.ToString(freq[0]);
            label8.Text = Convert.ToString(freq[1]);
            label9.Text = Convert.ToString(freq[2]);
            label10.Text = Convert.ToString(freq[3]);
            label11.Text = Convert.ToString(freq[4]);

            double E = calc_E(freq);
            double D = calc_D(freq);
            label16.Text = Convert.ToString(E);
            label17.Text = Convert.ToString(D);

         
            double Chi = calc_Chi(stat, experiment_number);
            label19.Text = Chi.ToString("N5");

            chart1.Series.Clear();
            Series series = new Series();
            series.ChartType = SeriesChartType.Column;
            series.Name = "series1";
            chart1.Series.Add(series);
            for (int i = 0; i < S; i++)
            {
                chart1.Series["series1"].Points.AddXY(i + 1, freq[i]);
            };

            
            label21.Text = "<";
            if (Chi >= 9.49) label21.Text = ">";
        }
    }
}
