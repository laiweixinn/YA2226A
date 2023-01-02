namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class IOCylinder
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
            this.lbl_sen_down = new System.Windows.Forms.Label();
            this.lbl_sen_up = new System.Windows.Forms.Label();
            this.btn_m = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_sen_down
            // 
            this.lbl_sen_down.BackColor = System.Drawing.Color.Red;
            this.lbl_sen_down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_sen_down.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_sen_down.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lbl_sen_down.Location = new System.Drawing.Point(6, 25);
            this.lbl_sen_down.Margin = new System.Windows.Forms.Padding(6, 3, 6, 6);
            this.lbl_sen_down.Name = "lbl_sen_down";
            this.lbl_sen_down.Size = new System.Drawing.Size(22, 13);
            this.lbl_sen_down.TabIndex = 12;
            this.lbl_sen_down.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_sen_up
            // 
            this.lbl_sen_up.BackColor = System.Drawing.Color.Red;
            this.lbl_sen_up.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_sen_up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_sen_up.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_sen_up.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lbl_sen_up.Location = new System.Drawing.Point(6, 6);
            this.lbl_sen_up.Margin = new System.Windows.Forms.Padding(6, 6, 6, 3);
            this.lbl_sen_up.Name = "lbl_sen_up";
            this.lbl_sen_up.Size = new System.Drawing.Size(22, 13);
            this.lbl_sen_up.TabIndex = 11;
            this.lbl_sen_up.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_m
            // 
            this.btn_m.AutoSize = true;
            this.btn_m.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_m.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_m.FlatAppearance.BorderSize = 0;
            this.btn_m.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_m.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_m.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_m.Location = new System.Drawing.Point(43, 3);
            this.btn_m.Name = "btn_m";
            this.btn_m.Size = new System.Drawing.Size(180, 44);
            this.btn_m.TabIndex = 11;
            this.btn_m.Text = "平台上下气缸";
            this.btn_m.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_m.UseVisualStyleBackColor = false;
            this.btn_m.Click += new System.EventHandler(this.btn_m_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_m, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(226, 50);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lbl_sen_down, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_sen_up, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(34, 44);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // IOCylinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "IOCylinder";
            this.Size = new System.Drawing.Size(226, 50);
            this.Load += new System.EventHandler(this.IOCylinder_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_sen_down;
        private System.Windows.Forms.Label lbl_sen_up;
        public System.Windows.Forms.Button btn_m;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
