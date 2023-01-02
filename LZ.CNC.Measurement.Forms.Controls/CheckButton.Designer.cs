namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class CheckButton
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
            this.lbl_false = new System.Windows.Forms.Label();
            this.lbl_true = new System.Windows.Forms.Label();
            this.lbl_name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_false
            // 
            this.lbl_false.BackColor = System.Drawing.Color.Green;
            this.lbl_false.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_false.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_false.ForeColor = System.Drawing.Color.White;
            this.lbl_false.Location = new System.Drawing.Point(176, 2);
            this.lbl_false.Name = "lbl_false";
            this.lbl_false.Size = new System.Drawing.Size(46, 24);
            this.lbl_false.TabIndex = 5;
            this.lbl_false.Text = "关闭";
            this.lbl_false.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_false.Click += new System.EventHandler(this.lbl_false_Click);
            // 
            // lbl_true
            // 
            this.lbl_true.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lbl_true.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_true.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_true.ForeColor = System.Drawing.Color.White;
            this.lbl_true.Location = new System.Drawing.Point(129, 2);
            this.lbl_true.Name = "lbl_true";
            this.lbl_true.Size = new System.Drawing.Size(48, 24);
            this.lbl_true.TabIndex = 4;
            this.lbl_true.Text = "启用";
            this.lbl_true.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_true.Click += new System.EventHandler(this.lbl_true_Click);
            // 
            // lbl_name
            // 
            this.lbl_name.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_name.Location = new System.Drawing.Point(3, 2);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(127, 22);
            this.lbl_name.TabIndex = 3;
            this.lbl_name.Text = "########:";
            this.lbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CheckButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_false);
            this.Controls.Add(this.lbl_true);
            this.Controls.Add(this.lbl_name);
            this.Name = "CheckButton";
            this.Size = new System.Drawing.Size(223, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_false;
        private System.Windows.Forms.Label lbl_true;
        private System.Windows.Forms.Label lbl_name;
    }
}
