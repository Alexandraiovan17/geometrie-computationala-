using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Se dă o mulțime de puncte în plan. Să se determine învelitoarea convexă a acestei mulțimi.
namespace seminar5ex1
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

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p1 = new Pen(Color.DarkBlue, 3), p2 = new Pen(Color.HotPink, 2);
            Random rnd = new Random();
            int num = rnd.Next(30, 50);
            List<PointF> points = new List<PointF>(num);
            float raza = 1;

            for (int i = 0; i < num; i++)
            {
                PointF x = new PointF();
                x.X = rnd.Next(52, this.Width - 52);
                x.Y = rnd.Next(52, this.Height - 52);
                points.Add(x);
                g.DrawEllipse(p1, points[i].X - raza, points[i].Y - raza, raza * 2, raza * 2);
            }

            List<PointF> hull = GrahamScan(points);// este aplicat pentru a găsi învelișul convex al punctelor
            // sortarea punctelor în funcție de unghiul lor față de un punct de referință
            if (hull.Count > 1)
            {
                g.DrawPolygon(p2, hull.ToArray());
            }
        }

        private static int Turn(PointF p, PointF q, PointF r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) return 0; // coliniare
            return (val > 0) ? 1 : -1; // sens trigonometric
        }

        private static float AngleTo(PointF reference, PointF p)
        {
            return (float)Math.Atan2(p.Y - reference.Y, p.X - reference.X);
        }

        public static List<PointF> GrahamScan(List<PointF> points)
        {
            if (points.Count < 3) return new List<PointF>();

            PointF start = points.OrderBy(p => p.Y).ThenBy(p => p.X).First();
            var sortedPoints = points.OrderBy(p => AngleTo(start, p)).ToList();

            Stack<PointF> hull = new Stack<PointF>();
            hull.Push(sortedPoints[0]);
            hull.Push(sortedPoints[1]);
            hull.Push(sortedPoints[2]);

            for (int i = 3; i < sortedPoints.Count; i++)
            {
                PointF top = hull.Pop();
                while (Turn(hull.Peek(), top, sortedPoints[i]) != -1)
                {
                    top = hull.Pop();
                }
                hull.Push(top);
                hull.Push(sortedPoints[i]);
            }

            return hull.ToList();
        }
    }
    }

       
    
