using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpatialAnalysis.Core
{
    [Serializable]
    public class Polygon : Geometry
    {
        double MaxX;
        double MinX;
        double MaxY;
        double MinY;
        public Point[] Points;
        public SimpleLine[] simpleLines;

        public Polygon(Point[] points)
        {
            this.Points = points;
            simpleLines = new SimpleLine[points.Length];
            for (int i = 0; i < simpleLines.Length; i++)
            {
                if (i == simpleLines.Length - 1)
                    simpleLines[i] = new SimpleLine(points[i], points[0]);
                else
                    simpleLines[i] = new SimpleLine(points[i], points[i + 1]);
            }
        }

        public Polygon(SimpleLine[] Lines)
        {
            this.simpleLines = Lines;
        }
    }
}
