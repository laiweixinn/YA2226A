namespace LZ.CNC.Measurement.Forms
{
    partial class FrValcunmMsg
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
            this.lblmsg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_continue = new System.Windows.Forms.Button();
            this.btn_closebuzzer = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.groupBoxEx1 = new LZ.CNC.Measurement.Forms.Controls.GroupBoxEx(this.components);
            this.btn_valcunm_blow = new System.Windows.Forms.Button();
            this.groupBoxEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblmsg
            // 
            this.lblmsg.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblmsg.ForeColor = System.Drawing.Color.Red;
            this.lblmsg.Location = new System.Drawing.Point(77, 74);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(414, 35);
            this.lblmsg.TabIndex = 4;
            this.lblmsg.Text = "！！！";
            this.lblmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(20, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 34);
            this.label1.TabIndex = 3;
            this.label1.Text = "提示：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_continue
            // 
            this.btn_continue.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_continue.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_continue.Location = new System.Drawing.Point(367, 155);
            this.btn_continue.Name = "btn_continue";
            this.btn_continue.Size = new System.Drawing.Size(65, 30);
            this.btn_continue.TabIndex = 8;
            this.btn_continue.Text = "继续";
            this.btn_continue.UseVisualStyleBackColor = false;
            this.btn_continue.Click += new System.EventHandler(this.btn_continue_Click);
            // 
            // btn_closebuzzer
            // 
            this.btn_closebuzzer.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_closebuzzer.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_closebuzzer.Location = new System.Drawing.Point(204, 155);
            this.btn_closebuzzer.Name = "btn_closebuzzer";
            this.btn_closebuzzer.Size = new System.Drawing.Size(65, 30);
            this.btn_closebuzzer.TabIndex = 9;
            this.btn_closebuzzer.Text = "静音";
            this.btn_closebuzzer.UseVisualStyleBackColor = false;
            this.btn_closebuzzer.Click += new System.EventHandler(this.btn_closebuzzer_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_stop.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_stop.Location = new System.Drawing.Point(438, 155);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(65, 30);
            this.btn_stop.TabIndex = 10;
            this.btn_stop.Text = "停机";
            this.btn_stop.UseVisualStyleBackColor = false;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // groupBoxEx1
            // 
            this.groupBoxEx1.BorderColor = System.Drawing.Color.Black;
            this.groupBoxEx1.Controls.Add(this.btn_valcunm_blow);
            this.groupBoxEx1.Controls.Add(this.btn_closebuzzer);
            this.groupBoxEx1.Controls.Add(this.label1);
            this.groupBoxEx1.Controls.Add(this.lblmsg);
            this.groupBoxEx1.Controls.Add(this.btn_continue);
            this.groupBoxEx1.Controls.Add(this.btn_stop);
            this.groupBoxEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEx1.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEx1.Name = "groupBoxEx1";
            this.groupBoxEx1.Size = new System.Drawing.Size(515, 197);
            this.groupBoxEx1.TabIndex = 11;
            this.groupBoxEx1.TabStop = false;
            // 
            // btn_valcunm_blow
            // 
            this.btn_valcunm_blow.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_valcunm_blow.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_valcunm_blow.Location = new System.Drawing.Point(275, 155);
            this.btn_valcunm_blow.Name = "btn_valcunm_blow";
            this.btn_valcunm_blow.Size = new System.Drawing.Size(86, 30);
            this.btn_valcunm_blow.TabIndex = 11;
            this.btn_valcunm_blow.Text = "破真空";
            this.btn_valcunm_blow.UseVisualStyleBackColor = false;
            // 
            // FrValcunmMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 197);
            this.Controls.Add(this.groupBoxEx1);
            this.Name = "FrValcunmMsg";
            this.Text = "FrValcunmMsg";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrValcunmMsg_Load);
            this.groupBoxEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Label lblmsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_continue;
        private System.Windows.Forms.Button btn_closebuzzer;
        private System.Windows.Forms.Button btn_stop;
        private Controls.GroupBoxEx groupBoxEx1;
        private System.Windows.Forms.Button btn_valcunm_blow;
    }
}