namespace seminar8ex1
{
    public partial class Form1 : Form
    {
        Pen pen = new Pen(Color.Navy);
        private List<Point> p = new List<Point>();
        int n;
        const int raza = 3;
        int currentClicks;
        double s = 0;

        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint);

        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentClicks < n)
            {
                p.Add(new Point(e.X, e.Y));
                currentClicks++;
                // Solicitarea redesenãrii formularului
                this.Invalidate();
            }
            if (currentClicks == n)
            {
                this.Invalidate();
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            pen = new Pen(Color.Red);
            if (p.Count > 1)
            {
                e.Graphics.DrawLines(Pens.Black, p.ToArray());
                if (currentClicks == n)
                {
                    if (p.Count > 2)
                    {
                        e.Graphics.DrawLine(Pens.Black, p[p.Count - 1], p[0]);

                        while (n > 3)
                        {
                            bool removed = false;
                            for (int c = 0; c < n; c++)
                            {
                                int next = (c + 1) % n;
                                int nextNext = (c + 2) % n;
                                if (este_varf_convex(c) && intoarcere_spre_dreapta(c, next, nextNext) && se_afla_in_interiorul_poligonului(c, nextNext))
                                {
                                    s = s + Sarrus(p[c], p[next], p[nextNext]);
                                    g.DrawLine(pen, p[c], p[nextNext]);
                                    p.RemoveAt(next);
                                    n--;
                                    removed = true;
                                    break; // Break the loop after modifying the list
                                }
                            }
                            if (!removed) break; // Exit the loop if no vertices were removed in this pass
                        }
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = (s / 2).ToString();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int parsedValue) && parsedValue > 0)
            {
                n = parsedValue;
                p.Clear();
                currentClicks = 0;
                this.Invalidate();
            }

        }
        private bool este_varf_convex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_dreapta(p_ant, p, p_urm);
        }

        private bool se_afla_in_interiorul_poligonului(int pi, int pj)
        {
            int pi_ant = (pi > 0) ? pi - 1 : n - 1;
            int pi_urm = (pi < n - 1) ? pi + 1 : 0;
            if ((este_varf_convex(pi) && intoarcere_spre_stanga(pi, pj, pi_urm) && intoarcere_spre_stanga(pi, pi_ant, pj)) ||
            (este_varf_reflex(pi) && !(intoarcere_spre_dreapta(pi, pj, pi_urm) && intoarcere_spre_dreapta(pi, pi_ant, pj))))
                return true;
            return false;
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
        private bool este_varf_reflex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_stanga(p_ant, p, p_urm);
        }


        private double Sarrus(PointF p1, PointF p2, PointF p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
        }

       
    }
}
