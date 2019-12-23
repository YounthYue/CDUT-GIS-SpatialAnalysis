using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpatialAnalysis.Network
{
    [Serializable]
    public class TIN : BaseNetwork
    {
        private List<Triangle> triangles;

        private int trianglesCount;

        private NetworkType networkType = NetworkType.TIN;

        public new NetworkType NetworkType { get { return this.networkType; } }

        public List<Triangle> Triangles
        {
            set
            {
                this.triangles = value;
                this.trianglesCount = value.Count;
            }
            get { return this.triangles; }
        }

        public int TrianglesCount
        { set { this.trianglesCount = value; } get { return this.trianglesCount; } }

        public void Store()
        {
            base.Store(this);
        }

        public override void Load()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "ntw文件(*.ntw)|*.ntw";
            open.InitialDirectory = new DirectoryInfo(@"..\..\..\").FullName + @"networks\";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int position = open.FileName.LastIndexOf(@"\");
                string baseDir = open.FileName.Substring(0, position);
                string fileName = open.FileName.Substring(position + 1);
                Object obj = Core.IO.ReadDataToMemory(baseDir, fileName);
                TIN newTIN = null;
                if (obj is TIN)
                    newTIN = (TIN)obj;
                else
                    return;
                if (newTIN == null)
                    return;
                this.Nodes = this.CopyObjectList(newTIN.Nodes);
                this.Edges = this.CopyObjectList(newTIN.Edges);
                this.Triangles = this.CopyObjectList(newTIN.Triangles);
            }
        }

        public void Clear()
        {
            base.Clear();
            this.Triangles = null;
        }
        // 构造函数
        public TIN() { }

        public TIN(List<Node> nodes)
        {
            if (nodes == null || nodes.Count == 0)
                return;
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].NodeId = i + 1;
            }
            this.Nodes = CopyObjectList(nodes);
            if (nodes.Count < 3)
                return; // 以下操作至少需要三个结点
            //// 生成不规则三角网算法 
            //// 算法初始化开始
            //List<Node> nodePool = CopyObjectList(this.Nodes);
            //List<Edge> edges = new List<Edge>();
            //// 选取初始点
            //Node node_0 = nodes[0];
            //// 选取初始边(距离最小原则)
            //Node node_1 = null;
            //double dMin = 10000000;
            //for (int i = 1; i < nodes.Count; i++)
            //{
            //    double d = node_0.DistanceWith(nodes[i]);
            //    if (d < dMin)
            //    {
            //        dMin = d;
            //        node_1 = nodes[i];
            //    }
            //}
            //nodePool.Remove(node_0);
            //nodePool.Remove(node_1);
            //Edge edge_0 = new Edge(node_0, node_1);
            //edges.Add(edge_0);

            //// 递归生成三角形，遵循空圆法则以及最大最小角准则
            //if (nodes.Count <= 2)
            //    return; // 以下操作至少需要三个结点
            //List<Triangle> triangles = new List<Triangle>();

            //// 寻找指定边右侧，满足空圆法则的结点
            //Node newNode = null;
            //if (nodes.Count == 4)
            //{
            //    int a = 1;
            //}
            //// 初始边右侧遍历
            //MinCircumCircle(edge_0, nodePool, nodes, ref newNode, Core.RelPointAndLine.LineRight);
            //if (newNode != null)
            //{
            //    nodePool.Remove(newNode);
            //    triangles.Add(new Triangle(edge_0.StartNode, edge_0.EndNode, newNode));
            //    Edge edge_1 = new Edge(edge_0.StartNode, newNode);
            //    edges.Add(edge_1);
            //    Edge edge_2 = new Edge(edge_0.EndNode, newNode);
            //    edges.Add(edge_2);
            //    // 算法初始化结束
            //    // Create TIN
            //    CreateTIN(edge_1, nodePool, nodes, edges, triangles);
            //    CreateTIN(edge_2, nodePool, nodes, edges, triangles);
            //}
            //newNode = null;
            // 初始边左侧遍历
            //MinCircumCircle(edge_0, nodePool, nodes, ref newNode, Core.RelPointAndLine.LineLeft);
            //if (newNode != null)
            //{
            //    nodePool.Remove(newNode);
            //    triangles.Add(new Triangle(edge_0.StartNode, edge_0.EndNode, newNode));
            //    Edge edge_1 = new Edge(edge_0.StartNode, newNode);
            //    edges.Add(edge_1);
            //    Edge edge_2 = new Edge(edge_0.EndNode, newNode);
            //    edges.Add(edge_2);
            //    // 算法初始化结束
            //    // Create TIN
            //    CreateTIN(edge_1, nodePool, nodes, edges, triangles);
            //    CreateTIN(edge_2, nodePool, nodes, edges, triangles);
            //}
            //newNode = null;
            List<Edge> edges = new List<Edge>();
            List<Triangle> triangles = new List<Triangle>();
            // 闭包收缩算法中间变量
            List<Node> oldNodes = new List<Node>();
            List<Node> newNodes = new List<Node>();
            List<Edge> oldEdges = new List<Edge>();
            // 为TIN生成凸包边界
            CreateConvexBound(nodes, edges, oldNodes);
            oldEdges = CopyObjectList(edges);
            // 递归深度
            int num1 = 0;
            // 向内收缩算法
            InShrink(
                oldNodes,
                newNodes,
                oldEdges,
                CopyObjectList(nodes),
                nodes,
                edges,
                triangles,
                ref num1);
            // MutileConvex(nodes, edges, oldNodes);
            // 为边、三角形生成Id
            for (int i = 0; i < edges.Count; i++)
            {
                edges[i].EdgeId = i + 1;
                double deltaX = edges[i].StartNode.Position.X - edges[i].EndNode.Position.X;
                double deltaY = edges[i].StartNode.Position.Y - edges[i].EndNode.Position.Y;
                double square = deltaX * deltaX + deltaY * deltaY;
                edges[i].EdgeValue = System.Math.Sqrt(square);
            }
            for (int i = 0; i < triangles.Count; i++)
            {
                triangles[i].TriangleId = i + 1;
            }
            // 为TIN添加边和三角形
            this.Edges = edges;
            this.Triangles = triangles;
            // TIN Created Successful
        }

        private void MutileConvex(List<Node> nodes, List<Edge> edges, List<Node> bounds)
        {
            if (nodes.Count >= 3)
            {
                List<Node> oldNodes = CopyObjectList(nodes);
                CreateConvexBound(oldNodes, edges, bounds);
                foreach (var item in bounds)
                {
                    oldNodes.Remove(item);
                }
                bounds.Clear();
                MutileConvex(oldNodes, edges, bounds);
            }
            else if (nodes.Count == 2)
            {
                edges.Add(new Edge(nodes[0], nodes[1]));
                return;
            }
            else
                return;
        }

        // 向内收缩算法
        private void InShrink(
            List<Node> oldNodes,
            List<Node> newNodes,
            List<Edge> oldEdges,
            List<Node> nodePool,
            List<Node> nodes,
            List<Edge> edges,
            List<Triangle> triangles,
            ref int num1)
        {
            List<Node> nodeScope = CopyObjectList(nodePool);
            if (num1 >= 20)
            {
                int a = 1;
            }
            // 孤点群
            if (oldNodes.Count == 0 && oldEdges.Count == 0 && nodePool.Count > 0)
            {
                int a = 1;
                // num1 = ReamoveSingalPoint(newNodes, nodePool, nodes, edges, triangles, num1);
            }
            // 更新结点池
            foreach (var item in oldNodes)
            {
                Node node = item.FindExistIn_ByNodeId(nodePool);
                if (node != null)
                    nodePool.Remove(node);
            }
            // 如果凸边界内没有结点，计算完全凸边界的三角形，然后退出算法，完成TIN生成
            if (!IsConvexContainNodeOfPool(oldEdges, nodePool))
            {
                if (oldEdges != null && oldEdges.Count > 0)
                {
                    int num2 = 0;
                    CreateTINWithFullConvex(oldEdges[0], oldNodes, edges, triangles, ref num2);
                }
                if (nodePool.Count > 0)
                {
                    num1 = RemoveSingalPoint(newNodes, nodePool, nodes, edges, triangles, num1);
                }
                return;
            }
            // 寻找闭合凸圈内的新点
            newNodes.Clear();
            if (nodes.Count == 6)
            {
                int a = 1;
            }
            List<Edge> newEdges = new List<Edge>();
            foreach (var item in oldEdges)
            {
                Node newNode = null;
                // 寻找指定边与新的结点，满足空圆法则的结点
                MinCircumCircle(item, nodePool, nodeScope, ref newNode, Core.RelPointAndLine.LineRight);
                // 满足要求的结点遍历完毕
                if (newNode == null)
                    continue;
                else
                {
                    if (newNode.FindExistIn_ByNodeId(newNodes) == null)
                        newNodes.Add(newNode);
                    Edge e1 = new Edge(item.StartNode, newNode);
                    if (e1.FindExistIn_ByNodeId(newEdges) == null)
                        newEdges.Add(e1);
                    if (e1.FindExistIn_ByNodeId(edges) == null)
                        edges.Add(e1);
                    Edge e2 = new Edge(newNode, item.EndNode);
                    if (e2.FindExistIn_ByNodeId(newEdges) == null)
                        newEdges.Add(e2);
                    if (e2.FindExistIn_ByNodeId(edges) == null)
                        edges.Add(e2);
                    Triangle triangle = new Triangle(item.StartNode, item.EndNode, newNode);
                    triangles.Add(triangle);
                }
            }

            // 修正newNodes与newEdges
            //List<Edge> preEdges = CopyObjectList(newEdges);

            //ResetConvexBound(newNodes, nodePool, edges, newEdges, preEdges);

            //foreach (var item in newEdges)
            //{
            //    if (!preEdges.Contains(item))
            //    {
            //        ResetConvexBound(newNodes, nodePool, edges, newEdges, CopyObjectList(newEdges));
            //    }
            //}
            

            // 添加朝向外的三角形
            for (int i = 0; i < newEdges.Count; i++)
            {
                Node n1 = newEdges[i].StartNode;
                Node n2 = newEdges[i].EndNode;
                Node n3 = newEdges[(i + 1) % newEdges.Count].EndNode;
                if (n3.NodeId == n1.NodeId || n3.NodeId == n2.NodeId)
                    n3 = newEdges[(i + 1) % newEdges.Count].StartNode;
                Triangle t = new Triangle(n1, n2, n3);

                if (t.FindExistIn_ByNodeId(triangles) == null)
                {
                    triangles.Add(t);
                }
            }
            // 直接在edges中修正newEdges，满足最大最小角规则
            foreach (var item in newEdges)
            {
                List<Node> temp = Triangle.FindNodesOfTrianglesContainEdge(item, triangles);
                if (temp != null && temp.Count == 2)
                {
                    MinAngleMustBeMax(temp[0], item.StartNode, temp[1], item.EndNode, edges, triangles);
                }
                else
                {
                    continue;
                }
            }
            // 准备开始递归
            // 生成新的凸边
            oldEdges.Clear();
            oldNodes.Clear();
            if (newNodes.Count >= 2)
            {
                for (int i = 0; i < newNodes.Count; i++)
                {
                    Edge item = new Edge(newNodes[i], newNodes[(i + 1) % newNodes.Count]);
                    oldEdges.Add(item);
                    if (item.FindExistIn_ByNodeId(edges) == null)
                        edges.Add(item);
                }
                // 新点变旧点
                oldNodes = CopyObjectList(newNodes);
            }
            else if (newNodes.Count == 1)
                oldNodes.Add(newNodes[0]);

            // 递归执行算法
            num1 += 1;
            InShrink(
                oldNodes,
                newNodes,
                oldEdges,
                nodePool,
                nodes,
                edges,
                triangles,
                ref num1);
        }

        private static void ResetConvexBound(List<Node> newNodes, List<Node> nodePool, List<Edge> edges, List<Edge> newEdges, List<Edge> preEdges)
        {
            for (int i = 0; i < preEdges.Count; i++)
            {
                Node n1 = preEdges[i].StartNode;
                Node n2 = preEdges[i].EndNode;
                Node n3 = preEdges[(i + 1) % preEdges.Count].EndNode;
                if (n3.NodeId == n1.NodeId || n3.NodeId == n2.NodeId)
                    n3 = preEdges[(i + 1) % preEdges.Count].StartNode;
                Triangle t = new Triangle(n1, n2, n3);

                int b = 0;

                foreach (var item in nodePool)
                {
                    if (t.IsIn(item.Position))
                    {
                        // 修改标志
                        b = -1;
                        // 提取点
                        List<Node> triangelNodes = new List<Node>();
                        triangelNodes.Add(t.StNode);
                        triangelNodes.Add(t.NdNode);
                        triangelNodes.Add(t.RdNode);
                        List<Node> nNodes = new List<Node>();
                        foreach (var node in newNodes)
                        {
                            if (node.IsEqual_ById(t.StNode))
                            {
                                nNodes.Add(node);
                                triangelNodes.Remove(t.StNode);
                            }
                            if (node.IsEqual_ById(t.NdNode))
                            {
                                nNodes.Add(node);
                                triangelNodes.Remove(t.NdNode);
                            }
                            if (node.IsEqual_ById(t.RdNode))
                            {
                                nNodes.Add(node);
                                triangelNodes.Remove(t.RdNode);
                            }
                        }
                        // 处理
                        Edge oldEdge = (new Edge(nNodes[0], nNodes[1])).FindExistIn_ByNodeId(edges);
                        if (oldEdge != null)
                            edges.Remove(oldEdge);
                        if (item.FindExistIn_ByNodeId(newNodes) == null)
                            newNodes.Insert(newNodes.IndexOf(nNodes[1]), item);
                        Edge newEdge = new Edge(item, triangelNodes[0]);
                        int index = 0;
                        for (int j = 0; i < newEdges.Count; j++)
                        {
                            if (newEdges[j].StartNode.IsEqual_ById(nNodes[0]) || newEdges[j].EndNode.IsEqual_ById(nNodes[0]))
                                if (newEdges[(j + 1) % newEdges.Count].StartNode.IsEqual_ById(nNodes[1]) || newEdges[(j + 1) % newEdges.Count].EndNode.IsEqual_ById(nNodes[1]))
                                {
                                    index = (j + 1) % newEdges.Count;
                                    break;
                                }
                        }
                        if (newEdge.FindExistIn_ByNodeId(newEdges) == null)
                            newEdges.Insert(index, newEdge);
                        if (newEdge.FindExistIn_ByNodeId(edges) == null)
                            edges.Add(newEdge);
                    }
                }
                if (b == 0)
                    continue;
                else
                    break;
            }
        }

        private int RemoveSingalPoint(List<Node> newNodes, List<Node> nodePool, List<Node> nodes, List<Edge> edges, List<Triangle> triangles, int num1)
        {
            List<Node> extraNodes = new List<Node>();
            List<Triangle> tempTriangles = CopyObjectList(triangles);
            foreach (var item in tempTriangles)
            {
                foreach (var node in nodePool)
                {
                    if (item.IsIn(node.Position))
                        extraNodes.Add(node);
                }
                if (extraNodes.Count > 0)
                {
                    List<Node> triangelNodes = new List<Node>();
                    triangelNodes.Add(item.StNode);
                    triangelNodes.Add(item.NdNode);
                    triangelNodes.Add(item.RdNode);

                    extraNodes.Add(item.StNode);
                    extraNodes.Add(item.NdNode);
                    extraNodes.Add(item.RdNode);

                    List<Edge> triangelEdges = new List<Edge>();
                    triangelEdges.Add(new Edge(triangelNodes[0], triangelNodes[1]));
                    triangelEdges.Add(new Edge(triangelNodes[1], triangelNodes[2]));
                    triangelEdges.Add(new Edge(triangelNodes[2], triangelNodes[0]));
                    // 在三角形内继续分割
                    num1 += 1;
                    InShrink(
                        triangelNodes,
                        newNodes,
                        triangelEdges,
                        extraNodes,
                        nodes,
                        edges,
                        triangles,
                        ref num1);
                    extraNodes.Clear();
                }
            }
            return num1;
        }

        // 凸包内无结点生成TIN算法
        private void CreateTINWithFullConvex(Edge edge, List<Node> fixedNodePool, List<Edge> edges, List<Triangle> triangles, ref int num2)
        {
            if (num2 >= 20)
            {
                int a = 1;
            }
            // 至少需要4个结点
            if (fixedNodePool.Count == 3)
            {
                triangles.Add(new Triangle(fixedNodePool[0], fixedNodePool[1], fixedNodePool[2]));
                return;
            }
            else if (fixedNodePool.Count < 3)
                return;
            Node newNode = null;
            // 寻找指定边与新的结点，满足空圆法则的结点
            MinCircumCircle(edge, fixedNodePool, fixedNodePool, ref newNode, Core.RelPointAndLine.LineRight);
            // 满足要求的结点遍历完毕
            if (newNode == null)
                return;
            Edge e1 = new Edge(edge.StartNode, newNode);
            Edge e2 = new Edge(newNode, edge.EndNode);
            // 将包含同一边的两个三角形进行最大最小角判别
            List<Node> temp = Triangle.FindNodesOfTrianglesContainEdge(edge, triangles);
            Node n = newNode.FindExistIn_ByNodeId(temp);
            if (temp != null && n != null)
                temp.Remove(n);
            if (temp.Count > 0)
            {
                if (temp[0].NodeId == newNode.NodeId)
                {
                    MinCircumCircle(edge, fixedNodePool, fixedNodePool, ref newNode, Core.RelPointAndLine.LineRight);
                    temp = Triangle.FindNodesOfTrianglesContainEdge(edge, triangles);
                }
                MinAngleMustBeMax(temp[0], edge.StartNode, newNode, edge.EndNode, edges, triangles);
            }
            else
            {
                if (e1.FindExistIn_ByNodeId(edges) == null)
                    edges.Add(e1);

                if (e2.FindExistIn_ByNodeId(edges) == null)
                    edges.Add(e2);

                triangles.Add(new Triangle(edge.StartNode, edge.EndNode, newNode));
            }
            num2 += 1;
            CreateTINWithFullConvex(e1, fixedNodePool, edges, triangles, ref num2);
            num2 += 1;
            CreateTINWithFullConvex(e2, fixedNodePool, edges, triangles, ref num2);
        }

        // 凸包生成算法
        private void CreateConvexBound(List<Node> nodes, List<Edge> edges, List<Node> oldNodes)
        {
            List<Node> baseBounds = new List<Node>();
            List<Node> bounds = new List<Node>();
            // 寻找左下角与右上角点
            double max = 0;
            int max_index = 0;
            double min = 100000;
            int min_index = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Position.X + nodes[i].Position.Y > max)
                {
                    max = nodes[i].Position.X + nodes[i].Position.Y;
                    max_index = i;
                }
                if (nodes[i].Position.X + nodes[i].Position.Y < min)
                {
                    min = nodes[i].Position.X + nodes[i].Position.Y;
                    min_index = i;
                }
            }
            baseBounds.Add(nodes[min_index]);
            baseBounds.Add(nodes[max_index]);

            if (nodes.Count == 4)
            {
                int a = 1;
            }
            // 寻找所有的凸边界点
            for (int i = 0; i < baseBounds.Count; i++)
            {
                Core.SimpleLine baseLine = new Core.SimpleLine(baseBounds[i].Position, baseBounds[(i + 1) % baseBounds.Count].Position);
                if (baseBounds[i].FindExistIn_ByNodeId(bounds) == null)
                    bounds.Add(baseBounds[i]);
                if (baseBounds[(i + 1) % baseBounds.Count].FindExistIn_ByNodeId(bounds) == null)
                    bounds.Add(baseBounds[(i + 1) % baseBounds.Count]);
                FindConvexBound(nodes, bounds, baseLine, bounds[(i + 1) % baseBounds.Count]);
            }

            // 为TIN添加凸边及三角形
            for (int i = 0; i < bounds.Count; i++)
            {
                oldNodes.Add(bounds[i]);
                Edge edge = new Edge(bounds[i], bounds[(i + 1) % bounds.Count]);
                if (edge.FindExistIn_ByNodeId(edges) == null)
                {
                    edges.Add(edge);
                }
            }
        }

        private static void FindConvexBound(List<Node> nodes, List<Node> bounds, Core.SimpleLine baseLine, Node lastNode)
        {
            double lM = 0;
            Node lm = null;
            foreach (var item in nodes)
            {
                Core.RelPointAndLine rel = Core.SpatialAnalysis.RelationshipOfPointAndLine(baseLine, item.Position);
                if (rel == Core.RelPointAndLine.LineLeft)
                {
                    if (item.Position.DistanceWith(baseLine) > lM)
                    {
                        lM = item.Position.DistanceWith(baseLine);
                        lm = item;
                    }
                }
            }
            if (lm != null)
            {
                int lastIndex = bounds.IndexOf(lastNode);
                if (lastIndex == 0)
                    bounds.Add(lm);
                else if (lm.FindExistIn_ByNodeId(bounds) == null)
                    bounds.Insert(lastIndex, lm);
                int index = bounds.IndexOf(lm);
                Core.SimpleLine baseLine1 = new Core.SimpleLine(bounds[index - 1].Position, lm.Position);
                Core.SimpleLine baseLine2 = new Core.SimpleLine(lm.Position, bounds[(index + 1) % bounds.Count].Position);
                FindConvexBound(nodes, bounds, baseLine1, lm);
                FindConvexBound(nodes, bounds, baseLine2, lastNode);
            }
            else
                return;
        }

        // 扩展生长算法（失败）
        private void CreateTIN(Edge edge, List<Node> nodePool, List<Node> nodes, List<Edge> edges, List<Triangle> triangles)
        {
            if (nodePool == null || nodePool.Count == 0)
            {
                if (edge.FindExistIn_ByNodeId(edges) == null)
                    edges.Add(edge);
                // 递归结束条件
                return;
            }
            // 寻找指定边与新的结点，满足空圆法则的结点
            Node newNode = null;
            MinCircumCircle(edge, nodePool, nodes, ref newNode, Core.RelPointAndLine.LineRight);
            // 满足要求的结点遍历完毕
            if (newNode == null)
                return;
            nodePool.Remove(newNode);
            Edge edge_1 = new Edge(edge.StartNode, newNode);
            Edge edge_2 = new Edge(newNode, edge.EndNode);
            // 将包含同一边的两个三角形进行最大最小角判别
            List<Node> temp = Triangle.FindNodesOfTrianglesContainEdge(edge, triangles);
            if (temp != null && temp.Count > 0)
                MinAngleMustBeMax(temp[0], edge.StartNode, newNode, edge.EndNode, edges, triangles);
            else
            {
                if (edge_1.FindExistIn_ByNodeId(edges) == null)
                    edges.Add(edge_1);

                if (edge_2.FindExistIn_ByNodeId(edges) == null)
                    edges.Add(edge_2);

                triangles.Add(new Triangle(edge.StartNode, edge.EndNode, newNode));
            }
            CreateTIN(edge_1, nodePool, nodes, edges, triangles);
            CreateTIN(edge_2, nodePool, nodes, edges, triangles);
        }

        private void MinCircumCircle(Edge edge, List<Node> nodePool, List<Node> nodeScope, ref Node newNode, Core.RelPointAndLine rel)
        {
            foreach (var item in nodePool)
            {
                Core.RelPointAndLine relTemp = Core.SpatialAnalysis.RelationshipOfPointAndLine(
                    new Core.SimpleLine(edge.StartNode.Position, edge.EndNode.Position), item.Position);
                if (relTemp != rel)
                    continue;
                List<Node> oldPool = CopyObjectList(nodeScope);
                oldPool.Remove(edge.StartNode);
                oldPool.Remove(edge.EndNode);
                oldPool.Remove(item);
                Core.Point center = null;
                double radius = 0;
                ThreePointsToCircle(edge.StartNode.Position, edge.EndNode.Position, item.Position, ref center, ref radius);
                if (center == null)
                {
                    int a = 0;
                    ThreePointsToCircle(edge.StartNode.Position, edge.EndNode.Position, item.Position, ref center, ref radius);
                }
                int label = 0;
                foreach (var item2 in oldPool)
                {
                    double distance = center.DistanceWith(item2.Position);
                    if (distance - radius < -0.0000001)
                    {
                        label = -1;
                        break;
                    }
                }
                if (label == 0)
                {
                    newNode = item;
                    break;
                }
            }
        }

        // 用于生成两个更稳定的三角形，及其对应的五条边
        private void MinAngleMustBeMax(Node n1, Node n2, Node n3, Node n4, List<Edge> edges, List<Triangle> triangles)
        {
            Edge e1 = new Edge(n1, n2);
            if (e1.FindExistIn_ByNodeId(edges) == null)
                edges.Add(e1);

            Edge e2 = new Edge(n2, n3);
            if (e2.FindExistIn_ByNodeId(edges) == null)
                edges.Add(e2);

            Edge e3 = new Edge(n3, n4);
            if (e3.FindExistIn_ByNodeId(edges) == null)
                edges.Add(e3);

            Edge e4 = new Edge(n4, n1);
            if (e4.FindExistIn_ByNodeId(edges) == null)
                edges.Add(e4);

            Edge e5 = new Edge(n1, n3);
            Edge e6 = new Edge(n2, n4);

            // 必须满足空外接圆法则
            // 正对角线的两个三角形
            Triangle t1 = new Triangle(n1, n2, n4);
            if (t1.IsCircumCircleContain(n3.Position))
                return;
            double sinAngle1 = (t1.MinEdgeLength / (2 * t1.CircumCircleRadius));
            Triangle t2 = new Triangle(n3, n2, n4);
            if (t2.IsCircumCircleContain(n1.Position))
                return;
            double sinAngle2 = (t2.MinEdgeLength / (2 * t2.CircumCircleRadius));
            // 反对角线的两个三角形
            Triangle t3 = new Triangle(n1, n2, n3);
            if (t3.IsCircumCircleContain(n4.Position))
                return;
            double sinAngle3 = (t3.MinEdgeLength / (2 * t3.CircumCircleRadius));
            Triangle t4 = new Triangle(n1, n4, n3);
            if (t4.IsCircumCircleContain(n2.Position))
                return;
            double sinAngle4 = (t4.MinEdgeLength / (2 * t4.CircumCircleRadius));

            double winner = (sinAngle1 < sinAngle2 ? sinAngle1 : sinAngle2) > (sinAngle3 < sinAngle4 ? sinAngle3 : sinAngle4)
                ? (sinAngle1 < sinAngle2 ? sinAngle1 : sinAngle2) : (sinAngle3 < sinAngle4 ? sinAngle3 : sinAngle4);

            if (Equals(winner, sinAngle1) || Equals(winner, sinAngle2))
            {
                if (e6.FindExistIn_ByNodeId(edges) == null)
                    edges.Add(e6);
                Edge tempE5 = e5.FindExistIn_ByNodeId(edges);
                if (tempE5 != null)
                    edges.Remove(tempE5);
                if (t1.FindExistIn_ByNodeId(triangles) == null)
                    triangles.Add(t1);
                if (t2.FindExistIn_ByNodeId(triangles) == null)
                    triangles.Add(t2);
                Triangle tempT3 = t1.FindExistIn_ByNodeId(triangles);
                if (tempT3 != null)
                    triangles.Remove(t3);
                Triangle tempT4 = t1.FindExistIn_ByNodeId(triangles);
                if (tempT4 != null)
                    triangles.Remove(t4);
            }
            else if (Equals(winner, sinAngle3) || Equals(winner, sinAngle4))
            {
                if (e5.FindExistIn_ByNodeId(edges) == null)
                    edges.Add(e5);
                Edge tempE6 = e6.FindExistIn_ByNodeId(edges);
                if (tempE6 != null)
                    edges.Remove(tempE6);
                if (t3.FindExistIn_ByNodeId(triangles) == null)
                    triangles.Add(t3);
                if (t4.FindExistIn_ByNodeId(triangles) == null)
                    triangles.Add(t4);
                Triangle tempT1 = t1.FindExistIn_ByNodeId(triangles);
                if (tempT1 != null)
                    triangles.Remove(tempT1);
                Triangle tempT2 = t2.FindExistIn_ByNodeId(triangles);
                if (tempT2 != null)
                    triangles.Remove(t2);
            }
        }

        private void ThreePointsToCircle(Core.Point p1, Core.Point p2, Core.Point p3, ref Core.Point center, ref double radius)
        {
            Core.SimpleLine line1 = new Core.SimpleLine(p1, p2);
            Core.SimpleLine line2 = new Core.SimpleLine(p1, p3);
            Core.Point point = Core.SpatialAnalysis.TwoLineOfIntersectPoint(line1.Midperpendicular, line2.Midperpendicular);
            if (point != null)
            {
                center = point;
                radius = center.DistanceWith(p1);
            }
        }

        private bool IsConvexContainNodeOfPool(List<Edge> edges, List<Node> nodePool)
        {
            if (edges.Count >= 3)
            {
                for (int j = 0; j < nodePool.Count; j++)
                {
                    int num = 0;
                    for (int i = 0; i < edges.Count; i++)
                    {
                        if (Core.SpatialAnalysis.RelationshipOfPointAndLine(
                            new Core.SimpleLine(edges[i].StartNode.Position, edges[i].EndNode.Position), nodePool[j].Position)
                            == Core.RelPointAndLine.LineRight)
                            num += 1;
                    }
                    if (num == edges.Count)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
