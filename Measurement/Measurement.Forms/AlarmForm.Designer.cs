namespace LZ.CNC.Measurement.Forms
{
    partial class AlarmForm
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
            this.rtxt_alarms = new System.Windows.Forms.RichTextBox();
            this.btn_QueryAlm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtxt_alarms
            // 
            this.rtxt_alarms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxt_alarms.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxt_alarms.Location = new System.Drawing.Point(3, 11);
            this.rtxt_alarms.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxt_alarms.Name = "rtxt_alarms";
            this.rtxt_alarms.Size = new System.Drawing.Size(702, 439);
            this.rtxt_alarms.TabIndex = 0;
            this.rtxt_alarms.Text = "";
            // 
            // btn_QueryAlm
            // 
            this.btn_QueryAlm.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_QueryAlm.Location = new System.Drawing.Point(711, 12);
            this.btn_QueryAlm.Name = "btn_QueryAlm";
            this.btn_QueryAlm.Size = new System.Drawing.Size(98, 33);
            this.btn_QueryAlm.TabIndex = 1;
            this.btn_QueryAlm.Text = "报警查询";
            this.btn_QueryAlm.UseVisualStyleBackColor = true;
            this.btn_QueryAlm.Click += new System.EventHandler(this.btn_QueryAlm_Click);
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 453);
            this.Controls.Add(this.btn_QueryAlm);
            this.Controls.Add(this.rtxt_alarms);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AlarmForm";
            this.Text = "AlarmForm";
            this.Load += new System.EventHandler(this.AlarmForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxt_alarms;
        private System.Windows.Forms.Button btn_QueryAlm;
    }
}