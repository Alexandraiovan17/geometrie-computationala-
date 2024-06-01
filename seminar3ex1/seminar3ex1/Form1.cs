using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seminar3ex1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p1 = new Pen(Color.Pink, 3);
            Pen p2 = new Pen(Color.Blue, 2);
            Pen p3 = new Pen(Color.Purple, 3);

            List<Segment> segments = new List<Segment>
            {
                new Segment(new PointF(50, 150), new PointF(200, 100)),
                new Segment(new PointF(100, 50), new PointF(150, 200)),
                // Adăugați alte segmente aici
            };

            // Desenarea segmentelor
            foreach (var segment in segments)
            {
                g.DrawLine(p1, segment.Start, segment.End);
            }

            var intersections = FindIntersections(segments);

            // Desenarea intersecțiilor
            foreach (var intersection in intersections)
            {
                g.DrawEllipse(p3, intersection.X - 3, intersection.Y - 3, 6, 6);
            }

        }
        private List<PointF> FindIntersections(List<Segment> segments)
        {
            List<Event> events = new List<Event>();
            foreach (var segment in segments)
            {
                events.Add(new Event(segment.Start, segment, true));
                events.Add(new Event(segment.End, segment, false));
            }

            events.Sort();

            SortedList<float, Segment> activeSegments = new SortedList<float, Segment>();
            List<PointF> intersections = new List<PointF>();

            foreach (var ev in events)
            {
                if (ev.IsStart)
                {
                    activeSegments.Add(ev.Point.Y, ev.Segment);
                    var adjacent = GetAdjacentSegments(activeSegments, ev.Point.Y).ToList();
                    foreach (var seg in adjacent)
                    {
                        if (DoIntersect(ev.Segment, seg))
                        {
                            intersections.Add(GetIntersectionPoint(ev.Segment, seg));
                        }
                    }
                }
                else
                {
                    activeSegments.Remove(ev.Point.Y);
                }
            }

            return intersections;
        }

        private IEnumerable<Segment> GetAdjacentSegments(SortedList<float, Segment> activeSegments, float y)
        {
            int index = activeSegments.IndexOfKey(y);
            if (index > 0) yield return activeSegments.Values[index - 1];
            if (index < activeSegments.Count - 1) yield return activeSegments.Values[index + 1];
        }

        private bool DoIntersect(Segment s1, Segment s2)
        {
            float o1 = Orientation(s1.Start, s1.End, s2.Start);
            float o2 = Orientation(s1.Start, s1.End, s2.End);
            float o3 = Orientation(s2.Start, s2.End, s1.Start);
            float o4 = Orientation(s2.Start, s2.End, s1.End);

            if (o1 != o2 && o3 != o4)
                return true;

            return (o1 == 0 && OnSegment(s1.Start, s2.Start, s1.End)) ||
                   (o2 == 0 && OnSegment(s1.Start, s2.End, s1.End)) ||
                   (o3 == 0 && OnSegment(s2.Start, s1.Start, s2.End)) ||
                   (o4 == 0 && OnSegment(s2.Start, s1.End, s2.End));
        }

        private float Orientation(PointF p, PointF q, PointF r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) return 0;
            return (val > 0) ? 1 : 2;
        }

        private bool OnSegment(PointF p, PointF q, PointF r)
        {
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                   q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }

        private PointF GetIntersectionPoint(Segment s1, Segment s2)
        {
            float A1 = s1.End.Y - s1.Start.Y;
            float B1 = s1.Start.X - s1.End.X;
            float C1 = A1 * s1.Start.X + B1 * s1.Start.Y;

            float A2 = s2.End.Y - s2.Start.Y;
            float B2 = s2.Start.X - s2.End.X;
            float C2 = A2 * s2.Start.X + B2 * s2.Start.Y;

            float det = A1 * B2 - A2 * B1;
            if (det == 0)
            {
                return new PointF(float.MaxValue, float.MaxValue); // Liniile sunt paralele
            }
            else
            {
                float x = (B2 * C1 - B1 * C2) / det;
                float y = (A1 * C2 - A2 * C1) / det;
                return new PointF(x, y);
            }
        }

        // Clase suplimentare
        public class Segment
        {
            public PointF Start { get; set; }
            public PointF End { get; set; }

            public Segment(PointF start, PointF end)
            {
                Start = start;
                End = end;
            }
        }

        public class Event : IComparable<Event>
        {
            public PointF Point { get; set; }
            public Segment Segment { get; set; }
            public bool IsStart { get; set; }

            public Event(PointF point, Segment segment, bool isStart)
            {
                Point = point;
                Segment = segment;
                IsStart = isStart;
            }

            public int CompareTo(Event other)
            {
                int compareX = Point.X.CompareTo(other.Point.X);
                if (compareX == 0)
                {
                    return IsStart.CompareTo(other.IsStart) * -1; // Evenimentele de început vin înaintea celor de sfârșit
                }
                return compareX;
            }
        }
    }

}

