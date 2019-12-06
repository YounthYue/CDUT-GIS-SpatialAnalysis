using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Network
{
    [Serializable]
    public class Node : INode
    {
        private int nodeId;
        private string nodeName;
        private double nodeValue;
        private Core.Point position;

        public int NodeId
        {
            set
            {
                this.nodeId = value;
            }
            get
            {
                return nodeId;
            }
        }
        public string NodeName
        {
            set
            {
                this.nodeName = value;
            }
            get
            {
                return nodeName;
            }
        }
        public double NodeValue
        {
            set
            {
                this.nodeValue = value;
            }
            get
            {
                return nodeValue;
            }
        }
        public Core.Point Position
        {
            set
            {
                this.position = new Core.Point(value.X, value.Y);
            }
            get
            {
                return position;
            }
        }

        public Node(int nodeId, Core.Point position)
        {
            this.nodeId = nodeId;
            this.Position = position;
        }

        public Node(Core.Point position)
        {
            this.Position = position;
        }

        public Node(Core.Point position, double value)
        {
            this.Position = position;
            this.nodeValue = value;
        }

        //public Node(int nodeId, Core.Point position, double value)
        //{
        //    this.nodeId = nodeId;
        //    this.Position = position;
        //    this.nodeValue = value;
        //}

        internal bool Equals(Node node)
        {
            if (!Core.SpatialAnalysis.IsEqual(this.position.X, node.position.X))
                return false;
            if (!Core.SpatialAnalysis.IsEqual(this.position.Y, node.position.Y))
                return false;
            if (this.nodeId != node.nodeId)
                return false;
            if (!Core.SpatialAnalysis.IsEqual(this.nodeValue, node.nodeValue))
                return false;
            return true;
        }

        public double DistanceWith(Node node)
        {
            double deltaX = this.Position.X - node.Position.X;
            double deltaY = this.Position.Y - node.Position.Y;
            double square = deltaX * deltaX + deltaY * deltaY;
            double distance = System.Math.Sqrt(square);
            if (distance >= 0)
                return distance;
            else
                return 0.0;
        }

        public Node FindExistIn_ByNodeId(List<Node> nodes)
        {
            foreach (var item in nodes)
            {
                if (item.NodeId == this.NodeId)
                    return item;
            }
            return null;
        }

        public bool IsEqual_ById(Node node)
        {
            if (this.NodeId == node.NodeId)
                return true;
            else
                return false;
        }
    }
}
