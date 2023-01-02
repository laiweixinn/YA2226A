namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class IOlcyCtrl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tablelayout1 = new LZ.CNC.Measurement.Forms.Controls.Tablelayout();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.tablelayout1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tablelayout1
            // 
            this.tablelayout1.BackColor = System.Drawing.Color.Silver;
            this.tablelayout1.BorderColor = System.Drawing.Color.White;
            this.tablelayout1.ColumnCount = 2;
            this.tablelayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tablelayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablelayout1.Controls.Add(this.panel1, 0, 0);
            this.tablelayout1.Controls.Add(this.button1, 1, 0);
            this.tablelayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablelayout1.Location = new System.Drawing.Point(0, 0);
            this.tablelayout1.Name = "tablelayout1";
            this.tablelayout1.RowCount = 1;
            this.tablelayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablelayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tablelayout1.Size = new System.Drawing.Size(206, 34);
            this.tablelayout1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(21, 28);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Silver;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(30, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "平台真空吸";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // IOlcyCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tablelayout1);
            this.Name = "IOlcyCtrl";
            this.Size = new System.Drawing.Size(206, 34);
            this.Load += new System.EventHandler(this.IOlcyCtrl_Load);
            this.tablelayout1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Tablelayout tablelayout1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
    }
}
