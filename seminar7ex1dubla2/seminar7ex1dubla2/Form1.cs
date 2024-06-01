using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace seminar7ex1dubla2
{
    public partial class Form1 : Form
    {    Graphics g;
        Pen pen = new Pen(Color.Navy); 
        const int raza = 3; // Definim raza punctelor care vor fi desenate
        int n = 0; // Numărul de vârfuri ale poligonului
        List<PointF> p = new List<PointF>(); // Lista punctelor ce formează poligonul
        bool poligon_inchis = false; // Variabilă pentru a ține evidența dacă poligonul este închis sau nu
        PointF primulPunct; // Variabilă pentru a stoca primul punct adăugat
        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_Click(object sender, EventArgs e)
        {
            // Obținem poziția cursorului la momentul click-ului
            PointF newPoint = this.PointToClient(MousePosition);
            // Adăugăm punctul în lista de puncte
            p.Add(newPoint);
            // Desenăm un punct la poziția cursorului
            g.DrawEllipse(pen, newPoint.X, newPoint.Y, raza, raza);
            // Adăugăm numărul de ordine al punctului
            g.DrawString((n + 1).ToString(), new Font(FontFamily.GenericSansSerif, 10),
                new SolidBrush(Color.Black), newPoint.X + raza, newPoint.Y - raza);
            // Dacă există mai mult de un punct, desenăm o linie între punctul curent și anteriorul
            if (n > 0)
                g.DrawLine(pen, p[n - 1], newPoint);
            n++; // Incrementăm numărul de puncte adăugate

            // Dacă acesta este primul punct, îl setăm ca primul punct al poligonului
            if (n == 1)
            {
                primulPunct = newPoint;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Dacă nu avem suficiente puncte pentru a forma un poligon, ieșim din funcție
            if (n < 3)
                return;
            // Desenăm o linie între ultimul punct și primul punct pentru a închide poligonul
            g.DrawLine(pen, p[n - 1], p[0]);
            // Marcam poligonul ca fiind inchis
            poligon_inchis = true;
        }

        // Metoda pentru calculul determinanților pentru a verifica orientarea punctelor
        private double Sarrus(PointF p1, PointF p2, PointF p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
        }

        // Metoda pentru a verifica dacă avem o întoarcere la stânga
        private bool intoarcere_spre_stanga(int p1, int p2, int p3)
        {
            return Sarrus(p[p1], p[p2], p[p3]) < 0;
        }

        // Metoda pentru a verifica dacă avem o întoarcere la dreapta
        private bool intoarcere_spre_dreapta(int p1, int p2, int p3)
        {
            return Sarrus(p[p1], p[p2], p[p3]) > 0;
        }

        // Metoda pentru a verifica dacă un vârf este convex
        private bool este_varf_convex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_dreapta(p_ant, p, p_urm);
        }

        // Metoda pentru a verifica dacă un vârf este reflex
        private bool este_varf_reflex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_stanga(p_ant, p, p_urm);
        }

        // Metoda pentru a verifica dacă două segmente se intersectează
        private bool se_intersecteaza(PointF s1, PointF s2, PointF p1, PointF p2)
        {
            return Sarrus(p2, p1, s1) * Sarrus(p2, p1, s2) <= 0 && Sarrus(s2, s1, p1) * Sarrus(s2, s1, p2) <= 0;
        }

        // Metoda pentru a verifica dacă un punct se află în interiorul poligonului
        private bool se_afla_in_interiorul_poligonului(int pi, int pj)
        {
            int pi_ant = (pi > 0) ? pi - 1 : n - 1;
            int pi_urm = (pi < n - 1) ? pi + 1 : 0;
            if ((este_varf_convex(pi) && intoarcere_spre_stanga(pi, pj, pi_urm) && intoarcere_spre_stanga(pi, pi_ant, pj)) ||
                (este_varf_reflex(pi) && !(intoarcere_spre_dreapta(pi, pj, pi_urm) && intoarcere_spre_dreapta(pi, pi_ant, pj))))
                return true;
            return false;
        }

        // Butonul pentru a desena diagonalele poligonului
        private void button2_Click(object sender, EventArgs e)
        {
            // Dacă avem mai puțin de 3 puncte, ieșim din funcție
            if (n <= 3)
                return;
            // Dacă poligonul nu este închis, închidem poligonul
            if (!poligon_inchis)
                button1_Click(sender, e);

            // Setăm creionul la culoarea neagră și un model de linie alternativ
            pen = new Pen(Color.Black);
            float[] dashValues = { 1, 2, 3, 4 };
            pen.DashPattern = dashValues;

            // Obținem primul punct din lista de puncte
            PointF primulPunct = p[0];

            // Parcurgem toate combinațiile de puncte pentru a găsi diagonalele
            for (int i = 1; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    bool intersectie = false;

                    for (int k = 1; k < n; k++)
                    {
                        int nextK = (k + 1) % n;

                        if ((k != i && nextK != i && k != j && nextK != j) &&
                            se_intersecteaza(p[i], p[j], p[k], p[nextK]))
                        {
                            intersectie = true;
                            break;
                        }
                    }

                    // Dacă nu există intersectie, desenăm diagonalele
                    if (!intersectie)
                    {
                        g.DrawLine(pen, primulPunct, p[i]);
                        g.DrawLine(pen, primulPunct, p[j]);
                        Thread.Sleep(100); // Doar pentru a vedea desenarea liniilor pas cu pas
                    }
                }
            }
        }
        
    }
}