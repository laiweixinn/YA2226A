namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class StringInputBox
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
            this.lbl_tips = new System.Windows.Forms.Label();
            this.txt_inputbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_tips
            // 
            this.lbl_tips.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_tips.Location = new System.Drawing.Point(7, 8);
            this.lbl_tips.Name = "lbl_tips";
            this.lbl_tips.Size = new System.Drawing.Size(137, 16);
            this.lbl_tips.TabIndex = 7;
            this.lbl_tips.Text = "字符输入：";
            this.lbl_tips.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_inputbox
            // 
            this.txt_inputbox.BackColor = System.Drawing.Color.White;
            this.txt_inputbox.Font = new System.Drawing.Font("华文中宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_inputbox.Location = new System.Drawing.Point(150, 3);
            this.txt_inputbox.Name = "txt_inputbox";
            this.txt_inputbox.ReadOnly = true;
            this.txt_inputbox.Size = new System.Drawing.Size(100, 29);
            this.txt_inputbox.TabIndex = 6;
            this.txt_inputbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_inputbox.Click += new System.EventHandler(this.Txt_inputbox_Click);
            // 
            // StringInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_tips);
            this.Controls.Add(this.txt_inputbox);
            this.Name = "StringInputBox";
            this.Size = new System.Drawing.Size(253, 35);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lbl_tips;
        internal System.Windows.Forms.TextBox txt_inputbox;
    }
}
