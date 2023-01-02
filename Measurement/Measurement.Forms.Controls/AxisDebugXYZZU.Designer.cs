namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class AxisDebugXYZZU
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_uadd = new System.Windows.Forms.Button();
            this.btn_udec = new System.Windows.Forms.Button();
            this.btn_z2dec = new System.Windows.Forms.Button();
            this.btn_z2add = new System.Windows.Forms.Button();
            this.btn_yadd = new System.Windows.Forms.Button();
            this.cbo_movemode = new System.Windows.Forms.ComboBox();
            this.btn_zdec = new System.Windows.Forms.Button();
            this.cbo_speedmode = new System.Windows.Forms.ComboBox();
            this.btn_ydec = new System.Windows.Forms.Button();
            this.numtxt_dist = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.btn_xadd = new System.Windows.Forms.Button();
            this.btn_xdec = new System.Windows.Forms.Button();
            this.btn_zadd = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_uadd);
            this.groupBox1.Controls.Add(this.btn_udec);
            this.groupBox1.Controls.Add(this.btn_z2dec);
            this.groupBox1.Controls.Add(this.btn_z2add);
            this.groupBox1.Controls.Add(this.btn_yadd);
            this.groupBox1.Controls.Add(this.cbo_movemode);
            this.groupBox1.Controls.Add(this.btn_zdec);
            this.groupBox1.Controls.Add(this.cbo_speedmode);
            this.groupBox1.Controls.Add(this.btn_ydec);
            this.groupBox1.Controls.Add(this.numtxt_dist);
            this.groupBox1.Controls.Add(this.btn_xadd);
            this.groupBox1.Controls.Add(this.btn_xdec);
            this.groupBox1.Controls.Add(this.btn_zadd);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 221);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "XYZUV";
            // 
            // btn_uadd
            // 
            this.btn_uadd.BackColor = System.Drawing.Color.Gray;
            this.btn_uadd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_uadd.ForeColor = System.Drawing.Color.Black;
            this.btn_uadd.Location = new System.Drawing.Point(327, 71);
            this.btn_uadd.Name = "btn_uadd";
            this.btn_uadd.Size = new System.Drawing.Size(66, 42);
            this.btn_uadd.TabIndex = 46;
            this.btn_uadd.Text = "U+";
            this.btn_uadd.UseVisualStyleBackColor = false;
            this.btn_uadd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_uadd.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_uadd.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_uadd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_udec
            // 
            this.btn_udec.BackColor = System.Drawing.Color.Gray;
            this.btn_udec.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_udec.ForeColor = System.Drawing.Color.Black;
            this.btn_udec.Location = new System.Drawing.Point(242, 71);
            this.btn_udec.Name = "btn_udec";
            this.btn_udec.Size = new System.Drawing.Size(66, 42);
            this.btn_udec.TabIndex = 45;
            this.btn_udec.Text = "U-";
            this.btn_udec.UseVisualStyleBackColor = false;
            this.btn_udec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_udec.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_udec.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_udec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_z2dec
            // 
            this.btn_z2dec.BackColor = System.Drawing.Color.Gray;
            this.btn_z2dec.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_z2dec.ForeColor = System.Drawing.Color.Black;
            this.btn_z2dec.Location = new System.Drawing.Point(242, 128);
            this.btn_z2dec.Name = "btn_z2dec";
            this.btn_z2dec.Size = new System.Drawing.Size(66, 42);
            this.btn_z2dec.TabIndex = 44;
            this.btn_z2dec.Text = "V-";
            this.btn_z2dec.UseVisualStyleBackColor = false;
            this.btn_z2dec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_z2dec.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_z2dec.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_z2dec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_z2add
            // 
            this.btn_z2add.BackColor = System.Drawing.Color.Gray;
            this.btn_z2add.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_z2add.ForeColor = System.Drawing.Color.Black;
            this.btn_z2add.Location = new System.Drawing.Point(327, 128);
            this.btn_z2add.Name = "btn_z2add";
            this.btn_z2add.Size = new System.Drawing.Size(66, 42);
            this.btn_z2add.TabIndex = 43;
            this.btn_z2add.Text = "V+";
            this.btn_z2add.UseVisualStyleBackColor = false;
            this.btn_z2add.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_z2add.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_z2add.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_z2add.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_yadd
            // 
            this.btn_yadd.BackColor = System.Drawing.Color.Gray;
            this.btn_yadd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_yadd.ForeColor = System.Drawing.Color.Black;
            this.btn_yadd.Location = new System.Drawing.Point(83, 102);
            this.btn_yadd.Name = "btn_yadd";
            this.btn_yadd.Size = new System.Drawing.Size(66, 42);
            this.btn_yadd.TabIndex = 28;
            this.btn_yadd.Text = "Y+";
            this.btn_yadd.UseVisualStyleBackColor = false;
            this.btn_yadd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_yadd.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_yadd.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_yadd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // cbo_movemode
            // 
            this.cbo_movemode.BackColor = System.Drawing.Color.SandyBrown;
            this.cbo_movemode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_movemode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbo_movemode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_movemode.FormattingEnabled = true;
            this.cbo_movemode.IntegralHeight = false;
            this.cbo_movemode.ItemHeight = 16;
            this.cbo_movemode.Location = new System.Drawing.Point(190, 183);
            this.cbo_movemode.Margin = new System.Windows.Forms.Padding(2);
            this.cbo_movemode.Name = "cbo_movemode";
            this.cbo_movemode.Size = new System.Drawing.Size(82, 24);
            this.cbo_movemode.TabIndex = 38;
            this.cbo_movemode.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btn_zdec
            // 
            this.btn_zdec.BackColor = System.Drawing.Color.Gray;
            this.btn_zdec.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_zdec.ForeColor = System.Drawing.Color.Black;
            this.btn_zdec.Location = new System.Drawing.Point(242, 20);
            this.btn_zdec.Name = "btn_zdec";
            this.btn_zdec.Size = new System.Drawing.Size(66, 42);
            this.btn_zdec.TabIndex = 42;
            this.btn_zdec.Text = "Z-";
            this.btn_zdec.UseVisualStyleBackColor = false;
            this.btn_zdec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_zdec.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_zdec.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_zdec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // cbo_speedmode
            // 
            this.cbo_speedmode.BackColor = System.Drawing.Color.SandyBrown;
            this.cbo_speedmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_speedmode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbo_speedmode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_speedmode.FormattingEnabled = true;
            this.cbo_speedmode.IntegralHeight = false;
            this.cbo_speedmode.ItemHeight = 16;
            this.cbo_speedmode.Location = new System.Drawing.Point(104, 183);
            this.cbo_speedmode.Margin = new System.Windows.Forms.Padding(2);
            this.cbo_speedmode.Name = "cbo_speedmode";
            this.cbo_speedmode.Size = new System.Drawing.Size(82, 24);
            this.cbo_speedmode.TabIndex = 37;
            this.cbo_speedmode.SelectedIndexChanged += new System.EventHandler(this.cbo_speedmode_SelectedIndexChanged);
            // 
            // btn_ydec
            // 
            this.btn_ydec.BackColor = System.Drawing.Color.Gray;
            this.btn_ydec.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ydec.ForeColor = System.Drawing.Color.Black;
            this.btn_ydec.Location = new System.Drawing.Point(84, 20);
            this.btn_ydec.Name = "btn_ydec";
            this.btn_ydec.Size = new System.Drawing.Size(66, 42);
            this.btn_ydec.TabIndex = 29;
            this.btn_ydec.Text = "Y-";
            this.btn_ydec.UseVisualStyleBackColor = false;
            this.btn_ydec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_ydec.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_ydec.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_ydec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // numtxt_dist
            // 
            this.numtxt_dist.IsActivated = false;
            this.numtxt_dist.IsDecimal = true;
            this.numtxt_dist.Location = new System.Drawing.Point(167, 178);
            this.numtxt_dist.Margin = new System.Windows.Forms.Padding(4);
            this.numtxt_dist.MaxValue = 100D;
            this.numtxt_dist.MinValue = 0D;
            this.numtxt_dist.Name = "numtxt_dist";
            this.numtxt_dist.Size = new System.Drawing.Size(233, 34);
            this.numtxt_dist.TabIndex = 32;
            this.numtxt_dist.Tips = "步长：";
            this.numtxt_dist.Unit = "(单位)";
            this.numtxt_dist.Value = 0D;
            // 
            // btn_xadd
            // 
            this.btn_xadd.BackColor = System.Drawing.Color.Gray;
            this.btn_xadd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_xadd.ForeColor = System.Drawing.Color.Black;
            this.btn_xadd.Location = new System.Drawing.Point(158, 62);
            this.btn_xadd.Name = "btn_xadd";
            this.btn_xadd.Size = new System.Drawing.Size(66, 42);
            this.btn_xadd.TabIndex = 41;
            this.btn_xadd.Text = "X+";
            this.btn_xadd.UseVisualStyleBackColor = false;
            this.btn_xadd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_xadd.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_xadd.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_xadd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_xdec
            // 
            this.btn_xdec.BackColor = System.Drawing.Color.Gray;
            this.btn_xdec.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_xdec.ForeColor = System.Drawing.Color.Black;
            this.btn_xdec.Location = new System.Drawing.Point(8, 62);
            this.btn_xdec.Name = "btn_xdec";
            this.btn_xdec.Size = new System.Drawing.Size(66, 42);
            this.btn_xdec.TabIndex = 39;
            this.btn_xdec.Text = "X-";
            this.btn_xdec.UseVisualStyleBackColor = false;
            this.btn_xdec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_xdec.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_xdec.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_xdec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_zadd
            // 
            this.btn_zadd.BackColor = System.Drawing.Color.Gray;
            this.btn_zadd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_zadd.ForeColor = System.Drawing.Color.Black;
            this.btn_zadd.Location = new System.Drawing.Point(327, 20);
            this.btn_zadd.Name = "btn_zadd";
            this.btn_zadd.Size = new System.Drawing.Size(66, 42);
            this.btn_zadd.TabIndex = 40;
            this.btn_zadd.Text = "Z+";
            this.btn_zadd.UseVisualStyleBackColor = false;
            this.btn_zadd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_zadd.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_zadd.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_zadd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // AxisDebugXYZZU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "AxisDebugXYZZU";
            this.Size = new System.Drawing.Size(413, 226);
            this.Load += new System.EventHandler(this.AxisDebugXYZZU_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button btn_uadd;
        internal System.Windows.Forms.Button btn_udec;
        internal System.Windows.Forms.Button btn_z2dec;
        internal System.Windows.Forms.Button btn_z2add;
        internal System.Windows.Forms.Button btn_yadd;
        private System.Windows.Forms.ComboBox cbo_movemode;
        internal System.Windows.Forms.Button btn_zdec;
        private System.Windows.Forms.ComboBox cbo_speedmode;
        internal System.Windows.Forms.Button btn_ydec;
        private NumericInputBox numtxt_dist;
        internal System.Windows.Forms.Button btn_xadd;
        internal System.Windows.Forms.Button btn_xdec;
        internal System.Windows.Forms.Button btn_zadd;
    }
}
