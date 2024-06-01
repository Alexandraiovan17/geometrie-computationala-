using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seminar2ex1
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
            Pen p1 = new Pen(Color.Red, 3); Pen p2 = new Pen(Color.Green, 9); Pen p3 = new Pen(Color.Purple, 2);
            Random r = new Random();
            float raza = 1;
            int n = r.Next(65, 100);
            PointF[] num = new PointF[n];
            int d = 200;
            for (int i = 0; i < n; i += 1)
            {
                num[i].X = r.Next(10, panel1.Width - 10);
                num[i].Y = r.Next(10, panel1.Height - 10);
                g.DrawEllipse(p1, num[i].X - raza, num[i].Y - raza, raza * 2, raza * 2);
            }

            PointF q = new PointF(300, 100);
            g.DrawEllipse(p2, q.X - raza, q.Y - raza, raza * 2, raza * 2);

            for (int i = 0; i < n; i += 1)
            {
                int dist = (int)Math.Sqrt(Math.Pow((num[i].X - q.X), 2) + Math.Pow((num[i].Y - q.Y), 2));
                if (dist <= d) g.DrawLine(p3, num[i].X, num[i].Y, q.X, q.Y);
            }
        }
    }
}
