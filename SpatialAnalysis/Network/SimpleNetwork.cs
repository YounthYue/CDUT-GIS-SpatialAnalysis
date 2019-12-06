using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpatialAnalysis.Network
{
    [Serializable]    
    public class SimpleNetwork : BaseNetwork
    {
        
        private NetworkType networkType = NetworkType.SimpleNetwork;

        public new NetworkType NetworkType { get { return this.networkType; } }

        

       

        // 简单网络构造函数
        public SimpleNetwork() { }

        public SimpleNetwork(List<Node> nodes, List<Edge> edges)
        {
            // 检查拓扑关系(判断每条边的端点是否在结点集内)
            foreach (var item in edges)
            {
                int num = 0;
                foreach (var node in nodes)
                {
                    if (node.NodeId == item.EndNodeId || node.NodeId == item.StartNodeId)
                    {
                        num += 1;
                    }
                }
                if (num != 2)
                {
                    MessageBox.Show("拓扑关系不正确！", "系统提示");
                    return;
                }
            }
            // 此处检查编号是否完整
            // 边与结点现未提供传Id进行构造实体的函数

            // 若结点与边无编号，按顺序生成编号
            //if (nodes[0].NodeId < 1)
            //{
            //    for (int i = 0; i < nodes.Count; i++)
            //    {
            //        nodes[i].NodeId = i + 1;
            //    }
            //}
            //if (edges[0].EdgeId < 1)
            //{
            //    for (int i = 0; i < edges.Count; i++)
            //    {
            //        edges[i].EdgeId = i + 1;
            //    }
            //}
            // 为结点与边赋值
            this.Nodes = CopyObjectList(nodes);
            this.Edges = CopyObjectList(edges);
        }

        
        

        public void Store()
        {
            base.Store(this);
        }

        public override void Load()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "ntw文件(*.ntw)|*.ntw";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int position = open.FileName.LastIndexOf(@"\");
                string baseDir = open.FileName.Substring(0, position);
                string fileName = open.FileName.Substring(position + 1);
                Object obj = Core.IO.ReadDataToMemory(baseDir, fileName);
                SimpleNetwork newNetwork = null;
                if (obj is SimpleNetwork)
                    newNetwork = (Network.SimpleNetwork)obj;
                else
                    return;
                if (newNetwork == null)
                    return;
                this.Nodes = this.CopyObjectList(newNetwork.Nodes);
                this.Edges = this.CopyObjectList(newNetwork.Edges);
            }
        }

        public void Clear()
        {
            base.Clear();
        }

    }
}