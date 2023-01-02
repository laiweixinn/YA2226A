using LZ.CNC.Measurement.Forms.Controls;
namespace LZ.CNC.Measurement.Forms
{
    partial class NewFile
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
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.strtxt_filename = new LZ.CNC.Measurement.Forms.Controls.StringInputBox();
            this.SuspendLayout();
            // 
            // btn_cancel
            // 
            this.btn_cancel.BackColor = System.Drawing.Color.Gray;
            this.btn_cancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btn_cancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btn_cancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cancel.Font = new System.Drawing.Font("华文中宋", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_cancel.Location = new System.Drawing.Point(355, 3);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(33, 20);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "X";
            this.btn_cancel.UseVisualStyleBackColor = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(257, 14);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 21);
            this.btn_ok.TabIndex = 3;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // strtxt_filename
            // 
            this.strtxt_filename.ForeColor = System.Drawing.Color.Red;
            this.strtxt_filename.Location = new System.Drawing.Point(-1, 3);
            this.strtxt_filename.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.strtxt_filename.Name = "strtxt_filename";
            this.strtxt_filename.PasswordChar = '\0';
            this.strtxt_filename.Size = new System.Drawing.Size(251, 32);
            this.strtxt_filename.TabIndex = 4;
            this.strtxt_filename.Tips = "字符输入：";
            // 
            // NewFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(394, 47);
            this.Controls.Add(this.strtxt_filename);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewFile";
            this.Text = "NewFile";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btn_cancel;
        internal System.Windows.Forms.Button btn_ok;
        private StringInputBox strtxt_filename;
    }
}