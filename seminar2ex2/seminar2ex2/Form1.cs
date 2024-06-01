using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seminar2ex2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p1 = new Pen(Color.Green, 3), p2 = new Pen(Color.Blue, 2);
            Random random = new Random();
            int num = random.Next(3, 100);
            PointF[] points = new PointF[num];
            float raza = 1;

            for (int i = 0; i < num; i++)
            {
                points[i].X = random.Next(10, panel1.Width - 10);
                points[i].Y = random.Next(10, panel1.Height - 10);
                g.DrawEllipse(p1, points[i].X - raza, points[i].Y - raza, raza * 2, raza * 2);
            }

            float ariam = float.MaxValue;
            int pct1 = 0, pct2 = 0, pct3 = 0;

            for (int i = 0; i < num - 2; i++)
                for (int j = 0; j < num - 1; j++)
                    for (int k = 0; k < num; k++)
                    {
                        if (i != j && j != k)
                        {
                            float aria = DeterminantGrad3(points[i].X, points[i].Y, 1, points[j].X, points[j].Y, 1, points[k].X, points[k].Y, 1);

                            if (aria <= ariam && aria >= 1500)
                            {
                                ariam = aria;
                                pct1 = i;
                                pct2 = j;
                                pct3 = k;
                            }
                        }
                    }
            g.DrawLine(p2, points[pct1].X, points[pct1].Y, points[pct2].X, points[pct2].Y);
            g.DrawLine(p2, points[pct1].X, points[pct1].Y, points[pct3].X, points[pct3].Y);
            g.DrawLine(p2, points[pct2].X, points[pct2].Y, points[pct3].X, points[pct3].Y);
        }
        float DeterminantGrad3(float i1, float i2, float i3, float j1, float j2, float j3, float k1, float k2, float k3)
        {
            float rezultat = 0;
            rezultat = (float)((Math.Abs((i1 * j2 * k3) + (j1 * k2 * i3) + (i2 * j3 * k1) - (i3 * j2 * k1) - (j3 * k2 * i1) - (i2 * j1 * k3))) / 2);
            return rezultat;
        }
    }
    }

