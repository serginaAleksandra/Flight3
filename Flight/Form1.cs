using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flight
{
    public partial class Form1 : Form
    {
        const decimal dt = 0.1M;
        private Model model;
        public Form1()
        {
            InitializeComponent();
            model = new Model();
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 100;
            chart1.ChartAreas[0].AxisX.Interval = 5;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart1.ChartAreas[0].AxisY.Interval = 5;

            if (!timer1.Enabled)
            {
                chart1.Series[0].Points.Clear();
                model.edValue(
                    edHeight.Value,
                    edSpeed.Value,
                    edAngle.Value,
                    edSquare.Value,
                    edWeight.Value
                );
                chart1.Series[0].Points.AddXY(this.model.getX(), this.model.getY());
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {   
            model.setT(model.getT() + dt);
            model.position();
            chart1.Series[0].Points.AddXY(model.getX(), model.getY());
            if (model.getY() <= 0)
            {
                timer1.Stop();
            }
        }
    }

    class Model
    {

        const double dt = 0.1;
        const double g = 9.81;
        const double C = 0.15;
        const double rho = 1.29;

        double a;
        double v0;
        double y0;
        double S;
        double m;
        double k;

        double t;
        double x;
        double y;
        double vx;
        double vy;

        public Model() { }
        public void edValue(
           decimal edHeight,
           decimal edSpeed,
           decimal edAngle,
           decimal edSize,
           decimal edWeight)
       {
            a = (double)edAngle;
            v0 = (double)edSpeed;
            y0 = (double)edHeight;
            m = (double)edWeight;
            S = (double)edSize;
            k = 0.5 * C * S * rho / m;

            vx = v0 * Math.Cos(a * Math.PI / 180);
            vy = v0 * Math.Sin(a * Math.PI / 180);

            t = 0;
            x = 0;
            y = y0;
       }

        public void position()
        {
            t += dt;

            vx = vx - (k * vx * Math.Sqrt((vx * vx) + (vy * vy)) * dt);
            vy = vy - ((g + (k * vy * Math.Sqrt((vx * vx) + (vy * vy)))) * dt);

            x = x + (vx * dt);
            y = y + (vy * dt);
        }

        public decimal getX()
        {
            return (decimal)this.x;
        }

        public decimal getY()
        {
            return (decimal)this.y; 
        }

        public decimal getT()
        {
            return (decimal)this.t;
        }

        public void setT(decimal t)
        {
            this.t = (double)t;
        }
    } 
}

