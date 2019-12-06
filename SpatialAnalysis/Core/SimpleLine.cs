using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialAnalysis.Core;

namespace SpatialAnalysis.Core
{
    [Serializable]
    public class SimpleLine
    {
        private Point startPoint;
        private Point endPoint;
        private double k;
        private double bias;
        private double a;
        private double b;
        private double c;

        public Point StartPoint
        {
            set
            {
                startPoint = value;
            }
            get
            {
                return startPoint;
            }
        }
        public Point EndPoint
        {
            set
            {
                endPoint = value;
            }
            get
            {
                return endPoint;
            }
        }
        // 斜率K
        public double K
        {
            set
            {
                k = value;
            }
            get
            {
                return k;
            }
        }
        public double Bias
        {
            set
            {
                bias = value;
            }
            get
            {
                return bias;
            }
        }
        // 直线的标准方程参数
        public double A
        {
            set 
            {
                a = value;
            }
            get
            {
                return a;
            }
        }
        public double B
        {
            set
            {
                b = value;
            }
            get
            {
                return b;
            }
        }
        public double C
        {
            set
            {
                c = value;
            }
            get
            {
                return c;
            }
        }

        public SimpleLine(Point StartPoint, Point EndPoint)
        {
            if (System.Math.Abs(StartPoint.X - EndPoint.X) >= 0.000000000001 || System.Math.Abs(StartPoint.Y - EndPoint.Y) >= 0.000000000001)
            {
                this.StartPoint = StartPoint;
                this.EndPoint = EndPoint;
                // 计算直线的标准方程参数
                if (System.Math.Abs(StartPoint.X - EndPoint.X) < 0.000000000001)
                {
                    K = Constant.INF;
                    Bias = Constant.INF;
                    A = 1;
                    B = 0;
                    C = -StartPoint.X;
                }
                else if (System.Math.Abs(StartPoint.Y - EndPoint.Y) < 0.000000000001)
                {
                    K = 0;
                    Bias = StartPoint.Y;
                    A = 0;
                    B = 1;
                    C = -StartPoint.Y;
                }
                else
                {
                    K = (EndPoint.Y - StartPoint.Y) / (EndPoint.X - StartPoint.X);
                    Bias = K * (-StartPoint.X) + StartPoint.Y;
                    A = K;
                    B = -1;
                    C = StartPoint.Y - K * StartPoint.X;
                }
            }
        }

        public SimpleLine Midperpendicular
        {
            get
            {
                if (this.K == Constant.INF)
                {
                    return new SimpleLine(
                        new Point(- Constant.INF, (this.StartPoint.Y + this.EndPoint.Y) / 2),
                        new Point(Constant.INF, (this.StartPoint.Y + this.EndPoint.Y) / 2));
                }
                else if (Equals(this.K, 0.0))
                {
                    return new SimpleLine(
                        new Point((this.StartPoint.X + this.EndPoint.X) / 2, - Constant.INF),
                        new Point((this.StartPoint.X + this.EndPoint.X) / 2, Constant.INF));
                }
                else
                {
                    double k2 = -1 / this.K;
                    double y1 = k2 * (-100000 - ((this.StartPoint.X + this.EndPoint.X) / 2)) + (this.StartPoint.Y + this.EndPoint.Y) / 2;
                    double y2 = k2 * (100000 - ((this.StartPoint.X + this.EndPoint.X) / 2)) + (this.StartPoint.Y + this.EndPoint.Y) / 2;
                    return new SimpleLine(
                        new Point(-100000, y1),
                        new Point(100000, y2));
                }
            }
        }
    }
}
