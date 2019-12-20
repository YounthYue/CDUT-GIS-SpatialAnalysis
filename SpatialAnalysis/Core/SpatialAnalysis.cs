using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Core
{
    public class SpatialAnalysis
    {
        // 判断双精度的浮点数是否相等
        public static bool IsEqual(double a, double b)
        {
            if (System.Math.Abs(a - b) < 0.00000000000001)
                return true;
            return false;
        }

        // 求两线段相交部分
        public static Core.Result.IntersectOfLines TwoSimpleLineOfIntersectPoint(SimpleLine line1, SimpleLine line2)
        {
            Core.Result.IntersectOfLines result = new Core.Result.IntersectOfLines();
            if (line1 != null && line2 != null)
            {
                if (IsEqual(line1.K,line2.K))
                {
                    if (IsEqual(line1.Bias, line2.Bias))
                    {
                        double alpha1 = PositionOfPointOnSimpleLine(line1, line2.StartPoint);
                        double alpha2 = PositionOfPointOnSimpleLine(line1, line2.EndPoint);
                        if (alpha1 <= 1 && alpha1 >= 0)
                        {
                            if (alpha2 <= 1 && alpha2 >= 0)
                                result.intersectSimpleLine = new SimpleLine(line2.StartPoint, line2.EndPoint);
                            else if (alpha2 > 1)
                                result.intersectSimpleLine = new SimpleLine(line2.StartPoint, line1.EndPoint);
                            else
                                result.intersectSimpleLine = new SimpleLine(line2.StartPoint, line1.StartPoint);
                        }
                        else if (alpha1 > 1)
                        {
                            if (alpha2 <= 1 && alpha2 >= 0)
                                result.intersectSimpleLine = new SimpleLine(line1.EndPoint, line2.EndPoint);
                            else if (alpha2 < 0)
                                result.intersectSimpleLine = new SimpleLine(line1.StartPoint, line1.EndPoint);
                        }
                        else
                        {
                            if (alpha2 <= 1 && alpha2 >= 0)
                                result.intersectSimpleLine = new SimpleLine(line1.StartPoint, line2.EndPoint);
                            else if (alpha2 > 1)
                                result.intersectSimpleLine = new SimpleLine(line1.StartPoint, line1.EndPoint);
                        }
                        return result;
                    }
                }
                else
                {
                    Core.Point point = new Point(
                        -(line1.C * line2.B - line1.B * line2.C) / (line1.A * line2.B - line1.B * line2.A),
                        -(line1.A * line2.C - line1.C * line2.A) / (line1.A * line2.B - line1.B * line2.A)
                        );
                    double alpha1 = PositionOfPointOnSimpleLine(line1, point);
                    double alpha2 = PositionOfPointOnSimpleLine(line2, point);
                    if (alpha1 >= 0 && alpha1 <= 1 && alpha2 >= 0 && alpha2 <= 1)
                    {
                        result.intersectPoint = point;
                    }
                    return result;
                }
            }
            return result;
        }

        // 求两直线交点
        public static Core.Point TwoLineOfIntersectPoint(SimpleLine line1, SimpleLine line2)
        {
            if (line1 != null && line2 != null)
            {
                if (IsEqual(line1.K, line2.K))
                    return null;
                else
                {
                    Core.Point point = new Point(
                        -(line1.C * line2.B - line1.B * line2.C) / (line1.A * line2.B - line1.B * line2.A),
                        -(line1.A * line2.C - line1.C * line2.A) / (line1.A * line2.B - line1.B * line2.A)
                        );
                    return point;
                }
            }
            else
                return null;
        }

        // 求点在线段上的位置，用直线的参数方程表示，值在(0,1)
        // -1表示不在线段上
        public static double PositionOfPointOnSimpleLine(SimpleLine line, Point point)
        {
            if (line != null && point != null)
            {
                if (IsEqual(
                    (point.X- line.StartPoint.X)/(line.EndPoint.X - line.StartPoint.X), 
                    (point.Y- line.StartPoint.Y)/(line.EndPoint.Y - line.StartPoint.Y)
                    ))
                    return (point.X - line.StartPoint.X) / (line.EndPoint.X - line.StartPoint.X);
            }
            return -1;
        }

        // 点与线的关系
        public static RelPointAndLine RelationshipOfPointAndLine(SimpleLine line, Point point)
        {
            double M = point.X * (line.StartPoint.Y - line.EndPoint.Y)
                - point.Y * (line.StartPoint.X - line.EndPoint.X)
                + line.StartPoint.X * line.EndPoint.Y - line.EndPoint.X * line.StartPoint.Y;
            if (IsEqual(M, 0))
                return RelPointAndLine.LineOn;
            else if(M < 0)
                return RelPointAndLine.LineRight;
            else
                return RelPointAndLine.LineLeft;
        }

        // 射线算法
        public static bool IsPolygonContainPoint(Polygon polygon, Point point)
        {
            bool b = IsPolylineOfPolygonContainPoint(polygon, point);
            if (b)
            {
                // 如果点在多边形的边上，返回false
                return !b;
            }
            SimpleLine ray = new SimpleLine(point, new Point(point.Y, Constant.INF));
            int sum = 0;
            for (int i = 0; i < polygon.simpleLines.Length; i++)
            {
                Core.Result.IntersectOfLines intersect = TwoSimpleLineOfIntersectPoint(polygon.simpleLines[i], ray);
                if (intersect.intersectPoint != null)
                {
                    double alpha = PositionOfPointOnSimpleLine(polygon.simpleLines[i], intersect.intersectPoint);
                    if (alpha < 1 && alpha > 0)
                        sum += 1;
                    else if (IsEqual(alpha, 1))
                    {
                        RelPointAndLine r1 = RelationshipOfPointAndLine(ray, 
                            polygon.simpleLines[i].StartPoint);
                        // 注意范围
                        RelPointAndLine r2 = RelationshipOfPointAndLine(ray, 
                            polygon.simpleLines[(i + 1) % polygon.simpleLines.Length].EndPoint);
                        if (r1 == r2)
                        {
                            // 同边加2
                            sum += 2;
                            // 跳过下一条边的判断
                            i++;
                        }
                        else if (r2 == RelPointAndLine.LineOn)
                        {
                            // 判断三条边是否为凸包
                            RelPointAndLine r3 = RelationshipOfPointAndLine(
                                polygon.simpleLines[i],
                                polygon.simpleLines[(i + 1) % polygon.simpleLines.Length].EndPoint);
                            // 注意范围
                            RelPointAndLine r4 = RelationshipOfPointAndLine(
                                polygon.simpleLines[(i + 1) % polygon.simpleLines.Length],
                                polygon.simpleLines[(i + 2) % polygon.simpleLines.Length].EndPoint);
                            if (r3 == r4)
                                // 凸包加2
                                sum += 2;
                            else
                                // 否则加1
                                sum += 1;
                            // 跳过下两条边的判断
                            i += 2;
                        }
                        else
                        {
                            // 异边加1
                            sum += 1;
                            // 跳过下一条边的判断
                            i++;
                        }
                    }
                }
            }
            if (sum % 2 == 0)
                return false;
            else
                return true;
        }

        // 点是否在多边形的边上
        public static bool IsPolylineOfPolygonContainPoint(Polygon polygon, Point point)
        {
            for (int i = 0; i < polygon.simpleLines.Length; i++)
            {
                double alpha = PositionOfPointOnSimpleLine(polygon.simpleLines[i], point);
                if (alpha <= 1 && alpha >= 0)
                    return true;
            }
            return false;
        }
    }
}
