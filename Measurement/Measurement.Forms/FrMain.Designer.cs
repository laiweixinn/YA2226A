namespace LZ.CNC.Measurement.Forms
{
    partial class FrMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrMain));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_logo = new System.Windows.Forms.PictureBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label_version = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label_runtime = new System.Windows.Forms.Label();
            this.label_date = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbt_debug = new System.Windows.Forms.RadioButton();
            this.rbt_set = new System.Windows.Forms.RadioButton();
            this.rbt_io = new System.Windows.Forms.RadioButton();
            this.rbtn_product = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cTabControl1 = new LZ.CNC.Measurement.Forms.Controls.CTabControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_logo)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(925, 363);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "主页";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(925, 363);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IO";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(925, 363);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "调试";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(925, 363);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1440, 900);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox_logo);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.rbt_debug);
            this.panel1.Controls.Add(this.rbt_set);
            this.panel1.Controls.Add(this.rbt_io);
            this.panel1.Controls.Add(this.rbtn_product);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1434, 74);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox_logo
            // 
            this.pictureBox_logo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_logo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_logo.Image")));
            this.pictureBox_logo.Location = new System.Drawing.Point(1162, 3);
            this.pictureBox_logo.Name = "pictureBox_logo";
            this.pictureBox_logo.Size = new System.Drawing.Size(72, 68);
            this.pictureBox_logo.TabIndex = 49;
            this.pictureBox_logo.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label_version);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.label_runtime);
            this.panel6.Controls.Add(this.label_date);
            this.panel6.Location = new System.Drawing.Point(1240, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(191, 68);
            this.panel6.TabIndex = 50;
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_version.Location = new System.Drawing.Point(6, 5);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(56, 17);
            this.label_version.TabIndex = 2;
            this.label_version.Text = "系统版本";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(6, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 17);
            this.label10.TabIndex = 1;
            this.label10.Text = "运行时间";
            // 
            // label_runtime
            // 
            this.label_runtime.AutoSize = true;
            this.label_runtime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_runtime.Location = new System.Drawing.Point(61, 47);
            this.label_runtime.Name = "label_runtime";
            this.label_runtime.Size = new System.Drawing.Size(56, 17);
            this.label_runtime.TabIndex = 0;
            this.label_runtime.Text = "运行时间";
            // 
            // label_date
            // 
            this.label_date.AutoSize = true;
            this.label_date.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_date.Location = new System.Drawing.Point(6, 26);
            this.label_date.Name = "label_date";
            this.label_date.Size = new System.Drawing.Size(32, 17);
            this.label_date.TabIndex = 0;
            this.label_date.Text = "日期";
            // 
            // radioButton1
            // 
            this.radioButton1.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Checked = true;
            this.radioButton1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Orange;
            this.radioButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButton1.ImageIndex = 1;
            this.radioButton1.Location = new System.Drawing.Point(522, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(120, 68);
            this.radioButton1.TabIndex = 48;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "退出";
            this.radioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbt_debug
            // 
            this.rbt_debug.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbt_debug.BackColor = System.Drawing.Color.Transparent;
            this.rbt_debug.FlatAppearance.CheckedBackColor = System.Drawing.Color.Orange;
            this.rbt_debug.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbt_debug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbt_debug.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbt_debug.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbt_debug.ImageIndex = 1;
            this.rbt_debug.Location = new System.Drawing.Point(396, 4);
            this.rbt_debug.Name = "rbt_debug";
            this.rbt_debug.Size = new System.Drawing.Size(120, 68);
            this.rbt_debug.TabIndex = 47;
            this.rbt_debug.Text = "调试";
            this.rbt_debug.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbt_debug.UseVisualStyleBackColor = false;
            this.rbt_debug.CheckedChanged += new System.EventHandler(this.rbtn_run_Click);
            // 
            // rbt_set
            // 
            this.rbt_set.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbt_set.BackColor = System.Drawing.Color.Transparent;
            this.rbt_set.FlatAppearance.CheckedBackColor = System.Drawing.Color.Orange;
            this.rbt_set.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbt_set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbt_set.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbt_set.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbt_set.ImageIndex = 1;
            this.rbt_set.Location = new System.Drawing.Point(265, 3);
            this.rbt_set.Name = "rbt_set";
            this.rbt_set.Size = new System.Drawing.Size(125, 68);
            this.rbt_set.TabIndex = 46;
            this.rbt_set.Text = "设置";
            this.rbt_set.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbt_set.UseVisualStyleBackColor = false;
            this.rbt_set.CheckedChanged += new System.EventHandler(this.rbtn_run_Click);
            // 
            // rbt_io
            // 
            this.rbt_io.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbt_io.BackColor = System.Drawing.Color.Transparent;
            this.rbt_io.FlatAppearance.CheckedBackColor = System.Drawing.Color.Orange;
            this.rbt_io.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbt_io.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbt_io.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbt_io.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbt_io.ImageIndex = 1;
            this.rbt_io.Location = new System.Drawing.Point(134, 3);
            this.rbt_io.Name = "rbt_io";
            this.rbt_io.Size = new System.Drawing.Size(125, 68);
            this.rbt_io.TabIndex = 45;
            this.rbt_io.Text = "IO监控";
            this.rbt_io.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbt_io.UseVisualStyleBackColor = false;
            this.rbt_io.CheckedChanged += new System.EventHandler(this.rbtn_run_Click);
            // 
            // rbtn_product
            // 
            this.rbtn_product.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_product.BackColor = System.Drawing.Color.Transparent;
            this.rbtn_product.FlatAppearance.CheckedBackColor = System.Drawing.Color.Orange;
            this.rbtn_product.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbtn_product.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn_product.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_product.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbtn_product.ImageIndex = 1;
            this.rbtn_product.Location = new System.Drawing.Point(3, 3);
            this.rbtn_product.Name = "rbtn_product";
            this.rbtn_product.Size = new System.Drawing.Size(125, 68);
            this.rbtn_product.TabIndex = 44;
            this.rbtn_product.Text = "主页";
            this.rbtn_product.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_product.UseVisualStyleBackColor = false;
            this.rbtn_product.CheckedChanged += new System.EventHandler(this.rbtn_run_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 83);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1434, 814);
            this.panel2.TabIndex = 1;
            // 
            // cTabControl1
            // 
            this.cTabControl1.Location = new System.Drawing.Point(0, 0);
            this.cTabControl1.Name = "cTabControl1";
            this.cTabControl1.SelectedIndex = 0;
            this.cTabControl1.Size = new System.Drawing.Size(200, 100);
            this.cTabControl1.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 900);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrMain";
            this.Load += new System.EventHandler(this.FrMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_logo)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CTabControl cTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.RadioButton rbtn_product;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.RadioButton rbt_debug;
        public System.Windows.Forms.RadioButton rbt_set;
        public System.Windows.Forms.RadioButton rbt_io;
        private System.Windows.Forms.PictureBox pictureBox_logo;
        public System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label_runtime;
        private System.Windows.Forms.Label label_date;
    }
}