namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class AxisDebug
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
            this.num_dist = new System.Windows.Forms.NumericUpDown();
            this.btn_positive = new System.Windows.Forms.Button();
            this.btn_movemode = new System.Windows.Forms.Button();
            this.btn_negative = new System.Windows.Forms.Button();
            this.cbo_speedmode = new System.Windows.Forms.ComboBox();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_gohome = new System.Windows.Forms.Button();
            this.lbl_position = new System.Windows.Forms.Label();
            this.lbl_axisname = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.num_dist)).BeginInit();
            this.SuspendLayout();
            // 
            // num_dist
            // 
            this.num_dist.DecimalPlaces = 2;
            this.num_dist.Font = new System.Drawing.Font("宋体", 12F);
            this.num_dist.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_dist.Location = new System.Drawing.Point(172, 8);
            this.num_dist.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_dist.Name = "num_dist";
            this.num_dist.Size = new System.Drawing.Size(65, 26);
            this.num_dist.TabIndex = 194;
            this.num_dist.Tag = "50";
            this.num_dist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_dist.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // btn_positive
            // 
            this.btn_positive.BackColor = System.Drawing.Color.LightGray;
            this.btn_positive.Location = new System.Drawing.Point(332, 5);
            this.btn_positive.Name = "btn_positive";
            this.btn_positive.Size = new System.Drawing.Size(36, 32);
            this.btn_positive.TabIndex = 193;
            this.btn_positive.Tag = "0";
            this.btn_positive.Text = "+";
            this.btn_positive.UseVisualStyleBackColor = false;
            this.btn_positive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_positive.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_positive.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_positive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_movemode
            // 
            this.btn_movemode.BackColor = System.Drawing.Color.LightGray;
            this.btn_movemode.Location = new System.Drawing.Point(278, 5);
            this.btn_movemode.Name = "btn_movemode";
            this.btn_movemode.Size = new System.Drawing.Size(51, 32);
            this.btn_movemode.TabIndex = 192;
            this.btn_movemode.Tag = "200";
            this.btn_movemode.Text = "连续";
            this.btn_movemode.UseVisualStyleBackColor = false;
            this.btn_movemode.Click += new System.EventHandler(this.btn_movemode_Click);
            // 
            // btn_negative
            // 
            this.btn_negative.BackColor = System.Drawing.Color.LightGray;
            this.btn_negative.Location = new System.Drawing.Point(241, 5);
            this.btn_negative.Name = "btn_negative";
            this.btn_negative.Size = new System.Drawing.Size(36, 32);
            this.btn_negative.TabIndex = 191;
            this.btn_negative.Tag = "0";
            this.btn_negative.Text = "-";
            this.btn_negative.UseVisualStyleBackColor = false;
            this.btn_negative.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_negative.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_negative.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_negative.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // cbo_speedmode
            // 
            this.cbo_speedmode.BackColor = System.Drawing.Color.Coral;
            this.cbo_speedmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_speedmode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbo_speedmode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_speedmode.FormattingEnabled = true;
            this.cbo_speedmode.IntegralHeight = false;
            this.cbo_speedmode.ItemHeight = 16;
            this.cbo_speedmode.Location = new System.Drawing.Point(372, 8);
            this.cbo_speedmode.Margin = new System.Windows.Forms.Padding(2);
            this.cbo_speedmode.Name = "cbo_speedmode";
            this.cbo_speedmode.Size = new System.Drawing.Size(70, 24);
            this.cbo_speedmode.TabIndex = 198;
            this.cbo_speedmode.SelectedIndexChanged += new System.EventHandler(this.cbo_speedmode_SelectedIndexChanged);
            // 
            // btn_stop
            // 
            this.btn_stop.BackColor = System.Drawing.Color.LightGray;
            this.btn_stop.Location = new System.Drawing.Point(449, 5);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(51, 32);
            this.btn_stop.TabIndex = 199;
            this.btn_stop.Tag = "200";
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = false;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_gohome
            // 
            this.btn_gohome.BackColor = System.Drawing.Color.LightGray;
            this.btn_gohome.Location = new System.Drawing.Point(504, 5);
            this.btn_gohome.Name = "btn_gohome";
            this.btn_gohome.Size = new System.Drawing.Size(51, 32);
            this.btn_gohome.TabIndex = 200;
            this.btn_gohome.Tag = "200";
            this.btn_gohome.Text = "回零";
            this.btn_gohome.UseVisualStyleBackColor = false;
            this.btn_gohome.Click += new System.EventHandler(this.btn_GoHome_Click);
            // 
            // lbl_position
            // 
            this.lbl_position.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbl_position.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_position.Location = new System.Drawing.Point(115, 5);
            this.lbl_position.Name = "lbl_position";
            this.lbl_position.Size = new System.Drawing.Size(54, 32);
            this.lbl_position.TabIndex = 201;
            this.lbl_position.Text = "1000.000";
            this.lbl_position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_axisname
            // 
            this.lbl_axisname.Location = new System.Drawing.Point(1, 8);
            this.lbl_axisname.Name = "lbl_axisname";
            this.lbl_axisname.Size = new System.Drawing.Size(108, 27);
            this.lbl_axisname.TabIndex = 202;
            this.lbl_axisname.Text = "轴名：###";
            this.lbl_axisname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AxisDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_axisname);
            this.Controls.Add(this.lbl_position);
            this.Controls.Add(this.btn_gohome);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.cbo_speedmode);
            this.Controls.Add(this.num_dist);
            this.Controls.Add(this.btn_positive);
            this.Controls.Add(this.btn_movemode);
            this.Controls.Add(this.btn_negative);
            this.Name = "AxisDebug";
            this.Size = new System.Drawing.Size(560, 42);
            this.Load += new System.EventHandler(this.AxisDebug_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_dist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown num_dist;
        private System.Windows.Forms.Button btn_positive;
        private System.Windows.Forms.Button btn_movemode;
        private System.Windows.Forms.Button btn_negative;
        private System.Windows.Forms.ComboBox cbo_speedmode;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Button btn_gohome;
        internal System.Windows.Forms.Label lbl_position;
        internal System.Windows.Forms.Label lbl_axisname;
    }
}
