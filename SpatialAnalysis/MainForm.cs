using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpatialAnalysis.Core;

namespace SpatialAnalysis
{
    public enum DrawPenTpye
    {
        画线,
        画面,
        画网络,
        画TIN,
        关闭
    }
    public partial class MainForm : Form
    {
        // 画板高度最高值
        public int PicBoxHeight;
        // 画板等工具初始属性值
        public Graphics Graph;
        private Pen pen;
        private Brush brush;
        private Font font;

        public PictureBox PictureBox
        {
            get
            {
                return this.pictureBox1;
            }
        }

        public ToolStripLabel ToolStripLabel
        {
            get { return this.toolStripStatusLabel1; }
        }

        // 画板开启标志
        public DrawPenTpye DrawPen = DrawPenTpye.关闭;

        // 网络数据集属性表
        private NetworkAttribute netForm;
        // 简单网络数据集
        private Network.SimpleNetwork simpleNetwork;
        // TIN数据集
        private Network.TIN tin;

        public Network.SimpleNetwork SimpleNetwork
        {
            set { this.simpleNetwork = value; }
            get { return this.simpleNetwork; }
        }

        public Network.TIN TIN
        {
            set { this.tin = value; }
            get { return this.tin; }
        }

        // 点集
        private List<System.Drawing.Point> list = new List<System.Drawing.Point>();
        private System.Drawing.Point[] drawPolylinePoints;
        private System.Drawing.Point[] drawPolygonPoints;

        // 裁剪算法的全局变量
        // 折线与多边形的所有交点
        private List<Core.Point> polylinIntersectPoints = new List<Core.Point>();
        // 每条线段与多边形的交点
        private List<double> alphaOfPosition = new List<double>();
        // 每条线段上的交点位置
        private List<double> alphaOfsimpleLineLabel = new List<double>();
        // 与多边形有交点的线段集合
        private List<int> simpleLineLabel = new List<int>();
        // 准备好的直角坐标系的交点
        private List<Core.Point> preparedPoints = new List<Core.Point>();
        // 准备好的屏幕坐标系的交点
        private List<System.Drawing.Point> resultPoints = new List<System.Drawing.Point>();
        private System.Drawing.Point[] drawResultPoints;


        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PicBoxHeight = pictureBox1.Height;
            toolStripStatusLabel1.Text = "Welcome!";
        }

        public void InitPicBox()
        {
            Graph = pictureBox1.CreateGraphics();
            pen = Pens.Red;
            brush = Brushes.Yellow;
        }

