namespace SpatialAnalysis
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.绘制线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.裁剪ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空画板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.网络分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.采集结点与边数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开普通网络数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存网络数据集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tIN生成示意ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开TIN文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.凸包生成示意ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(668, 396);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制线ToolStripMenuItem,
            this.绘制面ToolStripMenuItem,
            this.裁剪ToolStripMenuItem,
            this.清空画板ToolStripMenuItem,
            this.网络分析ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(668, 30);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 绘制线ToolStripMenuItem
            // 
            this.绘制线ToolStripMenuItem.Name = "绘制线ToolStripMenuItem";
            this.绘制线ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.绘制线ToolStripMenuItem.Text = "绘制线";
            this.绘制线ToolStripMenuItem.Click += new System.EventHandler(this.绘制线ToolStripMenuItem_Click);
            // 
            // 绘制面ToolStripMenuItem
            // 
            this.绘制面ToolStripMenuItem.Name = "绘制面ToolStripMenuItem";
            this.绘制面ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.绘制面ToolStripMenuItem.Text = "绘制面";
            this.绘制面ToolStripMenuItem.Click += new System.EventHandler(this.绘制面ToolStripMenuItem_Click);
            // 
            // 裁剪ToolStripMenuItem
            // 
            this.裁剪ToolStripMenuItem.Name = "裁剪ToolStripMenuItem";
            this.裁剪ToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            this.裁剪ToolStripMenuItem.Text = "裁剪";
            this.裁剪ToolStripMenuItem.Click += new System.EventHandler(this.裁剪ToolStripMenuItem_Click);
            // 
            // 清空画板ToolStripMenuItem
            // 
            this.清空画板ToolStripMenuItem.Name = "清空画板ToolStripMenuItem";
            this.清空画板ToolStripMenuItem.Size = new System.Drawing.Size(101, 24);
            this.清空画板ToolStripMenuItem.Text = "清空画板";
            this.清空画板ToolStripMenuItem.Click += new System.EventHandler(this.清空画板ToolStripMenuItem_Click);
            // 
            // 网络分析ToolStripMenuItem
            // 
            this.网络分析ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.采集结点与边数据ToolStripMenuItem,
            this.打开普通网络数据ToolStripMenuItem,
            this.保存网络数据集ToolStripMenuItem,
            this.tIN生成示意ToolStripMenuItem,
            this.打开TIN文件ToolStripMenuItem,
            this.凸包生成示意ToolStripMenuItem});
            this.网络分析ToolStripMenuItem.Name = "网络分析ToolStripMenuItem";
            this.网络分析ToolStripMenuItem.Size = new System.Drawing.Size(101, 24);
            this.网络分析ToolStripMenuItem.Text = "网络分析";
            // 
            // 采集结点与边数据ToolStripMenuItem
            // 
            this.采集结点与边数据ToolStripMenuItem.Name = "采集结点与边数据ToolStripMenuItem";
            this.采集结点与边数据ToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.采集结点与边数据ToolStripMenuItem.Text = "采集结点与边数据";
            this.采集结点与边数据ToolStripMenuItem.Click += new System.EventHandler(this.采集结点与边数据ToolStripMenuItem_Click);
            // 
            // 打开普通网络数据ToolStripMenuItem
            // 
            this.打开普通网络数据ToolStripMenuItem.Name = "打开普通网络数据ToolStripMenuItem";
            this.打开普通网络数据ToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.打开普通网络数据ToolStripMenuItem.Text = "打开普通网络数据";
            this.打开普通网络数据ToolStripMenuItem.Click += new System.EventHandler(this.打开网络数据文件ToolStripMenuItem_Click);
            // 
            // 保存网络数据集ToolStripMenuItem
            // 
            this.保存网络数据集ToolStripMenuItem.Name = "保存网络数据集ToolStripMenuItem";
            this.保存网络数据集ToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.保存网络数据集ToolStripMenuItem.Text = "保存网络数据集";
            this.保存网络数据集ToolStripMenuItem.Click += new System.EventHandler(this.保存网络数据集ToolStripMenuItem_Click);
            // 
            // tIN生成示意ToolStripMenuItem
            // 
            this.tIN生成示意ToolStripMenuItem.Name = "tIN生成示意ToolStripMenuItem";
            this.tIN生成示意ToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.tIN生成示意ToolStripMenuItem.Text = "TIN生成示意";
            this.tIN生成示意ToolStripMenuItem.Click += new System.EventHandler(this.tIN生成示意ToolStripMenuItem_Click);
            // 
            // 打开TIN文件ToolStripMenuItem
            // 
            this.打开TIN文件ToolStripMenuItem.Name = "打开TIN文件ToolStripMenuItem";
            this.打开TIN文件ToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.打开TIN文件ToolStripMenuItem.Text = "打开TIN文件";
            this.打开TIN文件ToolStripMenuItem.Click += new System.EventHandler(this.打开TIN文件ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("仿宋", 10F);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 426);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(668, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(197, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // 凸包生成示意ToolStripMenuItem
            // 
            this.凸包生成示意ToolStripMenuItem.Name = "凸包生成示意ToolStripMenuItem";
            this.凸包生成示意ToolStripMenuItem.Size = new System.Drawing.Size(244, 26);
            this.凸包生成示意ToolStripMenuItem.Text = "凸包生成示意";
            this.凸包生成示意ToolStripMenuItem.Click += new System.EventHandler(this.凸包生成示意ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 448);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "MainForm";
            this.Text = "点击绘制线或绘制面开始绘制，右击画板绘制结束！";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 绘制线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 裁剪ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空画板ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 网络分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 采集结点与边数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开普通网络数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存网络数据集ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem tIN生成示意ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开TIN文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 凸包生成示意ToolStripMenuItem;
    }
}

