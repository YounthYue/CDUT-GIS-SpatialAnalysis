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
    public partial class NetworkAttribute : Form
    {
        private BaseNetwork network;
        private MainForm form1;
        public NetworkAttribute(MainForm form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        public NetworkAttribute(BaseNetwork network)
        {
            InitializeComponent();
            this.network = network;
        }

        public DataGridView NodeDataGridView
        {
            get
            {
                return this.dataGridView1;
            }
        }

        public DataGridView EdgeDataGridView
        {
            get
            {
                return this.dataGridView2;
            }
        }

        private void NetworkAttribute_Load(object sender, EventArgs e)
        {
            // 初始化表格的字段
            InitDataGrid();
            // 加载网络属性表
            LoadSimpleNetwork();
            this.生成网络结构ToolStripMenuItem.Enabled = false;
        }

        private void InitDataGrid()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "NodeId";
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[0].ValueType = System.Type.GetType("INT32");
            dataGridView1.Columns[1].Name = "NodeName";
            dataGridView1.Columns[1].ValueType = System.Type.GetType("String");
            dataGridView1.Columns[2].Name = "NodeValue";
            dataGridView1.Columns[2].ValueType = System.Type.GetType("Double");
            dataGridView1.Columns[3].Name = "Position";
            dataGridView1.Columns[3].ValueType = System.Type.GetType("System.Drawing.Point");


            dataGridView2.ColumnCount = 5;
            dataGridView2.Columns[0].Name = "EdgeId";
            dataGridView2.Columns[0].ValueType = System.Type.GetType("INT32");
            dataGridView2.Columns[0].Width = 50;
            dataGridView2.Columns[1].Name = "EdgeName";
            dataGridView2.Columns[1].ValueType = System.Type.GetType("String");
            dataGridView2.Columns[2].Name = "EdgeValue";
            dataGridView2.Columns[2].ValueType = System.Type.GetType("Double");
            dataGridView2.Columns[3].Name = "StartNodeId";
            dataGridView2.Columns[3].ValueType = System.Type.GetType("INT32");
            dataGridView2.Columns[4].Name = "EndNodeId";
            dataGridView2.Columns[4].ValueType = System.Type.GetType("INT32");
        }

        private void 生成网络结构ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 将数据表生成网络
            List<Node> nodes = new List<Node>();
            // add node
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                nodes.Add(new Network.Node(
                    Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value),
                    (Core.Point)dataGridView1.Rows[i].Cells[3].Value));
            }
            List<Edge> edges = new List<Edge>();
            // add edge
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                edges.Add(new Network.Edge(
                    Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value),
                    Convert.ToDouble(dataGridView2.Rows[i].Cells[2].Value),
                    Convert.ToInt32(dataGridView2.Rows[i].Cells[3].Value),
                    Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value),
                    nodes));
            }
            if (nodes.Count > 0 && edges.Count > 0)
                network = new SimpleNetwork(nodes, edges);
            if (network != null && network.Nodes.Count > 0 && network.Edges.Count > 0)
            {
                NetworkForm fm = new NetworkForm(network, this.form1);
                if (form1.DrawPen == DrawPenTpye.画网络)
                {
                    // do something

                    // 锁定属性表
                    dataGridView1.ReadOnly = true;
                    dataGridView2.ReadOnly = true;
                    form1.ShutDownPicBox();
                    form1.DrawPen = DrawPenTpye.关闭;
                }
                if (network is SimpleNetwork)
                    form1.SimpleNetwork = (SimpleNetwork)network;
                else if (network is TIN)
                    form1.TIN = (TIN)network;
                fm.Show();
                // 退出属性窗口
                this.Close();
                this.Dispose();
            }
        }

        // 加载SimpleNetwork的属性表
        private void LoadSimpleNetwork()
        {
            if (network != null)
            {
                // 加载结点数据
                if (network.Nodes.Count > 0)
                {
                    for (int i = 0; i < network.Nodes.Count; i++)
                    {
                        object[] values = new object[network.Nodes.Count];
                        values[0] = network.Nodes[i].NodeId;
                        values[1] = network.Nodes[i].NodeName;
                        values[2] = network.Nodes[i].NodeValue;
                        values[3] = network.Nodes[i].Position;
                        dataGridView1.Rows.Add(values);
                    }
                }
                // 加载边数据
                if (network.Nodes.Count > 0 && network.Edges.Count > 0)
                {
                    for (int i = 0; i < network.Edges.Count; i++)
                    {
                        object[] values = new object[network.Nodes.Count];
                        values[0] = network.Edges[i].EdgeId;
                        values[1] = network.Edges[i].EdgeName;
                        values[2] = network.Edges[i].EdgeValue;
                        values[3] = network.Edges[i].StartNodeId;
                        values[4] = network.Edges[i].EndNodeId;
                        dataGridView2.Rows.Add(values);
                    }
                }
            }
        }
    }
}