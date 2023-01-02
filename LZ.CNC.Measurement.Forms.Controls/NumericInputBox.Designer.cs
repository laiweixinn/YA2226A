namespace LZ.CNC.Measurement.Forms.Controls
{
    partial class NumericInputBox
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
            this.lbl_unit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_tips
            // 
            this.lbl_tips.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_tips.Location = new System.Drawing.Point(4, 6);
            this.lbl_tips.Name = "lbl_tips";
            this.lbl_tips.Size = new System.Drawing.Size(158, 22);
            this.lbl_tips.TabIndex = 5;
            this.lbl_tips.Text = "数字输入名称:";
            this.lbl_tips.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_inputbox
            // 
            this.txt_inputbox.BackColor = System.Drawing.Color.White;
            this.txt_inputbox.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_inputbox.Location = new System.Drawing.Point(163, 7);
            this.txt_inputbox.Margin = new System.Windows.Forms.Padding(2);
            this.txt_inputbox.Name = "txt_inputbox";
            this.txt_inputbox.Size = new System.Drawing.Size(63, 22);
            this.txt_inputbox.TabIndex = 6;
            this.txt_inputbox.Text = "0";
            this.txt_inputbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_inputbox.Click += new System.EventHandler(this.Txt_inputbox_Click);
            this.txt_inputbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_inputbox_KeyPress);
            // 
            // lbl_unit
            // 
            this.lbl_unit.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_unit.Location = new System.Drawing.Point(231, 6);
            this.lbl_unit.Name = "lbl_unit";
            this.lbl_unit.Size = new System.Drawing.Size(53, 22);
            this.lbl_unit.TabIndex = 7;
            this.lbl_unit.Text = "(单位)";
            this.lbl_unit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumericInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_unit);
            this.Controls.Add(this.txt_inputbox);
            this.Controls.Add(this.lbl_tips);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "NumericInputBox";
            this.Size = new System.Drawing.Size(291, 35);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lbl_tips;
        private System.Windows.Forms.TextBox txt_inputbox;
        internal System.Windows.Forms.Label lbl_unit;
    }
}
