namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class AxisMovePanelEx
    {
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
            this.lbl_position = new System.Windows.Forms.Label();
            this.lbl_axisname = new System.Windows.Forms.Label();
            this.btn_GoHome = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_AxisJogADD = new System.Windows.Forms.Button();
            this.btn_AxisJogSub = new System.Windows.Forms.Button();
            this.lbl_elp = new System.Windows.Forms.Label();
            this.lbl_eln = new System.Windows.Forms.Label();
            this.lbl_org = new System.Windows.Forms.Label();
            this.cbo_speedmode = new System.Windows.Forms.ComboBox();
            this.cbo_movemode = new System.Windows.Forms.ComboBox();
            this.numtxt_dist = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.btn_iostate1 = new LZ.CNC.Measurement.Forms.Controls.btn_iostate();
            this.alarm_IOPanel1 = new LZ.CNC.Measurement.Forms.Controls.Alarm_IOPanel();
            this.SuspendLayout();
            // 
            // lbl_position
            // 
            this.lbl_position.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbl_position.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_position.Location = new System.Drawing.Point(121, 6);
            this.lbl_position.Name = "lbl_position";
            this.lbl_position.Size = new System.Drawing.Size(58, 31);
            this.lbl_position.TabIndex = 17;
            this.lbl_position.Text = "1000.000";
            this.lbl_position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_axisname
            // 
            this.lbl_axisname.Location = new System.Drawing.Point(3, 9);
            this.lbl_axisname.Name = "lbl_axisname";
            this.lbl_axisname.Size = new System.Drawing.Size(108, 27);
            this.lbl_axisname.TabIndex = 18;
            this.lbl_axisname.Text = "轴名：###";
            this.lbl_axisname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_GoHome
            // 
            this.btn_GoHome.BackColor = System.Drawing.Color.White;
            this.btn_GoHome.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_GoHome.Location = new System.Drawing.Point(600, 5);
            this.btn_GoHome.Name = "btn_GoHome";
            this.btn_GoHome.Size = new System.Drawing.Size(66, 30);
            this.btn_GoHome.TabIndex = 13;
            this.btn_GoHome.Text = "回零";
            this.btn_GoHome.UseVisualStyleBackColor = false;
            this.btn_GoHome.Click += new System.EventHandler(this.btn_GoHome_Click);
            this.btn_GoHome.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_GoHome.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_stop
            // 
            this.btn_stop.BackColor = System.Drawing.Color.White;
            this.btn_stop.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_stop.Location = new System.Drawing.Point(528, 5);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(66, 31);
            this.btn_stop.TabIndex = 14;
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = false;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            this.btn_stop.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_stop.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_AxisJogADD
            // 
            this.btn_AxisJogADD.BackColor = System.Drawing.Color.White;
            this.btn_AxisJogADD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AxisJogADD.ForeColor = System.Drawing.Color.Black;
            this.btn_AxisJogADD.Location = new System.Drawing.Point(249, 6);
            this.btn_AxisJogADD.Name = "btn_AxisJogADD";
            this.btn_AxisJogADD.Size = new System.Drawing.Size(66, 31);
            this.btn_AxisJogADD.TabIndex = 15;
            this.btn_AxisJogADD.Text = "+";
            this.btn_AxisJogADD.UseVisualStyleBackColor = false;
            this.btn_AxisJogADD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_AxisJogADD.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_AxisJogADD.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_AxisJogADD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // btn_AxisJogSub
            // 
            this.btn_AxisJogSub.BackColor = System.Drawing.Color.White;
            this.btn_AxisJogSub.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AxisJogSub.ForeColor = System.Drawing.Color.Black;
            this.btn_AxisJogSub.Location = new System.Drawing.Point(185, 6);
            this.btn_AxisJogSub.Name = "btn_AxisJogSub";
            this.btn_AxisJogSub.Size = new System.Drawing.Size(63, 31);
            this.btn_AxisJogSub.TabIndex = 16;
            this.btn_AxisJogSub.Text = "-";
            this.btn_AxisJogSub.UseVisualStyleBackColor = false;
            this.btn_AxisJogSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.btn_AxisJogSub.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.btn_AxisJogSub.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.btn_AxisJogSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // lbl_elp
            // 
            this.lbl_elp.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbl_elp.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_elp.Location = new System.Drawing.Point(672, 5);
            this.lbl_elp.Name = "lbl_elp";
            this.lbl_elp.Size = new System.Drawing.Size(32, 29);
            this.lbl_elp.TabIndex = 22;
            this.lbl_elp.Text = "ELP";
            this.lbl_elp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_eln
            // 
            this.lbl_eln.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbl_eln.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_eln.Location = new System.Drawing.Point(748, 6);
            this.lbl_eln.Name = "lbl_eln";
            this.lbl_eln.Size = new System.Drawing.Size(32, 28);
            this.lbl_eln.TabIndex = 22;
            this.lbl_eln.Text = "ELN";
            this.lbl_eln.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_org
            // 
            this.lbl_org.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbl_org.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_org.Location = new System.Drawing.Point(710, 6);
            this.lbl_org.Name = "lbl_org";
            this.lbl_org.Size = new System.Drawing.Size(32, 28);
            this.lbl_org.TabIndex = 22;
            this.lbl_org.Text = "ORG";
            this.lbl_org.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.cbo_speedmode.Location = new System.Drawing.Point(456, 10);
            this.cbo_speedmode.Margin = new System.Windows.Forms.Padding(2);
            this.cbo_speedmode.Name = "cbo_speedmode";
            this.cbo_speedmode.Size = new System.Drawing.Size(67, 24);
            this.cbo_speedmode.TabIndex = 24;
            this.cbo_speedmode.SelectedIndexChanged += new System.EventHandler(this.cbo_speedmode_SelectedIndexChanged);
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
            this.cbo_movemode.Location = new System.Drawing.Point(385, 10);
            this.cbo_movemode.Margin = new System.Windows.Forms.Padding(2);
            this.cbo_movemode.Name = "cbo_movemode";
            this.cbo_movemode.Size = new System.Drawing.Size(67, 24);
            this.cbo_movemode.TabIndex = 25;
            this.cbo_movemode.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numtxt_dist
            // 
            this.numtxt_dist.IsActivated = false;
            this.numtxt_dist.IsDecimal = true;
            this.numtxt_dist.Location = new System.Drawing.Point(155, 3);
            this.numtxt_dist.Margin = new System.Windows.Forms.Padding(4);
            this.numtxt_dist.MaxValue = 350D;
            this.numtxt_dist.MinValue = 0D;
            this.numtxt_dist.Name = "numtxt_dist";
            this.numtxt_dist.Size = new System.Drawing.Size(230, 39);
            this.numtxt_dist.TabIndex = 21;
            this.numtxt_dist.Tips = "";
            this.numtxt_dist.Unit = "(单位)";
            this.numtxt_dist.Value = 0D;
            // 
            // btn_iostate1
            // 
            this.btn_iostate1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_iostate1.IO = null;
            this.btn_iostate1.IsOutPut = true;
            this.btn_iostate1.Location = new System.Drawing.Point(824, 5);
            this.btn_iostate1.Name = "btn_iostate1";
            this.btn_iostate1.Size = new System.Drawing.Size(52, 29);
            this.btn_iostate1.TabIndex = 26;
            this.btn_iostate1.Text = "btn_iostate1";
            // 
            // alarm_IOPanel1
            // 
            this.alarm_IOPanel1.IO = null;
            this.alarm_IOPanel1.IsOutPut = false;
            this.alarm_IOPanel1.Location = new System.Drawing.Point(785, 4);
            this.alarm_IOPanel1.Name = "alarm_IOPanel1";
            this.alarm_IOPanel1.Size = new System.Drawing.Size(34, 30);
            this.alarm_IOPanel1.TabIndex = 27;
            this.alarm_IOPanel1.Text = "alarm_IOPanel1";
            // 
            // AxisMovePanelEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.alarm_IOPanel1);
            this.Controls.Add(this.btn_iostate1);
            this.Controls.Add(this.cbo_movemode);
            this.Controls.Add(this.cbo_speedmode);
            this.Controls.Add(this.lbl_org);
            this.Controls.Add(this.lbl_eln);
            this.Controls.Add(this.lbl_elp);
            this.Controls.Add(this.lbl_position);
            this.Controls.Add(this.lbl_axisname);
            this.Controls.Add(this.btn_GoHome);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_AxisJogADD);
            this.Controls.Add(this.btn_AxisJogSub);
            this.Controls.Add(this.numtxt_dist);
            this.Name = "AxisMovePanelEx";
            this.Size = new System.Drawing.Size(879, 46);
            this.Load += new System.EventHandler(this.AxisStatePanel_Load);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Label lbl_position;
        internal System.Windows.Forms.Label lbl_axisname;
        internal System.Windows.Forms.Button btn_GoHome;
        internal System.Windows.Forms.Button btn_stop;
        internal System.Windows.Forms.Button btn_AxisJogADD;
        internal System.Windows.Forms.Button btn_AxisJogSub;
        private LZ.CNC.Measurement.Forms.Controls.NumericInputBox numtxt_dist;
        internal System.Windows.Forms.Label lbl_elp;
        internal System.Windows.Forms.Label lbl_eln;
        internal System.Windows.Forms.Label lbl_org;
        private System.Windows.Forms.ComboBox cbo_speedmode;
        private System.Windows.Forms.ComboBox cbo_movemode;
        private btn_iostate btn_iostate1;
        private Alarm_IOPanel alarm_IOPanel1;
    }
}
