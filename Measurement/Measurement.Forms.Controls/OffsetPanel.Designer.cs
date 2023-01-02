namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class OffsetPanel
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nib_asheight = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_beheight = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_csheight = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_bsheight = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_ceheight = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_aeheight = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nib_aslength = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_belength = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_cslength = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_bslength = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_celength = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_aelength = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nib_cewidth = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_cswidth = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_bewidth = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_bswidth = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_aewidth = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_aswidth = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_bendzoffset = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.nib_aendzoffset = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.str_codefirstold = new LZ.CNC.Measurement.Forms.Controls.StringInputBox();
            this.chkex_isuvdisabled = new LZ.CNC.Measurement.Forms.Controls.CheckBoxEx();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(983, 588);
            this.panel2.TabIndex = 18;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nib_asheight);
            this.groupBox3.Controls.Add(this.nib_beheight);
            this.groupBox3.Controls.Add(this.nib_csheight);
            this.groupBox3.Controls.Add(this.nib_bsheight);
            this.groupBox3.Controls.Add(this.nib_ceheight);
            this.groupBox3.Controls.Add(this.nib_aeheight);
            this.groupBox3.Location = new System.Drawing.Point(669, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(304, 284);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "喷嘴高度补偿";
            // 
            // nib_asheight
            // 
            this.nib_asheight.IsActivated = false;
            this.nib_asheight.IsDecimal = true;
            this.nib_asheight.Location = new System.Drawing.Point(6, 19);
            this.nib_asheight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_asheight.MaxValue = 5D;
            this.nib_asheight.MinValue = -5D;
            this.nib_asheight.Name = "nib_asheight";
            this.nib_asheight.Size = new System.Drawing.Size(298, 35);
            this.nib_asheight.TabIndex = 26;
            this.nib_asheight.Tips = "A起始点高度补偿：";
            this.nib_asheight.Unit = "(mm)";
            this.nib_asheight.Value = 0D;
            // 
            // nib_beheight
            // 
            this.nib_beheight.IsActivated = false;
            this.nib_beheight.IsDecimal = true;
            this.nib_beheight.Location = new System.Drawing.Point(6, 151);
            this.nib_beheight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_beheight.MaxValue = 5D;
            this.nib_beheight.MinValue = -5D;
            this.nib_beheight.Name = "nib_beheight";
            this.nib_beheight.Size = new System.Drawing.Size(298, 35);
            this.nib_beheight.TabIndex = 29;
            this.nib_beheight.Tips = "B结束点高度补偿：";
            this.nib_beheight.Unit = "(mm)";
            this.nib_beheight.Value = 0D;
            // 
            // nib_csheight
            // 
            this.nib_csheight.IsActivated = false;
            this.nib_csheight.IsDecimal = true;
            this.nib_csheight.Location = new System.Drawing.Point(6, 195);
            this.nib_csheight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_csheight.MaxValue = 5D;
            this.nib_csheight.MinValue = -5D;
            this.nib_csheight.Name = "nib_csheight";
            this.nib_csheight.Size = new System.Drawing.Size(298, 35);
            this.nib_csheight.TabIndex = 30;
            this.nib_csheight.Tips = "C起始点高度补偿：";
            this.nib_csheight.Unit = "(mm)";
            this.nib_csheight.Value = 0D;
            // 
            // nib_bsheight
            // 
            this.nib_bsheight.IsActivated = false;
            this.nib_bsheight.IsDecimal = true;
            this.nib_bsheight.Location = new System.Drawing.Point(6, 107);
            this.nib_bsheight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_bsheight.MaxValue = 5D;
            this.nib_bsheight.MinValue = -5D;
            this.nib_bsheight.Name = "nib_bsheight";
            this.nib_bsheight.Size = new System.Drawing.Size(298, 35);
            this.nib_bsheight.TabIndex = 28;
            this.nib_bsheight.Tips = "B起始点高度补偿：";
            this.nib_bsheight.Unit = "(mm)";
            this.nib_bsheight.Value = 0D;
            // 
            // nib_ceheight
            // 
            this.nib_ceheight.IsActivated = false;
            this.nib_ceheight.IsDecimal = true;
            this.nib_ceheight.Location = new System.Drawing.Point(6, 239);
            this.nib_ceheight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_ceheight.MaxValue = 5D;
            this.nib_ceheight.MinValue = -5D;
            this.nib_ceheight.Name = "nib_ceheight";
            this.nib_ceheight.Size = new System.Drawing.Size(298, 35);
            this.nib_ceheight.TabIndex = 31;
            this.nib_ceheight.Tips = "C结束点高度补偿：";
            this.nib_ceheight.Unit = "(mm)";
            this.nib_ceheight.Value = 0D;
            // 
            // nib_aeheight
            // 
            this.nib_aeheight.IsActivated = false;
            this.nib_aeheight.IsDecimal = true;
            this.nib_aeheight.Location = new System.Drawing.Point(6, 63);
            this.nib_aeheight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_aeheight.MaxValue = 5D;
            this.nib_aeheight.MinValue = -5D;
            this.nib_aeheight.Name = "nib_aeheight";
            this.nib_aeheight.Size = new System.Drawing.Size(298, 35);
            this.nib_aeheight.TabIndex = 27;
            this.nib_aeheight.Tips = "A结束点高度补偿：";
            this.nib_aeheight.Unit = "(mm)";
            this.nib_aeheight.Value = 0D;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nib_aslength);
            this.groupBox2.Controls.Add(this.nib_belength);
            this.groupBox2.Controls.Add(this.nib_cslength);
            this.groupBox2.Controls.Add(this.nib_bslength);
            this.groupBox2.Controls.Add(this.nib_celength);
            this.groupBox2.Controls.Add(this.nib_aelength);
            this.groupBox2.Location = new System.Drawing.Point(334, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 284);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y轴长度补偿";
            // 
            // nib_aslength
            // 
            this.nib_aslength.IsActivated = false;
            this.nib_aslength.IsDecimal = true;
            this.nib_aslength.Location = new System.Drawing.Point(6, 19);
            this.nib_aslength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_aslength.MaxValue = 10D;
            this.nib_aslength.MinValue = -10D;
            this.nib_aslength.Name = "nib_aslength";
            this.nib_aslength.Size = new System.Drawing.Size(298, 35);
            this.nib_aslength.TabIndex = 18;
            this.nib_aslength.Tips = "A边起始点长度补偿：";
            this.nib_aslength.Unit = "(mm)";
            this.nib_aslength.Value = 0D;
            // 
            // nib_belength
            // 
            this.nib_belength.IsActivated = false;
            this.nib_belength.IsDecimal = true;
            this.nib_belength.Location = new System.Drawing.Point(6, 151);
            this.nib_belength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_belength.MaxValue = 10D;
            this.nib_belength.MinValue = -10D;
            this.nib_belength.Name = "nib_belength";
            this.nib_belength.Size = new System.Drawing.Size(298, 35);
            this.nib_belength.TabIndex = 21;
            this.nib_belength.Tips = "B边结束点长度补偿：";
            this.nib_belength.Unit = "(mm)";
            this.nib_belength.Value = 0D;
            // 
            // nib_cslength
            // 
            this.nib_cslength.IsActivated = false;
            this.nib_cslength.IsDecimal = true;
            this.nib_cslength.Location = new System.Drawing.Point(6, 195);
            this.nib_cslength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_cslength.MaxValue = 10D;
            this.nib_cslength.MinValue = -10D;
            this.nib_cslength.Name = "nib_cslength";
            this.nib_cslength.Size = new System.Drawing.Size(298, 35);
            this.nib_cslength.TabIndex = 22;
            this.nib_cslength.Tips = "C边起始点长度补偿：";
            this.nib_cslength.Unit = "(mm)";
            this.nib_cslength.Value = 0D;
            // 
            // nib_bslength
            // 
            this.nib_bslength.IsActivated = false;
            this.nib_bslength.IsDecimal = true;
            this.nib_bslength.Location = new System.Drawing.Point(6, 107);
            this.nib_bslength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_bslength.MaxValue = 10D;
            this.nib_bslength.MinValue = -10D;
            this.nib_bslength.Name = "nib_bslength";
            this.nib_bslength.Size = new System.Drawing.Size(298, 35);
            this.nib_bslength.TabIndex = 20;
            this.nib_bslength.Tips = "B边起始点长度补偿：";
            this.nib_bslength.Unit = "(mm)";
            this.nib_bslength.Value = 0D;
            // 
            // nib_celength
            // 
            this.nib_celength.IsActivated = false;
            this.nib_celength.IsDecimal = true;
            this.nib_celength.Location = new System.Drawing.Point(6, 239);
            this.nib_celength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_celength.MaxValue = 10D;
            this.nib_celength.MinValue = -10D;
            this.nib_celength.Name = "nib_celength";
            this.nib_celength.Size = new System.Drawing.Size(298, 35);
            this.nib_celength.TabIndex = 23;
            this.nib_celength.Tips = "C边结束点长度补偿：";
            this.nib_celength.Unit = "(mm)";
            this.nib_celength.Value = 0D;
            // 
            // nib_aelength
            // 
            this.nib_aelength.IsActivated = false;
            this.nib_aelength.IsDecimal = true;
            this.nib_aelength.Location = new System.Drawing.Point(6, 63);
            this.nib_aelength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_aelength.MaxValue = 10D;
            this.nib_aelength.MinValue = -10D;
            this.nib_aelength.Name = "nib_aelength";
            this.nib_aelength.Size = new System.Drawing.Size(298, 35);
            this.nib_aelength.TabIndex = 19;
            this.nib_aelength.Tips = "A边结束点长度补偿：";
            this.nib_aelength.Unit = "(mm)";
            this.nib_aelength.Value = 0D;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nib_cewidth);
            this.groupBox1.Controls.Add(this.nib_cswidth);
            this.groupBox1.Controls.Add(this.nib_bewidth);
            this.groupBox1.Controls.Add(this.nib_bswidth);
            this.groupBox1.Controls.Add(this.nib_aewidth);
            this.groupBox1.Controls.Add(this.nib_aswidth);
            this.groupBox1.Location = new System.Drawing.Point(3, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 284);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X方向补偿";
            // 
            // nib_cewidth
            // 
            this.nib_cewidth.IsActivated = false;
            this.nib_cewidth.IsDecimal = true;
            this.nib_cewidth.Location = new System.Drawing.Point(6, 241);
            this.nib_cewidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_cewidth.MaxValue = 5D;
            this.nib_cewidth.MinValue = -4D;
            this.nib_cewidth.Name = "nib_cewidth";
            this.nib_cewidth.Size = new System.Drawing.Size(298, 35);
            this.nib_cewidth.TabIndex = 22;
            this.nib_cewidth.Tips = "C边结束点距离：";
            this.nib_cewidth.Unit = "(mm)";
            this.nib_cewidth.Value = 0D;
            // 
            // nib_cswidth
            // 
            this.nib_cswidth.IsActivated = false;
            this.nib_cswidth.IsDecimal = true;
            this.nib_cswidth.Location = new System.Drawing.Point(6, 198);
            this.nib_cswidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_cswidth.MaxValue = 5D;
            this.nib_cswidth.MinValue = -4D;
            this.nib_cswidth.Name = "nib_cswidth";
            this.nib_cswidth.Size = new System.Drawing.Size(298, 35);
            this.nib_cswidth.TabIndex = 21;
            this.nib_cswidth.Tips = "C边起始点距离：";
            this.nib_cswidth.Unit = "(mm)";
            this.nib_cswidth.Value = 0D;
            // 
            // nib_bewidth
            // 
            this.nib_bewidth.IsActivated = false;
            this.nib_bewidth.IsDecimal = true;
            this.nib_bewidth.Location = new System.Drawing.Point(6, 155);
            this.nib_bewidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_bewidth.MaxValue = 5D;
            this.nib_bewidth.MinValue = -4D;
            this.nib_bewidth.Name = "nib_bewidth";
            this.nib_bewidth.Size = new System.Drawing.Size(298, 35);
            this.nib_bewidth.TabIndex = 20;
            this.nib_bewidth.Tips = "B边结束点距离：";
            this.nib_bewidth.Unit = "(mm)";
            this.nib_bewidth.Value = 0D;
            // 
            // nib_bswidth
            // 
            this.nib_bswidth.IsActivated = false;
            this.nib_bswidth.IsDecimal = true;
            this.nib_bswidth.Location = new System.Drawing.Point(6, 112);
            this.nib_bswidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_bswidth.MaxValue = 5D;
            this.nib_bswidth.MinValue = -4D;
            this.nib_bswidth.Name = "nib_bswidth";
            this.nib_bswidth.Size = new System.Drawing.Size(298, 35);
            this.nib_bswidth.TabIndex = 19;
            this.nib_bswidth.Tips = "B边起始点距离：";
            this.nib_bswidth.Unit = "(mm)";
            this.nib_bswidth.Value = 0D;
            // 
            // nib_aewidth
            // 
            this.nib_aewidth.IsActivated = false;
            this.nib_aewidth.IsDecimal = true;
            this.nib_aewidth.Location = new System.Drawing.Point(6, 69);
            this.nib_aewidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_aewidth.MaxValue = 5D;
            this.nib_aewidth.MinValue = -5D;
            this.nib_aewidth.Name = "nib_aewidth";
            this.nib_aewidth.Size = new System.Drawing.Size(298, 35);
            this.nib_aewidth.TabIndex = 18;
            this.nib_aewidth.Tips = "A边结束点距离：";
            this.nib_aewidth.Unit = "(mm)";
            this.nib_aewidth.Value = 0D;
            // 
            // nib_aswidth
            // 
            this.nib_aswidth.IsActivated = false;
            this.nib_aswidth.IsDecimal = true;
            this.nib_aswidth.Location = new System.Drawing.Point(6, 26);
            this.nib_aswidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_aswidth.MaxValue = 5D;
            this.nib_aswidth.MinValue = -5D;
            this.nib_aswidth.Name = "nib_aswidth";
            this.nib_aswidth.Size = new System.Drawing.Size(298, 35);
            this.nib_aswidth.TabIndex = 17;
            this.nib_aswidth.Tips = "A边起始点距离：";
            this.nib_aswidth.Unit = "(mm)";
            this.nib_aswidth.Value = 0D;
            // 
            // nib_bendzoffset
            // 
            this.nib_bendzoffset.IsActivated = false;
            this.nib_bendzoffset.IsDecimal = true;
            this.nib_bendzoffset.Location = new System.Drawing.Point(344, 596);
            this.nib_bendzoffset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_bendzoffset.MaxValue = 1D;
            this.nib_bendzoffset.MinValue = -1D;
            this.nib_bendzoffset.Name = "nib_bendzoffset";
            this.nib_bendzoffset.Size = new System.Drawing.Size(298, 35);
            this.nib_bendzoffset.TabIndex = 46;
            this.nib_bendzoffset.Tips = "C点包角Z补偿：";
            this.nib_bendzoffset.Unit = "(mm)";
            this.nib_bendzoffset.Value = 0D;
            // 
            // nib_aendzoffset
            // 
            this.nib_aendzoffset.IsActivated = false;
            this.nib_aendzoffset.IsDecimal = true;
            this.nib_aendzoffset.Location = new System.Drawing.Point(7, 596);
            this.nib_aendzoffset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nib_aendzoffset.MaxValue = 1D;
            this.nib_aendzoffset.MinValue = -1D;
            this.nib_aendzoffset.Name = "nib_aendzoffset";
            this.nib_aendzoffset.Size = new System.Drawing.Size(298, 35);
            this.nib_aendzoffset.TabIndex = 45;
            this.nib_aendzoffset.Tips = "B点包角Z补偿：";
            this.nib_aendzoffset.Unit = "(mm)";
            this.nib_aendzoffset.Value = 0D;
            // 
            // str_codefirstold
            // 
            this.str_codefirstold.Location = new System.Drawing.Point(732, 647);
            this.str_codefirstold.Name = "str_codefirstold";
            this.str_codefirstold.PasswordChar = '\0';
            this.str_codefirstold.Size = new System.Drawing.Size(254, 35);
            this.str_codefirstold.TabIndex = 71;
            this.str_codefirstold.Tips = "解析字符：";
            // 
            // chkex_isuvdisabled
            // 
            this.chkex_isuvdisabled.FalseTip = "否";
            this.chkex_isuvdisabled.IsCkecked = false;
            this.chkex_isuvdisabled.Location = new System.Drawing.Point(492, 647);
            this.chkex_isuvdisabled.Name = "chkex_isuvdisabled";
            this.chkex_isuvdisabled.Size = new System.Drawing.Size(296, 34);
            this.chkex_isuvdisabled.TabIndex = 72;
            this.chkex_isuvdisabled.Tips = "玻璃ID解析：";
            this.chkex_isuvdisabled.TrueTip = "是";
            // 
            // OffsetPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkex_isuvdisabled);
            this.Controls.Add(this.str_codefirstold);
            this.Controls.Add(this.nib_bendzoffset);
            this.Controls.Add(this.nib_aendzoffset);
            this.Controls.Add(this.panel2);
            this.Name = "OffsetPanel";
            this.Size = new System.Drawing.Size(990, 696);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private NumericInputBox nib_asheight;
        private NumericInputBox nib_beheight;
        private NumericInputBox nib_csheight;
        private NumericInputBox nib_bsheight;
        private NumericInputBox nib_ceheight;
        private NumericInputBox nib_aeheight;
        private System.Windows.Forms.GroupBox groupBox2;
        private NumericInputBox nib_aslength;
        private NumericInputBox nib_belength;
        private NumericInputBox nib_cslength;
        private NumericInputBox nib_bslength;
        private NumericInputBox nib_celength;
        private NumericInputBox nib_aelength;
        private System.Windows.Forms.GroupBox groupBox1;
        private NumericInputBox nib_cewidth;
        private NumericInputBox nib_cswidth;
        private NumericInputBox nib_bewidth;
        private NumericInputBox nib_bswidth;
        private NumericInputBox nib_aewidth;
        private NumericInputBox nib_aswidth;
        private NumericInputBox nib_aendzoffset;
        private NumericInputBox nib_bendzoffset;
        private StringInputBox str_codefirstold;
        private CheckBoxEx chkex_isuvdisabled;
    }
}
