namespace seminar11ex1
{
    public partial class Form1 : Form
    {
        List<PointF> p;
        Graphics g;
        int n;
        public Form1()
        {
            InitializeComponent();
            p = new List<PointF>();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            float x = e.X;
            float y = e.Y;
            p.Add(new PointF(x, y));
            g = pictureBox1.CreateGraphics();
            g.DrawEllipse(new Pen(Color.Pink, 2), x - 1, y - 1, 2, 2);
            g.DrawString((n + 1).ToString(), new Font(FontFamily.GenericSansSerif, 10),
                new SolidBrush(Color.Blue), p[n].X + 2, p[n].Y - 2);
            if (n > 0)
                g.DrawLine(new Pen(Color.DarkViolet, 2), p[n - 1], p[n]);
            n++;

            g.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g = pictureBox1.CreateGraphics();
            g.DrawLine(new Pen(Color.DarkViolet, 2), p[0], p[p.Count - 1]);

            while (n > 3)
            {
                for (int i = 0; i < n; i++)
                {
                    int p1 = i;
                    int p2 = (i + 1) % n;
                    int p3 = (i + 2) % n;

                    bool intersectie = false;
                    for (int k = 0; k < n - 1; k++)
                        if (i != k && i != (k + 1) && p3 != k && p3 != (k + 1) && se_intersecteaza(p[i], p[p3], p[k], p[k + 1]))
                        {
                            intersectie = true;
                            break;
                        }
                    if (i != n - 1 && i != 0 && p3 != n - 1 && p3 != 0 && se_intersecteaza(p[i], p[p3], p[n - 1], p[0]))
                        intersectie = true;
                    if (!intersectie)
                    {
                        if (se_afla_in_interiorul_poligonului(p1, p3))
                        {
                            n = n - 1;
                            g.DrawLine(new Pen(Color.HotPink, 2), p[p1], p[p3]);
                            Thread.Sleep(1000);
                            if (!este_varf_reflex(p1))
                                g.DrawLine(new Pen(Color.White, 2), p[p1], p[p3]);
                            p.RemoveAt(p2);
                            break;
                        }
                    }

                }
            }

        }
        private bool se_intersecteaza(PointF s1, PointF s2, PointF p1, PointF p2)
        {
            if (Sarrus(p2, p1, s1) * Sarrus(p2, p1, s2) <= 0 && Sarrus(s2, s1, p1) * Sarrus(s2, s1, p2) <= 0)
                return true;
            return false;
        }


        private double Sarrus(PointF p1, PointF p2, PointF p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
        }
        private bool intoarcere_spre_stanga(int p1, int p2, int p3)
        {
            if (Sarrus(p[p1], p[p2], p[p3]) < 0)
                return true;
            return false;
        }
        private bool intoarcere_spre_dreapta(int p1, int p2, int p3)
        {
            if (Sarrus(p[p1], p[p2], p[p3]) > 0)
                return true;
            return false;
        }
        private bool este_varf_convex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_dreapta(p_ant, p, p_urm);
        }
        private bool este_varf_reflex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_stanga(p_ant, p, p_urm);
        }

        //verifica daca segmentul p_i p_j se afla in interiorul poligonului
        private bool se_afla_in_interiorul_poligonului(int pi, int pj)
        {
            int pi_ant = (pi > 0) ? pi - 1 : n - 1;
            int pi_urm = (pi < n - 1) ? pi + 1 : 0;
            if ((este_varf_convex(pi) && intoarcere_spre_stanga(pi, pj, pi_urm) && intoarcere_spre_stanga(pi, pi_ant, pj)) ||
            (este_varf_reflex(pi) && !(intoarcere_spre_dreapta(pi, pj, pi_urm) && intoarcere_spre_dreapta(pi, pi_ant, pj))))
                return true;
            return false;
        }

        private void white(object sender, PaintEventArgs e)
        {

        }
    }
}