        public void ShutDownPicBox()
        {
            Graph.Dispose();
            list.Clear();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (DrawPen != DrawPenTpye.关闭)
                {
                    list.Add(new System.Drawing.Point(e.X, e.Y));
                    Graph.FillEllipse(Brushes.Red, e.X - 1, e.Y - 1, 2, 2);
                    if (list.Count >= 2 && (DrawPen == DrawPenTpye.画线 || DrawPen == DrawPenTpye.画面))
                    {
                        if (drawPolylinePoints != null && drawPolylinePoints.Length > 0)
                            Array.Clear(drawPolylinePoints, 0, drawPolylinePoints.Length);
                        drawPolylinePoints = list.ToArray();
                        Graph.DrawLines(pen, drawPolylinePoints);
                    }
                    if (DrawPen == DrawPenTpye.画网络)
                    {
                        // 数据行加一
                        netForm.NodeDataGridView.Rows.Add();
                        int index = netForm.NodeDataGridView.Rows.Count - 2;
                        object[] values = new object[netForm.NodeDataGridView.ColumnCount];
                        values[0] = list.Count;
                        values[1] = "Node" + list.Count;
                        values[3] = new Core.Point(e.X, PicBoxHeight - e.Y);
                        netForm.NodeDataGridView.Rows[index].SetValues(values);
                        // 在画板上画出对应位置
                        Graph.DrawString(Convert.ToString(values[1]), this.Font, Brushes.Black, new System.Drawing.RectangleF(e.X, e.Y, 60, 300));

                        // 添加全联通边
                        // AddFullConnectedEdge();
                        // 添加部分边
                        // AddPartConnectedEdge();
                    }
                    if (DrawPen == DrawPenTpye.画TIN)
                    {
                        List<Network.Node> nodes = new List<Network.Node>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            nodes.Add(new Network.Node(i + 1, new Core.Point(list[i].X, PicBoxHeight - list[i].Y)));
                        }
                        // MidLineTest(nodes);
                        TIN = new Network.TIN(nodes);
                        TIN.Show(Graph, pictureBox1, PicBoxHeight);
                    }
                }
                else
                {
                    MessageBox.Show("请点击绘制按钮，然后开始绘制！");
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (DrawPen == DrawPenTpye.画线)
                {
                    if (list.Count < 2)
                    {
                        MessageBox.Show("绘制的点不足两个，无法绘制线！");
                        ShutDownPicBox();
                        DrawPen = DrawPenTpye.关闭;
                        清空画板ToolStripMenuItem_Click(new object(), new EventArgs());
                        return;
                    }
                    ShutDownPicBox();
                    MessageBox.Show("线绘制完成！");
                }
                if (DrawPen == DrawPenTpye.画面)
                {
                    if (list.Count < 3)
                    {
                        MessageBox.Show("绘制的点不足三个，无法绘制面！");
                        ShutDownPicBox();
                        DrawPen = DrawPenTpye.关闭;
                        清空画板ToolStripMenuItem_Click(new object(), new EventArgs());
                        return;
                    }
                    drawPolygonPoints = list.ToArray();
                    Graph.FillPolygon(brush, drawPolygonPoints);
                    ShutDownPicBox();
                    MessageBox.Show("面绘制完成！");
                }
                if (DrawPen == DrawPenTpye.画网络)
                {
                    // do something

                    // 锁定属性表
                    netForm.NodeDataGridView.ReadOnly = true;
                    netForm.EdgeDataGridView.ReadOnly = true;
                    ShutDownPicBox();
                    MessageBox.Show("结点与边采集完成！");
                }
                if (DrawPen == DrawPenTpye.画TIN)
                {
                    ShutDownPicBox();
                    MessageBox.Show("TIN示意完成！");
                }
                DrawPen = DrawPenTpye.关闭;
            }
        }

        private void MidLineTest(List<Network.Node> nodes)
        {
            if (list.Count == 3)
            {
                Core.SimpleLine line1 = new Core.SimpleLine(nodes[0].Position, nodes[1].Position);
                Core.SimpleLine line2 = new Core.SimpleLine(nodes[0].Position, nodes[2].Position);

                Graph.DrawLine(
                    Pens.Blue,
                    new System.Drawing.Point((int)line1.StartPoint.X, (int)(PicBoxHeight - line1.StartPoint.Y)),
                    new System.Drawing.Point((int)line1.EndPoint.X, (int)(PicBoxHeight - line1.EndPoint.Y)));

                Graph.DrawLine(
                    Pens.Blue,
                    new System.Drawing.Point((int)line2.StartPoint.X, (int)(PicBoxHeight - line2.StartPoint.Y)),
                    new System.Drawing.Point((int)line2.EndPoint.X, (int)(PicBoxHeight - line2.EndPoint.Y)));

                SimpleLine lineM1 = line1.Midperpendicular;
                SimpleLine lineM2 = line2.Midperpendicular;
                Graph.DrawLine(
                    Pens.Red,
                    new System.Drawing.Point((int)lineM1.StartPoint.X, (int)(PicBoxHeight - lineM1.StartPoint.Y)),
                    new System.Drawing.Point((int)lineM1.EndPoint.X, (int)(PicBoxHeight - lineM1.EndPoint.Y)));

                Graph.DrawLine(
                    Pens.Red,
                    new System.Drawing.Point((int)lineM2.StartPoint.X, (int)(PicBoxHeight - lineM2.StartPoint.Y)),
                    new System.Drawing.Point((int)lineM2.EndPoint.X, (int)(PicBoxHeight - lineM2.EndPoint.Y)));
                Core.Result.IntersectOfLines res = Core.SpatialAnalysis.TwoSimpleLineOfIntersectPoint(lineM1, lineM2);
                if (res.intersectPoint != null)
                {
                    Core.Point center = res.intersectPoint;
                    Graph.FillEllipse(Brushes.Orange, (int)(center.X - 3), (int)(PicBoxHeight - center.Y - 3), 6, 6);
                }
            }
        }

        private void 绘制线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawPen != DrawPenTpye.画线 && DrawPen != DrawPenTpye.关闭)
            {
                MessageBox.Show("当前绘制完成，点击右键完成绘制！");
                return;
            }
            else if (DrawPen == DrawPenTpye.画线)
            {
                return;
            }
            else
            {
                DrawPen = DrawPenTpye.画线;
                InitPicBox();
            }
        }

        private void 绘制面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawPen != DrawPenTpye.画面 && DrawPen != DrawPenTpye.关闭)
            {
                MessageBox.Show("当前绘制完成，点击右键完成绘制！");
                return;
            }
            else if (DrawPen == DrawPenTpye.画面)
            {
                return;
            }
            else
            {
                DrawPen = DrawPenTpye.画面;
                InitPicBox();
            }
        }

        private void 裁剪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawPolylinePoints != null && drawPolylinePoints.Length > 0
                && drawPolygonPoints != null && drawPolygonPoints.Length > 0)
            {
                // 生成polygon
                Core.Point[] polygonPoints = new Core.Point[drawPolygonPoints.Length];
                for (int i = 0; i < polygonPoints.Length; i++)
                {
                    polygonPoints[i] = new Core.Point(drawPolygonPoints[i].X, PicBoxHeight - drawPolygonPoints[i].Y);
                }
                Core.Polygon polygon = new Polygon(polygonPoints);
                // 生成polyline
                Core.Point[] polylinePoints = new Core.Point[drawPolylinePoints.Length];
                for (int i = 0; i < polylinePoints.Length; i++)
                {
                    polylinePoints[i] = new Core.Point(drawPolylinePoints[i].X, PicBoxHeight - drawPolylinePoints[i].Y);
                }
                Core.Polyline polyline = new Polyline(polylinePoints);
                // 计算所有交点，生成交点集
                for (int i = 0; i < polyline.simpleLines.Length; i++)
                {
                    Core.SimpleLine line1 = polyline.simpleLines[i];
                    if (Core.SpatialAnalysis.IsPolygonContainPoint(polygon, line1.StartPoint)
                        && Core.SpatialAnalysis.IsPolygonContainPoint(polygon, line1.EndPoint))
                    {
                        if (!simpleLineLabel.Contains(i))
                            simpleLineLabel.Add(i);
                    }
                    else
                    {
                        for (int j = 0; j < polygon.simpleLines.Length; j++)
                        {
                            // 多边形的边
                            SimpleLine line2 = polygon.simpleLines[j];
                            SimpleLine line3 = polygon.simpleLines[(j + 1) % polygon.simpleLines.Length];
                            SimpleLine line4 = polygon.simpleLines[(j + 2) % polygon.simpleLines.Length];
                            Core.Result.IntersectOfLines intersect = Core.SpatialAnalysis.TwoSimpleLineOfIntersectPoint(line1, line2);
                            if (intersect.intersectPoint != null)
                            {
                                double alpha = Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line2, intersect.intersectPoint);
                                if (alpha < 1 && alpha > 0)
                                {
                                    polylinIntersectPoints.Add(intersect.intersectPoint);
                                    alphaOfPosition.Add(Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line1, intersect.intersectPoint));
                                }
                                else if (Core.SpatialAnalysis.IsEqual(alpha, 1))
                                {
                                    // r1必定为在线上
                                    RelPointAndLine r1 = Core.SpatialAnalysis.RelationshipOfPointAndLine(line1, line2.StartPoint);
                                    // 注意范围
                                    RelPointAndLine r2 = Core.SpatialAnalysis.RelationshipOfPointAndLine(line1, line3.EndPoint);
                                    if (r1 == r2)
                                    {
                                        // 同边添加两个相同点
                                        polylinIntersectPoints.Add(intersect.intersectPoint);
                                        polylinIntersectPoints.Add(intersect.intersectPoint);
                                        alphaOfPosition.Add(Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line1, intersect.intersectPoint));
                                        alphaOfPosition.Add(Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line1, intersect.intersectPoint));
                                        // 跳过下一条边的判断
                                        j++;
                                    }
                                    else if (r2 == RelPointAndLine.LineOn)
                                    {
                                        // 判断三条边是否为凸包
                                        RelPointAndLine r3 = Core.SpatialAnalysis.RelationshipOfPointAndLine(line2, line3.EndPoint);
                                        // 注意范围
                                        RelPointAndLine r4 = Core.SpatialAnalysis.RelationshipOfPointAndLine(line3, line4.EndPoint);
                                        if (r3 == r4)
                                        {     // 凸包添加两个相同点
                                            polylinIntersectPoints.Add(line3.StartPoint);
                                            polylinIntersectPoints.Add(line3.EndPoint);
                                            alphaOfPosition.Add(Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line1, line3.StartPoint));
                                            alphaOfPosition.Add(Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line1, line3.EndPoint));
                                        }
                                        else
                                        {    // 否则添加一个点
                                            polylinIntersectPoints.Add(line3.EndPoint);
                                            alphaOfPosition.Add(Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line1, line3.EndPoint));
                                        }
                                        // 跳过下两条边的判断
                                        j += 2;
                                    }
                                    else
                                    {
                                        // 异边添加一个点
                                        polylinIntersectPoints.Add(intersect.intersectPoint);
                                        alphaOfPosition.Add(Core.SpatialAnalysis.PositionOfPointOnSimpleLine(line1, intersect.intersectPoint));
                                        // 跳过下一条边的判断
                                        j++;
                                    }
                                }
                                if (!simpleLineLabel.Contains(i))
                                    simpleLineLabel.Add(i);
                            }
                        }
                    }
                    polylinIntersectPoints.Add(new Core.Point(0, 0));
                    alphaOfPosition.Add(-1);
                }

                int position1 = 0;
                int position2 = 0;
                for (int i = 0; i < simpleLineLabel.Count; i++)
                {
                    // 添加交点到线要素顶点集，并将顶点排序
                    // 初始化
                    int lineLabel = simpleLineLabel[i];
                    // 提取一条线段与多边形的所有交点
                    for (int j = position1; j < polylinIntersectPoints.Count; j++)
                    {
                        if (alphaOfPosition[j] < 0)
                        {
                            // 线段的交点添加完成，开始添加顶点，若顶点不在多边形内，则不添加
                            if (Core.SpatialAnalysis.IsPolygonContainPoint(polygon, polyline.simpleLines[lineLabel].StartPoint))
                            {
                                preparedPoints.Insert(position2, polyline.simpleLines[lineLabel].StartPoint);
                                alphaOfsimpleLineLabel.Insert(position2, 0);
                            }
                            if (Core.SpatialAnalysis.IsPolygonContainPoint(polygon, polyline.simpleLines[lineLabel].EndPoint))
                            {
                                preparedPoints.Add(polyline.simpleLines[lineLabel].EndPoint);
                                alphaOfsimpleLineLabel.Add(1);
                            }
                            position1 = j + 1;
                            position2 = preparedPoints.Count;
                            break;
                        }
                        // 按从小到大的顺序添加线段的交点
                        if (alphaOfsimpleLineLabel.Count == position2)
                        {
                            preparedPoints.Add(polylinIntersectPoints[position1]);
                            alphaOfsimpleLineLabel.Add(alphaOfPosition[position1]);
                        }
                        else
                        {
                            int insertIndex = position2;
                            for (int label = position2; label <= alphaOfsimpleLineLabel.Count; label++)
                            {
                                if (label == alphaOfsimpleLineLabel.Count)
                                    break;
                                // 从小到大排序
                                if (alphaOfsimpleLineLabel[label] < alphaOfPosition[j])
                                    insertIndex += 1;
                                else
                                    break;
                            }
                            if (insertIndex == 1)
                            {
                                int a = 1;
                            }
                            if (insertIndex == preparedPoints.Count)
                            {
                                preparedPoints.Add(polylinIntersectPoints[j]);
                                alphaOfsimpleLineLabel.Add(alphaOfPosition[j]);
                            }
                            else
                            {
                                preparedPoints.Insert(insertIndex, polylinIntersectPoints[j]);
                                alphaOfsimpleLineLabel.Insert(insertIndex, alphaOfPosition[j]);
                            }
                        }
                    }

                }

                // 根据保留的顶点获取裁剪后得到的线段集
                for (int i = 0; i < preparedPoints.Count; i++)
                {
                    resultPoints.Add(new System.Drawing.Point(
                        Convert.ToInt32(preparedPoints[i].X),
                        Convert.ToInt32(PicBoxHeight - preparedPoints[i].Y)));
                }
                drawResultPoints = resultPoints.ToArray();
                // 绘制结果
                InitPicBox();
                Graph.Clear(this.pictureBox1.BackColor);
                Graph.FillPolygon(brush, drawPolygonPoints);
                for (int i = 0; i < drawResultPoints.Length - 1; )
                {
                    Graph.DrawLine(pen, drawResultPoints[i], drawResultPoints[i + 1]);
                    i += 2;
                }
                ShutDownPicBox();

                // 清空所有数据
                if (polylinIntersectPoints.Count > 0)
                    polylinIntersectPoints.Clear();
                if (alphaOfPosition.Count > 0)
                    alphaOfPosition.Clear();
                if (alphaOfsimpleLineLabel.Count > 0)
                    alphaOfsimpleLineLabel.Clear();
                if (simpleLineLabel.Count > 0)
                    simpleLineLabel.Clear();
                if (simpleLineLabel.Count > 0)
                    simpleLineLabel.Clear();
                if (preparedPoints.Count > 0)
                    preparedPoints.Clear();
                if (resultPoints.Count > 0)
                    resultPoints.Clear();
                if (drawResultPoints.Length > 0)
                    Array.Clear(drawResultPoints, 0, drawResultPoints.Length);
            }
        }

        private void 清空画板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawPen != DrawPenTpye.关闭)
            {
                MessageBox.Show("需要先完成绘制，单击右键完成绘制！");
                return;
            }
            if (drawPolylinePoints != null && drawPolylinePoints.Length > 0)
                Array.Clear(drawPolylinePoints, 0, drawPolylinePoints.Length);
            if (drawPolygonPoints != null && drawPolygonPoints.Length > 0)
                Array.Clear(drawPolygonPoints, 0, drawPolygonPoints.Length);

            InitPicBox();
            Graph.Clear(this.pictureBox1.BackColor);
            ShutDownPicBox();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InitPicBox();
            Graph.Clear(this.pictureBox1.BackColor);
            list.Add(new System.Drawing.Point(20, 500));
            list.Add(new System.Drawing.Point(900, 400));
            list.Add(new System.Drawing.Point(100, 200));
            drawPolylinePoints = list.ToArray();
            list.Clear();
            list.Add(new System.Drawing.Point(400, 100));
            list.Add(new System.Drawing.Point(800, 800));
            list.Add(new System.Drawing.Point(200, 400));
            drawPolygonPoints = list.ToArray();
            list.Clear();
            Graph.FillPolygon(Brushes.Yellow, drawPolygonPoints);
            Graph.DrawLines(pen, drawPolylinePoints);
            ShutDownPicBox();
        }

        private void 采集结点与边数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawPen != DrawPenTpye.画网络 && DrawPen != DrawPenTpye.关闭)
            {
                MessageBox.Show("当前绘制完成，点击右键完成绘制！");
                return;
            }
            else if (DrawPen == DrawPenTpye.画网络)
            {
                return;
            }
            else
            {
                netForm = new NetworkAttribute(this);
                netForm.Show();
                DrawPen = DrawPenTpye.画网络;
                InitPicBox();
            }
        }

        private void AddFullConnectedEdge()
        {
            if (list.Count > 1)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    // 数据行加一
                    netForm.EdgeDataGridView.Rows.Add();
                    int index = netForm.EdgeDataGridView.Rows.Count - 2;
                    object[] values = new object[netForm.EdgeDataGridView.ColumnCount];
                    values[0] = index + 1;
                    double deltaX = list[i].X - list[list.Count - 1].X;
                    double deltaY = list[i].Y - list[list.Count - 1].Y;

                    double square = deltaX * deltaX + deltaY * deltaY;
                    values[2] = System.Math.Sqrt(square);
                    values[3] = i + 1;
                    values[4] = list.Count;

                    netForm.EdgeDataGridView.Rows[index].SetValues(values);

                    // 在画板上画出对应位置
                    Graph.DrawLine(pen, list[i], list[list.Count - 1]);
                }
            }
        }

        private void AddPartConnectedEdge()
        {
            if (list.Count > 1)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    // 数据行加一
                    if (i % 3 == 0)
                    {
                        netForm.EdgeDataGridView.Rows.Add();
                        int index = netForm.EdgeDataGridView.Rows.Count - 2;
                        object[] values = new object[netForm.EdgeDataGridView.ColumnCount];
                        values[0] = index + 1;
                        double deltaX = list[i].X - list[list.Count - 1].X;
                        double deltaY = list[i].Y - list[list.Count - 1].Y;

                        double square = deltaX * deltaX + deltaY * deltaY;
                        values[2] = System.Math.Sqrt(square);
                        values[3] = i + 1;
                        values[4] = list.Count;

                        netForm.EdgeDataGridView.Rows[index].SetValues(values);

                        // 在画板上画出对应位置
                        Graph.DrawLine(pen, list[i], list[list.Count - 1]);
                    }
                }
            }
        }

        private void 打开网络数据文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (simpleNetwork == null)
                simpleNetwork = new Network.SimpleNetwork();
            simpleNetwork.Load();
            if (simpleNetwork.Nodes == null)
                return;
            InitPicBox();
            simpleNetwork.Show(this.Graph, this.pictureBox1, this.PicBoxHeight);
            ShutDownPicBox();
            NetworkForm fm = new NetworkForm(simpleNetwork, this);
            fm.Show();
        }

        private void 保存网络数据集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (simpleNetwork != null)
                simpleNetwork.Store();
            else if (tin != null)
                tin.Store();
        }

        private void tIN生成示意ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawPen != DrawPenTpye.画TIN && DrawPen != DrawPenTpye.关闭)
            {
                MessageBox.Show("当前绘制完成，点击右键完成绘制！");
                return;
            }
            else if (DrawPen == DrawPenTpye.画TIN)
            {
                return;
            }
            else
            {
                //netForm = new NetworkAttribute(this);
                //netForm.Show();
                DrawPen = DrawPenTpye.画TIN;
                InitPicBox();
            }
        }

        private void 打开TIN文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tin == null)
                tin = new Network.TIN();
            tin.Load();
            if (tin.Nodes == null)
                return;
            InitPicBox();
            tin.Show(this.Graph, this.pictureBox1, this.PicBoxHeight);
            ShutDownPicBox();
            NetworkForm fm = new NetworkForm(tin, this);
            fm.Show();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.PicBoxHeight = this.pictureBox1.Height;
        }
    }
}
