using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SpatialAnalysis.Network
{
    [Serializable]
    public abstract class BaseNetwork : INetwork
    {
        private List<Node> nodes;
        private List<Edge> edges;
        private NetworkType networkType = NetworkType.BaseNetwork;

        private int nodeCount;
        private int edgeCount;

        public int NodeCount
        {
            set { this.nodeCount = value; }
            get { return this.nodeCount; }
        }

        public int EdgeCount
        {
            set { this.edgeCount = value; }
            get { return this.edgeCount; }
        }

        public NetworkType NetworkType { get { return this.networkType; } }
        public List<Node> Nodes
        {
            get
            {
                return nodes;
            }
            set
            {
                this.nodes = value;
                this.NodeCount = value.Count;
            }
        }

        public List<Edge> Edges
        {
            get
            {
                return edges;
            }
            set
            {
                this.edges = value;
                this.EdgeCount = value.Count;
            }
        }

        public double[,] WeightMatrix
        {
            get
            {
                if (Nodes != null && Edges != null)
                    return CreateWeightMatrix(this.Nodes, this.Edges);
                else
                    return null;
            }
        }

        public List<Node>[,] EdgeMatrix
        {
            get
            {
                if (Nodes != null && Edges != null)
                    return CreateEdgeMatrix(this.Nodes, this.Edges);
                else
                    return null;
            }
        }

        public void Show(System.Drawing.Graphics graph, PictureBox picBox, int initPicBoxHeight)
        {
            graph.Clear(picBox.BackColor);
            // draw node
            if (Nodes == null || Nodes.Count <= 0)
                return;
            for (int i = 0; i < Nodes.Count; i++)
            {
                System.Drawing.Point p = new System.Drawing.Point((int)Nodes[i].Position.X, (int)(initPicBoxHeight - Nodes[i].Position.Y));
                graph.FillEllipse(Brushes.Red, p.X - 1, p.Y - 1, 2, 2);
                graph.DrawString(
                    "Node" + Nodes[i].NodeId,
                    new Font(new FontFamily("仿宋"), 12),
                    Brushes.Black,
                    new System.Drawing.RectangleF((int)Nodes[i].Position.X, (int)(initPicBoxHeight - Nodes[i].Position.Y),
                    60,
                    300));
            }
            // draw edge
            if (Edges == null || Edges.Count <= 0)
                return;
            for (int i = 0; i < Edges.Count; i++)
            {
                System.Drawing.Point p1 = new Point((int)Edges[i].StartNode.Position.X, (int)(initPicBoxHeight - Edges[i].StartNode.Position.Y));
                System.Drawing.Point p2 = new Point((int)Edges[i].EndNode.Position.X, (int)(initPicBoxHeight - Edges[i].EndNode.Position.Y));
                graph.DrawLine(Pens.Red, p1, p2);
            }
        }

        public virtual void Store(INetwork network)
        {
            if (network == null)
                return;
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "ntw文件(*.ntw)|*.ntw";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int position = save.FileName.LastIndexOf(@"\");
                string baseDir = save.FileName.Substring(0, position);
                string fileName = save.FileName.Substring(position + 1);
                Object obj = (object)network;
                Core.IO.WriteData(obj, baseDir, fileName);
                MessageBox.Show("保存成功！", "系统提示");
            }
        }

        public abstract void Load();
        public virtual void Clear()
        {
            this.Nodes = null;
            this.Edges = null;
        }

        public List<T> CopyObjectList<T>(List<T> nodes)
        {
            List<T> res = new List<T>();
            foreach (var item in nodes)
            {
                res.Add(item);
            }
            return res;
        }

        public void AddNode(Node node, int nodeId)
        {
            if (nodeId > this.NodeCount + 1 || nodeId < 1)
                return;
            node.NodeId = nodeId;
            this.Nodes.Insert(nodeId - 1, node);
        }

        public void AddEdge(Edge edge, int edgeId)
        {
            if (edgeId > this.EdgeCount + 1 || edgeId < 1)
                return;
            edge.EdgeId = edgeId;
            this.Edges.Insert(edgeId - 1, edge);
        }

        public void AddOne<T>(List<T> list, T obj, int index)
        {
            if (list == null || index > list.Count || index < 0)
                return;
            if (index == list.Count)
                list.Add(obj);
            else
                list.Insert(index, obj);
        }

        public void RemoveOne<T>(List<T> list, T obj)
        {
            if (list != null)
                list.Remove(obj);
        }

        // 生成权重邻接矩阵
        public double[,] CreateWeightMatrix(List<Node> nodes, List<Edge> edges)
        {
            // 按存储顺序生成邻接矩阵
            List<Edge> newEdges = CopyObjectList(edges);
            double[,] weightMatrix = new double[nodes.Count, nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                weightMatrix[i, i] = 0;
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    Edge tempEdge = null;
                    bool isConnected = false;
                    foreach (var item in newEdges)
                    {
                        if ((item.StartNode.Equals(nodes[i])
                            && item.EndNode.Equals(nodes[j]))
                            || (item.StartNode.Equals(nodes[j])
                            && item.EndNode.Equals(nodes[i])))
                        {
                            weightMatrix[i, j] = item.EdgeValue;
                            weightMatrix[j, i] = item.EdgeValue;
                            tempEdge = item;
                            isConnected = true;
                            break;
                        }
                    }
                    newEdges.Remove(tempEdge);
                    if (!isConnected)
                    {
                        weightMatrix[i, j] = Core.Constant.INF;
                        weightMatrix[j, i] = Core.Constant.INF;
                    }
                }
            }
            return weightMatrix;
        }

        // 生成网络路径矩阵
        public List<Node>[,] CreateEdgeMatrix(List<Node> nodes, List<Edge> edges)
        {
            // 按存储顺序生成邻接矩阵
            List<Edge> newEdges = CopyObjectList(edges);
            List<Node>[,] weightMatrix = new List<Node>[nodes.Count, nodes.Count];

            for (int i = 0; i < nodes.Count; i++)
            {
                weightMatrix[i, i] = null;
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    Edge tempEdge = null;
                    bool isConnected = false;
                    foreach (var item in newEdges)
                    {
                        if ((item.StartNode.Equals(nodes[i]) && item.EndNode.Equals(nodes[j]))
                            || (item.StartNode.Equals(nodes[j]) && item.EndNode.Equals(nodes[i])))
                        {
                            weightMatrix[i, j] = new List<Node>();
                            weightMatrix[i, j].Add(item.StartNode);
                            weightMatrix[i, j].Add(item.EndNode);
                            weightMatrix[j, i] = new List<Node>();
                            weightMatrix[j, i].Add(item.EndNode);
                            weightMatrix[j, i].Add(item.StartNode);
                            tempEdge = item;
                            isConnected = true;
                            break;
                        }
                    }
                    newEdges.Remove(tempEdge);
                    if (!isConnected)
                    {
                        weightMatrix[i, j] = new List<Node>();
                        weightMatrix[j, i] = new List<Node>(); ;
                    }
                }
            }
            return weightMatrix;
        }

        // Floyd算法,计算最短路径耗费矩阵及其路径矩阵
        public void Floyd(BaseNetwork network, ref List<Node>[,] pathMatrix, ref double[,] costMatrix)
        {
            if (network == null || network.Nodes == null || network.Edges == null)
                return;
            pathMatrix = new List<Node>[network.Nodes.Count, network.Nodes.Count];
            costMatrix = new double[network.Nodes.Count, network.Nodes.Count];
            List<Node>[,] path0 = network.EdgeMatrix;
            double[,] D0 = network.WeightMatrix;

            // 赋初值
            for (int i = 0; i < network.Nodes.Count; i++)
            {
                for (int j = i + 1; j < network.Nodes.Count; j++)
                {
                    pathMatrix[i, j] = new List<Node>();
                    foreach (var item in path0[i, j])
                    {
                        pathMatrix[i, j].Add(item);
                    }
                    pathMatrix[j, i] = new List<Node>();
                    for (int k = path0[i, j].Count - 1; k >= 0; k--)
                    {
                        pathMatrix[j, i].Add(path0[i, j][k]);
                    }
                    costMatrix[i, j] = D0[i, j];
                    costMatrix[j, i] = D0[j, i];
                }
            }

            // Floyd算法
            for (int n = 0; n < network.Nodes.Count; n++)
            {
                for (int i = 0; i < network.Nodes.Count; i++)
                {
                    for (int j = i + 1; j < network.Nodes.Count; j++)
                    {
                        double preCost = costMatrix[i, j];
                        double newCost = costMatrix[i, n] + costMatrix[n, j];
                        if (newCost < preCost)
                        {
                            // 更新耗费矩阵
                            costMatrix[i, j] = newCost;
                            costMatrix[j, i] = newCost;
                            // 更新路径
                            List<Node> path = new List<Node>();
                            foreach (var item in pathMatrix[i, n])
                            {
                                path.Add(item);
                            }
                            foreach (var item in pathMatrix[n, j])
                            {
                                if (!path.Contains(item))
                                    path.Add(item);
                            }
                            pathMatrix[i, j].Clear();
                            foreach (var item in path)
                            {
                                pathMatrix[i, j].Add(item);
                            }
                            path.Reverse();
                            pathMatrix[j, i].Clear();
                            foreach (var item in path)
                            {
                                pathMatrix[j, i].Add(item);
                            }
                        }
                    }
                }
            }
        }

        // Floyd算法,计算指定两结点间的最短路径耗费及其路径
        public void Floyd(BaseNetwork network, int startNodeId, int endNodeId, ref List<Node> path, ref double cost)
        {
            try
            {
                if (!IsNetworkContainNode(startNodeId, network) && !IsNetworkContainNode(endNodeId, network))
                {
                    throw new Exception("结点Id不存在，请选择网络中的点！");
                }
                int i = startNodeId - 1;
                int j = endNodeId - 1;

                List<Node>[,] pathMatrix = null;
                double[,] costMatrix = null;

                Floyd(network, ref pathMatrix, ref costMatrix);

                path = pathMatrix[i, j];
                cost = costMatrix[i, j];
            }
            catch (IndexOutOfRangeException ex)
            {
                // do something
                MessageBox.Show("传入的结点索引超界！");
            }
            catch (Exception ex)
            {
                // do something
                MessageBox.Show(ex.Message);
            }
        }

        public bool IsNetworkContainNode(int nodeId, BaseNetwork network)
        {
            for (int i = 0; i < network.Nodes.Count; i++)
            {
                if (nodeId == network.Nodes[i].NodeId)
                    return true;
            }
            return false;
        }

    }
}
