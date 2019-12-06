using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Core
{
    [Serializable]
    public class Polyline : Geometry
    {
        double MaxX;
        double MinX;
        double MaxY;
        double MinY;
        public Point[] Points;
        public SimpleLine[] simpleLines;

        public Polyline(Point[] points)
        {
            this.Points = points;
            simpleLines = new SimpleLine[points.Length - 1];
            for (int i = 0; i < simpleLines.Length; i++)
            {
                simpleLines[i] = new SimpleLine(points[i], points[i + 1]);
            }
        }
    }
}
