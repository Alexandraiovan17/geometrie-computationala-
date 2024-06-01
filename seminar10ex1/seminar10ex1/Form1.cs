namespace seminar10ex1
{
        public partial class Form1 : Form
        {
            private Graphics g;
            private List<Point> points = new List<Point>();
            private int contor = 1;
            private bool drawingMode = false;

            public Form1()
            {
                InitializeComponent();
                panel1.Paint += new PaintEventHandler(panel1_Paint);
                panel1.MouseUp += new MouseEventHandler(panel1_MouseUp);
            }

            private void panel1_Paint(object sender, PaintEventArgs e)
            {
                g = e.Graphics;
            }

            private void panel1_MouseUp(object sender, MouseEventArgs e)
            {
            Graphics g = panel1.CreateGraphics();
            if (drawingMode)
            {
                Pen pen = new Pen(Color.Black, 3);
                Point aux = new Point(e.X, e.Y);
                Pen linie = new Pen(Color.DarkViolet, 2);
                g.DrawString(contor.ToString(), new Font(FontFamily.GenericSansSerif, 10), new SolidBrush(Color.Black), aux.X - 20, aux.Y - 20);
                contor++;
                g.DrawEllipse(pen, aux.X - 2, aux.Y - 2, 4, 4);
                if (points.Count != 0)
                {
                    g.DrawLine(linie, aux, points[points.Count - 1]);
                }
                points.Add(aux);
            }
        }

            private void button1_Click(object sender, EventArgs e)
            {
                if (!drawingMode)
                {
                    drawingMode = true;
                    button1.Enabled = false;
                    button2.Enabled = true;
                }
            }

            private void button2_Click(object sender, EventArgs e)
            {
            if (drawingMode)
            {
                Graphics g = panel1.CreateGraphics();
                Pen linie = new Pen(Color.DarkViolet, 2);
                g.DrawLine(linie, points[0], points[points.Count - 1]);
                button2.Enabled = false;
                drawingMode = false;
                button3.Enabled = true;
            }
        }

            private void button3_Click(object sender, EventArgs e)
            {
                Pen penPart = new Pen(Color.DarkViolet, 2);
                float[] dashValues = { 3, 2, 3, 2 };
                penPart.DashPattern = dashValues;

                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (este_varf_reflex(points, i))
                    {
                        if (i == 0)
                        {
                            if (este_sub(i, (points.Count - 1)) && este_sub(i, (i + 1)))
                            {
                                int index = UnVarfDeasupraVarfului(i);
                                g.DrawLine(penPart, points[i], points[index]);
                            }
                            if (este_deasupra(i, (points.Count - 1)) && este_deasupra(i, (i + 1)))
                            {
                                int index = UnVarfSubVarful(i);
                                g.DrawLine(penPart, points[i], points[index]);
                            }
                        }
                        else if (i == points.Count - 1)
                        {
                            if (este_sub(i, (i - 1)) && este_sub(i, 0))
                            {
                                int index = UnVarfDeasupraVarfului(i);
                                g.DrawLine(penPart, points[i], points[index]);
                            }
                            if (este_deasupra(i, (i - 1)) && este_deasupra(i, 0))
                            {
                                int index = UnVarfSubVarful(i);
                                g.DrawLine(penPart, points[i], points[index]);
                            }
                        }
                        else
                        {
                            if (este_sub(i, (i - 1)) && este_sub(i, (i + 1)))
                            {
                                int index = UnVarfDeasupraVarfului(i);
                                g.DrawLine(penPart, points[i], points[index]);
                            }
                            if (este_deasupra(i, (i - 1)) && este_deasupra(i, (i + 1)))
                            {
                                int index = UnVarfSubVarful(i);
                                g.DrawLine(penPart, points[i], points[index]);
                            }
                        }
                    }
                }
            }

            private int UnVarfDeasupraVarfului(int i)
            {
                int minIndex = -1;
                for (int index = 0; index < points.Count; index++)
                {
                    if (este_deasupra(i, index) && IsDiagonal(points, i, index))
                    {
                        if (minIndex == -1)
                        {
                            minIndex = index;
                        }
                        else
                        {
                            if (points[index].Y > points[minIndex].Y)
                            {
                                minIndex = index;
                            }
                        }
                    }
                }
                return minIndex;
            }

            private int UnVarfSubVarful(int i)
            {
                int maxIndex = -1;
                for (int index = 0; index < points.Count; index++)
                {
                    if (este_sub(i, index) && IsDiagonal(points, i, index))
                    {
                        if (maxIndex == -1)
                        {
                            maxIndex = index;
                        }
                        else
                        {
                            if (points[index].Y < points[maxIndex].Y)
                            {
                                maxIndex = index;
                            }
                        }
                    }
                }
                return maxIndex;
            }

            private bool este_sub(int i, int j)
            {
                if (points[j].Y > points[i].Y || (points[j].Y == points[i].Y && points[j].X < points[i].X))
                {
                    return true;
                }
                return false;
            }

            private bool este_deasupra(int i, int j)
            {
                if (points[j].Y < points[i].Y || (points[j].Y == points[i].Y && points[j].X < points[i].X))
                {
                    return true;
                }
                return false;
            }

            #region FunctiiDiagonala

            private bool IsDiagonal(List<Point> puncte, int i, int j)
            {
                bool intersectie = false;
                for (int k = 0; k < puncte.Count - 1; k++)
                {
                    if (i != k && i != (k + 1) && j != k && j != (k + 1) && se_intersecteaza(puncte[i], puncte[j], puncte[k], puncte[k + 1]))
                    {
                        intersectie = true;
                        break;
                    }
                }
                if (i != puncte.Count - 1 && i != 0 && j != puncte.Count - 1 && j != 0 && se_intersecteaza(puncte[i], puncte[j], puncte[puncte.Count - 1], puncte[0]))
                {
                    intersectie = true;
                }
                if (!intersectie && se_afla_in_interiorul_poligonului(puncte, i, j))
                {
                    return true;
                }
                return false;
            }

            private int ValoareDeterminant(Point a, Point b, Point c)
            {
                return a.X * b.Y + b.X * c.Y + c.X * a.Y - c.X * b.Y - a.X * c.Y - b.X * a.Y;
            }

            private bool se_afla_in_interiorul_poligonului(List<Point> puncte, int pi, int pj)
            {
                int pi_ant = (pi > 0) ? pi - 1 : puncte.Count - 1;
                int pi_urm = (pi < puncte.Count - 1) ? pi + 1 : 0;
                if ((este_varf_convex(puncte, pi) && intoarcere_spre_stanga(puncte, pi, pj, pi_urm) && intoarcere_spre_stanga(puncte, pi, pi_ant, pj)) || (este_varf_reflex(puncte, pi) && !(intoarcere_spre_dreapta(puncte, pi, pj, pi_urm) && intoarcere_spre_dreapta(puncte, pi, pi_ant, pj))))
                {
                    return true;
                }
                return false;
            }

            private bool intoarcere_spre_dreapta(List<Point> puncte, int p1, int p2, int p3)
            {
                if (ValoareDeterminant(puncte[p1], puncte[p2], puncte[p3]) > 0)
                {
                    return true;
                }
                return false;
            }

            private bool intoarcere_spre_stanga(List<Point> puncte, int p1, int p2, int p3)
            {
                if (ValoareDeterminant(puncte[p1], puncte[p2], puncte[p3]) < 0)
                {
                    return true;
                }
                return false;
            }

            private bool este_varf_reflex(List<Point> puncte, int p)
            {
                int p_ant = (p > 0) ? p - 1 : puncte.Count - 1;
                int p_urm = (p < puncte.Count - 1) ? p + 1 : 0;
                return intoarcere_spre_stanga(puncte, p_ant, p, p_urm);
            }

            private bool este_varf_convex(List<Point> puncte, int p)
            {
                int p_ant = (p > 0) ? p - 1 : puncte.Count - 1;
                int p_urm = (p < puncte.Count - 1) ? p + 1 : 0;
                return intoarcere_spre_dreapta(puncte, p_ant, p, p_urm);
            }

            private bool se_intersecteaza(Point s1, Point s2, Point p1, Point p2)
            {
                if (ValoareDeterminant(p2, p1, s1) * ValoareDeterminant(p2, p1, s2) <= 0 && ValoareDeterminant(s2, s1, p1) * ValoareDeterminant(s2, s1, p2) <= 0)
                {
                    return true;
                }
                return false;
            }

            #endregion
        }
    }


