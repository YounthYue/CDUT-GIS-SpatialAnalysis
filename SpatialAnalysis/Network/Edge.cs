using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialAnalysis.Network
{
    [Serializable]
    public class Edge : IEdge
    {
        private int edgeId;
        private string edgeName;
        private double edgeValue;
        private int startNodeId;
        private int endNodeId;
        private Node startNode;
        private Node endNode;

        public Node StartNode
        {
            set
            {
                this.startNode = value;
            }
            get
            {
                return this.startNode;
            }
        }

        public Node EndNode
        {
            set
            {
                this.endNode = value;
            }
            get
            {
                return this.endNode;
            }
        }

        public int StartNodeId
        {
            set
            {
                this.startNodeId = value;
            }
            get
            {
                if (startNodeId == 0 && startNode != null)
                    return startNode.NodeId;
                else
                    return startNodeId;
            }
        }
        public int EndNodeId
        {
            set
            {
                this.endNodeId = value;
            }
            get
            {
                if (endNodeId == 0 && endNode != null)
                    return endNode.NodeId;
                else
                    return endNodeId;
            }
        }
        public int EdgeId
        {
            set
            {
                this.edgeId = value;
            }
            get
            {
                return edgeId;
            }
        }
        public string EdgeName
        {
            set
            {
                this.edgeName = value;
            }
            get
            {
                return edgeName;
            }
        }
        public double EdgeValue
        {
            set
            {
                this.edgeValue = value;
            }
            get
            {
                return edgeValue;
            }
        }

        public Edge(int edgeId, double edgeValue, Node startNode, Node endNode)
        {
            if (startNode.NodeId == endNode.NodeId)
                return;
            this.edgeId = edgeId;
            this.StartNode = startNode;
            this.EndNode = endNode;
            this.edgeValue = edgeValue;
        }

        public Edge(int edgeId, Node startNode, Node endNode)
        {
            if (startNode.NodeId == endNode.NodeId)
                return;
            this.edgeId = edgeId;
            this.StartNode = startNode;
            this.EndNode = endNode;
            this.edgeValue = 0.0;
        }

        public Edge(Node startNode, Node endNode)
        {
            if (startNode.NodeId == endNode.NodeId)
                return;
            this.StartNode = startNode;
            this.EndNode = endNode;
            this.edgeValue = 0.0;
        }

        public Edge(int EdgeId, double EdgeValue, int StartNodeId, int EndNodeId, List<Node> nodes)
        {
            if (StartNodeId == EndNodeId)
                return;
            this.EdgeId = EdgeId;
            this.StartNodeId = StartNodeId;
            this.EndNodeId = EndNodeId;
            this.EdgeValue = EdgeValue;
            this.StartNode = GetNodeFromNodesById(nodes, StartNodeId);
            this.EndNode = GetNodeFromNodesById(nodes, EndNodeId);
        }

        public Edge(int EdgeId, int StartNodeId, int EndNodeId, List<Node> nodes)
        {
            if (StartNodeId == EndNodeId)
                return;
            this.EdgeId = EdgeId;
            this.StartNodeId = StartNodeId;
            this.EndNodeId = EndNodeId;
            this.EdgeValue = 0.0;
            this.StartNode = GetNodeFromNodesById(nodes, StartNodeId);
            this.EndNode = GetNodeFromNodesById(nodes, EndNodeId);
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

        public Edge FindExistIn_ByNodeId(List<Edge> edges)
        {
            foreach (var item in edges)
            {
                if (item.StartNode.NodeId == this.StartNode.NodeId || item.StartNode.NodeId == this.EndNode.NodeId)
                    if (item.EndNode.NodeId == this.StartNode.NodeId || item.EndNode.NodeId == this.EndNode.NodeId)
                        return item;
            }
            return null;
        }

        public bool IsEqual_ByNodeId(Edge e)
        {
            if (this.StartNode.NodeId == e.StartNode.NodeId || this.EndNode.NodeId == e.StartNode.NodeId)
                if (this.StartNode.NodeId == e.EndNode.NodeId || this.EndNode.NodeId == e.EndNode.NodeId)
                    return true;
            return false;
        }
    }
}
