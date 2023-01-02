namespace LZ.CNC.Measurement.Forms
{
    partial class VisionFailProc
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
            this.btn_handvision = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.lbl_tips = new System.Windows.Forms.Label();
            this.btn_conti = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_handvision
            // 
            this.btn_handvision.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_handvision.Location = new System.Drawing.Point(18, 214);
            this.btn_handvision.Name = "btn_handvision";
            this.btn_handvision.Size = new System.Drawing.Size(130, 64);
            this.btn_handvision.TabIndex = 1;
            this.btn_handvision.Text = "手动对位";
            this.btn_handvision.UseVisualStyleBackColor = true;
            this.btn_handvision.Click += new System.EventHandler(this.btn_handvision_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_stop.Location = new System.Drawing.Point(386, 214);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(130, 64);
            this.btn_stop.TabIndex = 2;
            this.btn_stop.Text = "取消";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // lbl_tips
            // 
            this.lbl_tips.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_tips.Location = new System.Drawing.Point(12, 33);
            this.lbl_tips.Name = "lbl_tips";
            this.lbl_tips.Size = new System.Drawing.Size(504, 140);
            this.lbl_tips.TabIndex = 3;
            this.lbl_tips.Text = "Tips:<A>拍照失败!";
            this.lbl_tips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_conti
            // 
            this.btn_conti.Enabled = false;
            this.btn_conti.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_conti.Location = new System.Drawing.Point(204, 214);
            this.btn_conti.Name = "btn_conti";
            this.btn_conti.Size = new System.Drawing.Size(130, 64);
            this.btn_conti.TabIndex = 4;
            this.btn_conti.Text = "继续";
            this.btn_conti.UseVisualStyleBackColor = true;
            this.btn_conti.Click += new System.EventHandler(this.btn_conti_Click);
            // 
            // VisionFailProc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 359);
            this.Controls.Add(this.btn_conti);
            this.Controls.Add(this.lbl_tips);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_handvision);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisionFailProc";
            this.Text = "拍照NG处理";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_handvision;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Label lbl_tips;
        private System.Windows.Forms.Button btn_conti;
    }
}