using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpatialAnalysis.Core
{
    [Serializable]
    public class Point
    {
        private double x;
        private double y;
        public double X
        {
            set
            {
                x = value;
            }
            get 
            { 
                return x;
            }
        }
        public double Y 
        {
            set
            {
                y = value;
            }
            get
            {
                return y;
            }
        }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point()
        {
            
        }

        public double DistanceWith(Point p)
        {
            double deltaX = this.X - p.X;
            double deltaY = this.Y - p.Y;
            double square = deltaX * deltaX + deltaY * deltaY;
            double distance = System.Math.Sqrt(square);
            if (distance >= 0)
                return distance;
            else
                return 0.0;
        }

        public double DistanceWith(SimpleLine line)
        {
            double a2 = line.A * line.A;
            double b2 = line.B * line.B;
            double bottom = System.Math.Sqrt(a2 + b2);
            double top = line.A * this.X + line.B * this.Y + line.C;
            double distance = System.Math.Abs(top / bottom);
            return distance;
        }
    }
}
