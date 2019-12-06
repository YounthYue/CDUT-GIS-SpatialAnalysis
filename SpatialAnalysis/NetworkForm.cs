using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpatialAnalysis.Network;

namespace SpatialAnalysis
{
    public partial class NetworkForm : Form
    {
        private Network.BaseNetwork network;
        private MainForm form1;
        private List<Node> nodes;
        public NetworkForm(Network.BaseNetwork network, MainForm From1)
        {
            InitializeComponent();
            this.network = network;
            this.form1 = From1;
            this.nodes = network.Nodes;
        }

        private void Network_Load(object sender, EventArgs e)
        {
            LoadNetwork();
        }

        private void LoadNetwork()
        {
            // load network
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            LoadFullNode(comboBox1);
            LoadFullNode(comboBox2);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void LoadFullNode(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            for (int i = 0; i < nodes.Count; i++)
            {
                comboBox.Items.Add("Node" + nodes[i].NodeId);
            }
        }

        private void LoadExceptNode(ComboBox comboBox, object excludedNode)
        {
            LoadFullNode(comboBox);
            comboBox.Items.Remove(excludedNode);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1.Graph = form1.PictureBox.CreateGraphics();
            DrawNetwork(form1.Graph);
            int sId = Convert.ToInt32((Convert.ToString(comboBox1.SelectedItem).Substring(4)));
            int eId = Convert.ToInt32((Convert.ToString(comboBox2.SelectedItem).Substring(4)));
            DrawNode(form1.Graph, sId);
            DrawNode(form1.Graph, eId);
            DrawPath(form1.Graph, sId, eId);
            form1.ShutDownPicBox();
            
        }

        private void DrawNode(Graphics graph, int nodeId)
        {
            graph.FillEllipse(
                Brushes.Orange, 
                (int)(nodes[nodeId - 1].Position.X - 5), 
                (int)(form1.PicBoxHeight - nodes[nodeId -1].Position.Y - 5),
                10,
                10);
        }

        private void DrawPath(Graphics graph, int sId, int eId)
        {
            List<Node> path = null;
            double cost = 0.0;
            network.Floyd(network, sId, eId, ref path, ref cost);
            if (path == null)
                return;
            System.Drawing.Point[] drawPoints = new Point[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                drawPoints[i] = new Point((int)path[i].Position.X, (int)(form1.PicBoxHeight - path[i].Position.Y));
            }
            Pen pen = new Pen(Brushes.Blue, 3);
            graph.DrawLines(pen, drawPoints);
            form1.ToolStripLabel.Text = "总计耗费为：" + cost + "。";
        }

        private void DrawNetwork(Graphics graph)
        {
            graph.Clear(form1.PictureBox.BackColor);
            // draw node
            for (int i = 0; i < network.Nodes.Count; i++)
            {
                System.Drawing.Point p = new Point((int)network.Nodes[i].Position.X, (int)(form1.PicBoxHeight - network.Nodes[i].Position.Y));
                graph.FillEllipse(Brushes.Red, p.X - 1, p.Y - 1, 2, 2);
                graph.DrawString(
                    "Node" + network.Nodes[i].NodeId,
                    form1.Font,
                    Brushes.Black,
                    new System.Drawing.RectangleF((int)network.Nodes[i].Position.X, (int)(form1.PicBoxHeight - network.Nodes[i].Position.Y),
                    60,
                    300));
            }
            // draw edge
            for (int i = 0; i < network.Edges.Count; i++)
            {
                System.Drawing.Point p1 = new Point((int)network.Edges[i].StartNode.Position.X, (int)(form1.PicBoxHeight - network.Edges[i].StartNode.Position.Y));
                System.Drawing.Point p2 = new Point((int)network.Edges[i].EndNode.Position.X, (int)(form1.PicBoxHeight - network.Edges[i].EndNode.Position.Y));
                graph.DrawLine(Pens.Red, p1, p2);
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (network != null)
                if (network is SimpleNetwork)
                    ((SimpleNetwork)network).Store();
                else if (network is TIN)
                    ((TIN)network).Store();
        }

        private void 加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (network == null)
                network = new Network.SimpleNetwork();
            network.Load();
            if (network == null)
                return;
            form1.InitPicBox();
            network.Show(form1.Graph, form1.PictureBox, form1.PicBoxHeight);
            form1.ShutDownPicBox();
            LoadNetwork();
        }

        private void 打开属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkAttribute netAtt = new NetworkAttribute(network);
            netAtt.Show();
        }
    }
}
