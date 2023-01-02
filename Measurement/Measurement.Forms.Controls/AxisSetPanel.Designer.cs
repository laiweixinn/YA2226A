using LZ.CNC.Measurement.Forms.Controls;
namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class AxisSetPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grp_axisparm = new System.Windows.Forms.GroupBox();
            this.numtxt_handnomal = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_softmin = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_softmax = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_handhih = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_handlow = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_homespeed = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_runspeed = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_accspeed = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_startspeed = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_leaddist = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.numtxt_pulsnum = new LZ.CNC.Measurement.Forms.Controls.NumericInputBox();
            this.lbl_homedir = new System.Windows.Forms.Label();
            this.lbl_softenabled = new System.Windows.Forms.Label();
            this.cbo_homedir = new System.Windows.Forms.ComboBox();
            this.chk_softenabled = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grp_axisparm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp_axisparm
            // 
            this.grp_axisparm.BackColor = System.Drawing.Color.White;
            this.grp_axisparm.Controls.Add(this.numtxt_handnomal);
            this.grp_axisparm.Controls.Add(this.numtxt_softmin);
            this.grp_axisparm.Controls.Add(this.numtxt_softmax);
            this.grp_axisparm.Controls.Add(this.numtxt_handhih);
            this.grp_axisparm.Controls.Add(this.numtxt_handlow);
            this.grp_axisparm.Controls.Add(this.numtxt_homespeed);
            this.grp_axisparm.Controls.Add(this.numtxt_runspeed);
            this.grp_axisparm.Controls.Add(this.numtxt_accspeed);
            this.grp_axisparm.Controls.Add(this.numtxt_startspeed);
            this.grp_axisparm.Controls.Add(this.numtxt_leaddist);
            this.grp_axisparm.Controls.Add(this.numtxt_pulsnum);
            this.grp_axisparm.Controls.Add(this.lbl_homedir);
            this.grp_axisparm.Controls.Add(this.lbl_softenabled);
            this.grp_axisparm.Controls.Add(this.cbo_homedir);
            this.grp_axisparm.Controls.Add(this.chk_softenabled);
            this.grp_axisparm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grp_axisparm.Location = new System.Drawing.Point(3, 400);
            this.grp_axisparm.Margin = new System.Windows.Forms.Padding(0);
            this.grp_axisparm.Name = "grp_axisparm";
            this.grp_axisparm.Size = new System.Drawing.Size(1115, 196);
            this.grp_axisparm.TabIndex = 7;
            this.grp_axisparm.TabStop = false;
            this.grp_axisparm.Text = "轴参数";
            // 
            // numtxt_handnomal
            // 
            this.numtxt_handnomal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_handnomal.IsActivated = false;
            this.numtxt_handnomal.IsDecimal = false;
            this.numtxt_handnomal.Location = new System.Drawing.Point(271, 55);
            this.numtxt_handnomal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_handnomal.MaxValue = 1000D;
            this.numtxt_handnomal.MinValue = 0D;
            this.numtxt_handnomal.Name = "numtxt_handnomal";
            this.numtxt_handnomal.Size = new System.Drawing.Size(229, 32);
            this.numtxt_handnomal.TabIndex = 6;
            this.numtxt_handnomal.Tips = "手动常速:";
            this.numtxt_handnomal.Unit = "(单位)";
            this.numtxt_handnomal.Value = 0D;
            // 
            // numtxt_softmin
            // 
            this.numtxt_softmin.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_softmin.IsActivated = false;
            this.numtxt_softmin.IsDecimal = false;
            this.numtxt_softmin.Location = new System.Drawing.Point(539, 54);
            this.numtxt_softmin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_softmin.MaxValue = 100D;
            this.numtxt_softmin.MinValue = -40D;
            this.numtxt_softmin.Name = "numtxt_softmin";
            this.numtxt_softmin.Size = new System.Drawing.Size(232, 32);
            this.numtxt_softmin.TabIndex = 5;
            this.numtxt_softmin.Tips = "软限位Min:";
            this.numtxt_softmin.Unit = "(单位)";
            this.numtxt_softmin.Value = 0D;
            // 
            // numtxt_softmax
            // 
            this.numtxt_softmax.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_softmax.IsActivated = false;
            this.numtxt_softmax.IsDecimal = false;
            this.numtxt_softmax.Location = new System.Drawing.Point(539, 20);
            this.numtxt_softmax.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_softmax.MaxValue = 1500D;
            this.numtxt_softmax.MinValue = 0D;
            this.numtxt_softmax.Name = "numtxt_softmax";
            this.numtxt_softmax.Size = new System.Drawing.Size(232, 32);
            this.numtxt_softmax.TabIndex = 5;
            this.numtxt_softmax.Tips = "软限位Max:";
            this.numtxt_softmax.Unit = "(单位)";
            this.numtxt_softmax.Value = 0D;
            // 
            // numtxt_handhih
            // 
            this.numtxt_handhih.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_handhih.IsActivated = false;
            this.numtxt_handhih.IsDecimal = false;
            this.numtxt_handhih.Location = new System.Drawing.Point(271, 89);
            this.numtxt_handhih.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_handhih.MaxValue = 5000D;
            this.numtxt_handhih.MinValue = 0D;
            this.numtxt_handhih.Name = "numtxt_handhih";
            this.numtxt_handhih.Size = new System.Drawing.Size(229, 32);
            this.numtxt_handhih.TabIndex = 5;
            this.numtxt_handhih.Tips = "手动高速:";
            this.numtxt_handhih.Unit = "(单位)";
            this.numtxt_handhih.Value = 0D;
            // 
            // numtxt_handlow
            // 
            this.numtxt_handlow.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_handlow.IsActivated = false;
            this.numtxt_handlow.IsDecimal = false;
            this.numtxt_handlow.Location = new System.Drawing.Point(270, 21);
            this.numtxt_handlow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_handlow.MaxValue = 1000D;
            this.numtxt_handlow.MinValue = 0D;
            this.numtxt_handlow.Name = "numtxt_handlow";
            this.numtxt_handlow.Size = new System.Drawing.Size(229, 32);
            this.numtxt_handlow.TabIndex = 5;
            this.numtxt_handlow.Tips = "手动低速:";
            this.numtxt_handlow.Unit = "(单位)";
            this.numtxt_handlow.Value = 0D;
            // 
            // numtxt_homespeed
            // 
            this.numtxt_homespeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_homespeed.IsActivated = false;
            this.numtxt_homespeed.IsDecimal = false;
            this.numtxt_homespeed.Location = new System.Drawing.Point(270, 122);
            this.numtxt_homespeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_homespeed.MaxValue = 2000D;
            this.numtxt_homespeed.MinValue = 0D;
            this.numtxt_homespeed.Name = "numtxt_homespeed";
            this.numtxt_homespeed.Size = new System.Drawing.Size(229, 32);
            this.numtxt_homespeed.TabIndex = 5;
            this.numtxt_homespeed.Tips = "回零速度:";
            this.numtxt_homespeed.Unit = "(单位)";
            this.numtxt_homespeed.Value = 0D;
            // 
            // numtxt_runspeed
            // 
            this.numtxt_runspeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_runspeed.IsActivated = false;
            this.numtxt_runspeed.IsDecimal = false;
            this.numtxt_runspeed.Location = new System.Drawing.Point(38, 156);
            this.numtxt_runspeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_runspeed.MaxValue = 10000D;
            this.numtxt_runspeed.MinValue = 0D;
            this.numtxt_runspeed.Name = "numtxt_runspeed";
            this.numtxt_runspeed.Size = new System.Drawing.Size(230, 32);
            this.numtxt_runspeed.TabIndex = 5;
            this.numtxt_runspeed.Tips = "运行速度:";
            this.numtxt_runspeed.Unit = "(单位)";
            this.numtxt_runspeed.Value = 0D;
            // 
            // numtxt_accspeed
            // 
            this.numtxt_accspeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_accspeed.IsActivated = false;
            this.numtxt_accspeed.IsDecimal = false;
            this.numtxt_accspeed.Location = new System.Drawing.Point(38, 122);
            this.numtxt_accspeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_accspeed.MaxValue = 5000D;
            this.numtxt_accspeed.MinValue = 0D;
            this.numtxt_accspeed.Name = "numtxt_accspeed";
            this.numtxt_accspeed.Size = new System.Drawing.Size(230, 32);
            this.numtxt_accspeed.TabIndex = 5;
            this.numtxt_accspeed.Tips = "加速度:";
            this.numtxt_accspeed.Unit = "(单位)";
            this.numtxt_accspeed.Value = 0D;
            // 
            // numtxt_startspeed
            // 
            this.numtxt_startspeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_startspeed.IsActivated = false;
            this.numtxt_startspeed.IsDecimal = false;
            this.numtxt_startspeed.Location = new System.Drawing.Point(38, 88);
            this.numtxt_startspeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_startspeed.MaxValue = 1000D;
            this.numtxt_startspeed.MinValue = 0D;
            this.numtxt_startspeed.Name = "numtxt_startspeed";
            this.numtxt_startspeed.Size = new System.Drawing.Size(230, 32);
            this.numtxt_startspeed.TabIndex = 5;
            this.numtxt_startspeed.Tips = "启动速度:";
            this.numtxt_startspeed.Unit = "(单位)";
            this.numtxt_startspeed.Value = 0D;
            // 
            // numtxt_leaddist
            // 
            this.numtxt_leaddist.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_leaddist.IsActivated = false;
            this.numtxt_leaddist.IsDecimal = false;
            this.numtxt_leaddist.Location = new System.Drawing.Point(38, 55);
            this.numtxt_leaddist.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_leaddist.MaxValue = 150D;
            this.numtxt_leaddist.MinValue = 0D;
            this.numtxt_leaddist.Name = "numtxt_leaddist";
            this.numtxt_leaddist.Size = new System.Drawing.Size(230, 32);
            this.numtxt_leaddist.TabIndex = 5;
            this.numtxt_leaddist.Tips = "导程:";
            this.numtxt_leaddist.Unit = "(单位)";
            this.numtxt_leaddist.Value = 0D;
            // 
            // numtxt_pulsnum
            // 
            this.numtxt_pulsnum.BackColor = System.Drawing.Color.White;
            this.numtxt_pulsnum.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numtxt_pulsnum.IsActivated = false;
            this.numtxt_pulsnum.IsDecimal = false;
            this.numtxt_pulsnum.Location = new System.Drawing.Point(38, 21);
            this.numtxt_pulsnum.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numtxt_pulsnum.MaxValue = 50000D;
            this.numtxt_pulsnum.MinValue = 0D;
            this.numtxt_pulsnum.Name = "numtxt_pulsnum";
            this.numtxt_pulsnum.Size = new System.Drawing.Size(230, 32);
            this.numtxt_pulsnum.TabIndex = 5;
            this.numtxt_pulsnum.Tips = "每圈脉冲数:";
            this.numtxt_pulsnum.Unit = "(单位)";
            this.numtxt_pulsnum.Value = 0D;
            // 
            // lbl_homedir
            // 
            this.lbl_homedir.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_homedir.Location = new System.Drawing.Point(608, 101);
            this.lbl_homedir.Name = "lbl_homedir";
            this.lbl_homedir.Size = new System.Drawing.Size(87, 15);
            this.lbl_homedir.TabIndex = 4;
            this.lbl_homedir.Text = "回零方向:";
            this.lbl_homedir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_softenabled
            // 
            this.lbl_softenabled.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_softenabled.Location = new System.Drawing.Point(608, 130);
            this.lbl_softenabled.Name = "lbl_softenabled";
            this.lbl_softenabled.Size = new System.Drawing.Size(87, 22);
            this.lbl_softenabled.TabIndex = 4;
            this.lbl_softenabled.Text = "软限位:";
            this.lbl_softenabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbo_homedir
            // 
            this.cbo_homedir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_homedir.Font = new System.Drawing.Font("宋体", 10F);
            this.cbo_homedir.FormattingEnabled = true;
            this.cbo_homedir.Location = new System.Drawing.Point(701, 95);
            this.cbo_homedir.Name = "cbo_homedir";
            this.cbo_homedir.Size = new System.Drawing.Size(70, 21);
            this.cbo_homedir.TabIndex = 3;
            // 
            // chk_softenabled
            // 
            this.chk_softenabled.Font = new System.Drawing.Font("宋体", 10F);
            this.chk_softenabled.Location = new System.Drawing.Point(698, 129);
            this.chk_softenabled.Name = "chk_softenabled";
            this.chk_softenabled.Size = new System.Drawing.Size(84, 26);
            this.chk_softenabled.TabIndex = 2;
            this.chk_softenabled.Text = "启用";
            this.chk_softenabled.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Orange;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13,
            this.Column14});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(169)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(-3, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Orange;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1115, 397);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridView1_RowStateChanged);
            // 
            // Column1
            // 
            this.Column1.FillWeight = 142.132F;
            this.Column1.HeaderText = "轴名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 96.75909F;
            this.Column2.HeaderText = "脉冲数每圈";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 96.75909F;
            this.Column3.HeaderText = "导程";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 96.75909F;
            this.Column4.HeaderText = "起始速度";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.FillWeight = 96.75909F;
            this.Column5.HeaderText = "加速度";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.FillWeight = 96.75909F;
            this.Column6.HeaderText = "常规运行速度";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.FillWeight = 96.75909F;
            this.Column7.HeaderText = "回零速度";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.FillWeight = 96.75909F;
            this.Column8.HeaderText = "手动低速";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.FillWeight = 96.75909F;
            this.Column9.HeaderText = "手动常速";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.FillWeight = 96.75909F;
            this.Column10.HeaderText = "手动高速";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // Column11
            // 
            this.Column11.FillWeight = 96.75909F;
            this.Column11.HeaderText = "软限位Max";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column12
            // 
            this.Column12.FillWeight = 96.75909F;
            this.Column12.HeaderText = "软限位Min";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // Column13
            // 
            this.Column13.FillWeight = 96.75909F;
            this.Column13.HeaderText = "软限位";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            // 
            // Column14
            // 
            this.Column14.FillWeight = 96.75909F;
            this.Column14.HeaderText = "回零方向";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.grp_axisparm);
            this.panel1.Location = new System.Drawing.Point(2, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1115, 596);
            this.panel1.TabIndex = 8;
            // 
            // AxisSetPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.panel1);
            this.Name = "AxisSetPanel";
            this.Size = new System.Drawing.Size(1120, 599);
            this.grp_axisparm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.GroupBox grp_axisparm;
        private NumericInputBox numtxt_softmin;
        private NumericInputBox numtxt_softmax;
        private NumericInputBox numtxt_handhih;
        private NumericInputBox numtxt_handlow;
        private NumericInputBox numtxt_homespeed;
        private NumericInputBox numtxt_runspeed;
        private NumericInputBox numtxt_accspeed;
        private NumericInputBox numtxt_startspeed;
        private NumericInputBox numtxt_leaddist;
        private NumericInputBox numtxt_pulsnum;
        internal System.Windows.Forms.Label lbl_homedir;
        internal System.Windows.Forms.Label lbl_softenabled;
        internal System.Windows.Forms.ComboBox cbo_homedir;
        internal System.Windows.Forms.CheckBox chk_softenabled;
        internal System.Windows.Forms.DataGridView dataGridView1;
        private NumericInputBox numtxt_handnomal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
    }
}
