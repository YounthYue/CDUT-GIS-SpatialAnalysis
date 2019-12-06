using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Network
{
    [Serializable]
    public class Triangle
    {
        private int triangleId;
        private double triangleValue;
        private int stNodeId;
        private int ndNodeId;
        private int rdNodeId;
        private Node stNode;
        private Node ndNode;
        private Node rdNode;

        public int TriangleId
        {
            get { return this.triangleId; }
            set { this.triangleId = value; }
        }

        public double TriangleValue { get { return this.triangleValue; } set { this.triangleValue = value; } }

        public int StNodeId
        {
            get
            {
                if (this.stNodeId == 0 && this.StNode != null)
                    return StNode.NodeId;
                else
                    return this.stNodeId;
            }
            set { this.stNodeId = value; }
        }
        public int NdNodeId
        {
            get
            {
                if (this.ndNodeId == 0 && this.NdNode != null)
                    return NdNode.NodeId;
                else
                    return this.ndNodeId;
            }
            set { this.ndNodeId = value; }
        }
        public int RdNodeId
        {
            get
            {
                if (this.rdNodeId == 0 && this.RdNode != null)
                    return RdNode.NodeId;
                else
                    return this.rdNodeId;
            }
            set { this.rdNodeId = value; }
        }

        public Node StNode { get { return this.stNode; } set { this.stNode = value; } }
        public Node NdNode { get { return this.ndNode; } set { this.ndNode = value; } }
        public Node RdNode { get { return this.rdNode; } set { this.rdNode = value; } }

        public Triangle(int TriangleId, int StNodeId, int NdNodeId, int RdNodeId, double TriangleValue, List<Node> nodes)
        {
            this.TriangleId = TriangleId;
            this.TriangleValue = TriangleValue;

            this.StNodeId = StNodeId;
            this.NdNodeId = NdNodeId;
            this.RdNodeId = RdNodeId;

            this.StNode = GetNodeFromNodesById(nodes, StNodeId);
            this.NdNode = GetNodeFromNodesById(nodes, NdNodeId);
            this.RdNode = GetNodeFromNodesById(nodes, RdNodeId);
        }

        public Triangle(int TriangleId, int StNodeId, int NdNodeId, int RdNodeId, List<Node> nodes)
        {
            this.TriangleId = TriangleId;

            this.StNodeId = StNodeId;
            this.NdNodeId = NdNodeId;
            this.RdNodeId = RdNodeId;

            this.StNode = GetNodeFromNodesById(nodes, StNodeId);
            this.NdNode = GetNodeFromNodesById(nodes, NdNodeId);
            this.RdNode = GetNodeFromNodesById(nodes, RdNodeId);
        }

        public Triangle(int TriangleId, Node StNode, Node NdNode, Node RdNode)
        {
            this.TriangleId = TriangleId;

            this.StNode = StNode;
            this.NdNode = NdNode;
            this.RdNode = RdNode;
        }

        public Triangle(Node StNode, Node NdNode, Node RdNode)
        {
            if (StNode.NodeId == NdNode.NodeId || RdNode.NodeId == NdNode.NodeId || RdNode.NodeId == StNode.NodeId)
            {
                int a = 1;
            }
            this.StNode = StNode;
            this.NdNode = NdNode;
            this.RdNode = RdNode;
        }

        public Node GetNodeFromNodesById(List<Node> nodes, int nodeId)
        {
            foreach (var item in nodes)
            {
                if (item.NodeId == nodeId)
                    return item;
            }
            return null;
        }

        public static List<Triangle> FindTrianglesContainEdge(Edge edge, List<Triangle> triangles)
        {
            List<Triangle> res = new List<Triangle>();
            foreach (var item in triangles)
            {
                if (Equals(edge.StartNode, item.StNode) || Equals(edge.StartNode, item.NdNode) || Equals(edge.StartNode, item.RdNode))
                    if (Equals(edge.StartNode, item.StNode) || Equals(edge.EndNode, item.NdNode) || Equals(edge.EndNode, item.RdNode))
                        res.Add(item);
            }
            return res;
        }

        public static List<Node> FindNodesOfTrianglesContainEdge(Edge edge, List<Triangle> triangles)
        {
            List<Node> res = new List<Node>();
            foreach (var item in triangles)
            {
                if (Equals(edge.StartNode, item.StNode))
                    if (Equals(edge.EndNode, item.NdNode))
                    { res.Add(item.RdNode); }
                    else if (Equals(edge.EndNode, item.RdNode))
                    { res.Add(item.NdNode); }
                if (Equals(edge.StartNode, item.NdNode))
                {
                    if (Equals(edge.EndNode, item.StNode))
                    { res.Add(item.RdNode); }
                    else if (Equals(edge.EndNode, item.RdNode))
                    { res.Add(item.StNode); }
                }
                if (Equals(edge.StartNode, item.RdNode))
                {
                    if (Equals(edge.EndNode, item.StNode))
                    { res.Add(item.NdNode); }
                    else if (Equals(edge.EndNode, item.NdNode))
                    { res.Add(item.StNode); }
                }
            }
            return res;
        }

        public void GetCircumCircle(ref Core.Point center, ref double radius)
        {
            Core.SimpleLine line1 = new Core.SimpleLine(this.StNode.Position, this.NdNode.Position);
            Core.SimpleLine line2 = new Core.SimpleLine(this.StNode.Position, this.RdNode.Position);
            Core.Point point = Core.SpatialAnalysis.TwoLineOfIntersectPoint(line1.Midperpendicular, line2.Midperpendicular);
            if (point != null)
            {
                center = point;
                radius = center.DistanceWith(this.StNode.Position);
            }
        }

        public Core.Point CircumCircleCenter
        {
            get
            {
                double r = 0;
                Core.Point center = null;
                GetCircumCircle(ref center, ref r);
                if (center == null)
                    GetCircumCircle(ref center, ref r);
                return center;
            }
        }

        public double CircumCircleRadius
        {
            get
            {
                double r = 0;
                Core.Point center = null;
                GetCircumCircle(ref center, ref r);
                return r;
            }
        }

        public double MinEdgeLength
        {
            get
            {
                double d1 = StNode.DistanceWith(NdNode);
                double d2 = StNode.DistanceWith(RdNode);
                double d3 = RdNode.DistanceWith(NdNode);
                double min = d3 < (d1 < d2 ? d1 : d2) ? d3 : (d1 < d2 ? d1 : d2);
                return min;
            }
        }

        public Triangle FindExistIn_ByNodeId(List<Triangle> triangles)
        {
            foreach (var item in triangles)
            {
                if (item.StNode.NodeId == this.StNode.NodeId || item.StNode.NodeId == this.NdNode.NodeId || item.StNode.NodeId == this.RdNode.NodeId)
                    if (item.NdNode.NodeId == this.StNode.NodeId || item.NdNode.NodeId == this.NdNode.NodeId || item.NdNode.NodeId == this.RdNode.NodeId)
                        if (item.RdNode.NodeId == this.StNode.NodeId || item.RdNode.NodeId == this.NdNode.NodeId || item.RdNode.NodeId == this.RdNode.NodeId)
                            return item;
            }
            return null;
        }

        public bool IsCircumCircleContain(Core.Point point)
        {
            double distance = this.CircumCircleCenter.DistanceWith(point);
            if (distance - this.CircumCircleRadius < -0.0000001)
                return true;
            else
                return false;
        }

        public bool IsIn(Core.Point point)
        {
            Core.RelPointAndLine rel = Core.SpatialAnalysis.RelationshipOfPointAndLine(
                new Core.SimpleLine(this.StNode.Position, this.NdNode.Position),
                this.RdNode.Position
                );
            if(
                Core.SpatialAnalysis.RelationshipOfPointAndLine(new Core.SimpleLine(this.StNode.Position, this.NdNode.Position), point) == rel
                && Core.SpatialAnalysis.RelationshipOfPointAndLine(new Core.SimpleLine(this.NdNode.Position, this.RdNode.Position), point) == rel
                && Core.SpatialAnalysis.RelationshipOfPointAndLine(new Core.SimpleLine(this.RdNode.Position, this.StNode.Position), point) == rel
                )
                return true;
            else
                return false;
        }
    }
}
