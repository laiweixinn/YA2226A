namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class ValueBox
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
            this.txt_inputbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_inputbox
            // 
            this.txt_inputbox.BackColor = System.Drawing.Color.White;
            this.txt_inputbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_inputbox.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_inputbox.Location = new System.Drawing.Point(0, 0);
            this.txt_inputbox.Margin = new System.Windows.Forms.Padding(2);
            this.txt_inputbox.Name = "txt_inputbox";
            this.txt_inputbox.Size = new System.Drawing.Size(67, 22);
            this.txt_inputbox.TabIndex = 6;
            this.txt_inputbox.Text = "0";
            this.txt_inputbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_inputbox.Click += new System.EventHandler(this.Txt_inputbox_Click);
            this.txt_inputbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_inputbox_KeyPress);
            // 
            // ValueBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_inputbox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ValueBox";
            this.Size = new System.Drawing.Size(67, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_inputbox;
    }
}
