namespace LZ.CNC.UserLevel
{
    partial class LoginForm
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
            this.lbl_errortips = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.tabControlEx1 = new LZ.CNC.Measurement.Forms.Controls.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbl_usertype = new System.Windows.Forms.Label();
            this.cbo_usertype = new System.Windows.Forms.ComboBox();
            this.strtxt_password = new LZ.CNC.Measurement.Forms.Controls.StringInputBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_changePSW = new System.Windows.Forms.Button();
            this.strtxt_pswtwo = new LZ.CNC.Measurement.Forms.Controls.StringInputBox();
            this.strtxt_pswone = new LZ.CNC.Measurement.Forms.Controls.StringInputBox();
            this.strtxt_oldpsw = new LZ.CNC.Measurement.Forms.Controls.StringInputBox();
            this.lbl_logintypes = new System.Windows.Forms.Label();
            this.cbo_logintype = new System.Windows.Forms.ComboBox();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_errortips
            // 
            this.lbl_errortips.AutoSize = true;
            this.lbl_errortips.ForeColor = System.Drawing.Color.Red;
            this.lbl_errortips.Location = new System.Drawing.Point(212, 8);
            this.lbl_errortips.Name = "lbl_errortips";
            this.lbl_errortips.Size = new System.Drawing.Size(103, 13);
            this.lbl_errortips.TabIndex = 22;
            this.lbl_errortips.Text = "提示：密码错误！";
            this.lbl_errortips.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_close
            // 
            this.btn_close.Font = new System.Drawing.Font("华文中宋", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_close.Location = new System.Drawing.Point(315, 3);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(42, 22);
            this.btn_close.TabIndex = 23;
            this.btn_close.Text = "X";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(362, 270);
            this.tabControlEx1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlEx1.TabIndex = 0;
            this.tabControlEx1.TitelTextColorSelected = System.Drawing.SystemColors.Control;
            this.tabControlEx1.TitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabControlEx1.TitleColorDisSelected = System.Drawing.Color.Silver;
            this.tabControlEx1.TitleTextColorDisSelected = System.Drawing.SystemColors.ControlText;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbl_usertype);
            this.tabPage1.Controls.Add(this.cbo_usertype);
            this.tabPage1.Controls.Add(this.strtxt_password);
            this.tabPage1.Controls.Add(this.btn_OK);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(354, 240);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "登录管理";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbl_usertype
            // 
            this.lbl_usertype.AutoSize = true;
            this.lbl_usertype.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_usertype.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_usertype.Location = new System.Drawing.Point(88, 28);
            this.lbl_usertype.Name = "lbl_usertype";
            this.lbl_usertype.Size = new System.Drawing.Size(78, 17);
            this.lbl_usertype.TabIndex = 22;
            this.lbl_usertype.Text = "登陆类型：";
            // 
            // cbo_usertype
            // 
            this.cbo_usertype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_usertype.Font = new System.Drawing.Font("华文中宋", 10F);
            this.cbo_usertype.FormattingEnabled = true;
            this.cbo_usertype.Location = new System.Drawing.Point(171, 25);
            this.cbo_usertype.Name = "cbo_usertype";
            this.cbo_usertype.Size = new System.Drawing.Size(99, 23);
            this.cbo_usertype.TabIndex = 19;
            // 
            // strtxt_password
            // 
            this.strtxt_password.BackColor = System.Drawing.Color.White;
            this.strtxt_password.ForeColor = System.Drawing.Color.DimGray;
            this.strtxt_password.Location = new System.Drawing.Point(21, 105);
            this.strtxt_password.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.strtxt_password.Name = "strtxt_password";
            this.strtxt_password.PasswordChar = '\0';
            this.strtxt_password.Size = new System.Drawing.Size(253, 37);
            this.strtxt_password.TabIndex = 21;
            this.strtxt_password.Tips = "密码：";
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_OK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_OK.Font = new System.Drawing.Font("华文中宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.Location = new System.Drawing.Point(3, 197);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(348, 40);
            this.btn_OK.TabIndex = 20;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_changePSW);
            this.tabPage2.Controls.Add(this.strtxt_pswtwo);
            this.tabPage2.Controls.Add(this.strtxt_pswone);
            this.tabPage2.Controls.Add(this.strtxt_oldpsw);
            this.tabPage2.Controls.Add(this.lbl_logintypes);
            this.tabPage2.Controls.Add(this.cbo_logintype);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(354, 240);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "密码管理";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_changePSW
            // 
            this.btn_changePSW.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_changePSW.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_changePSW.Font = new System.Drawing.Font("华文中宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_changePSW.ForeColor = System.Drawing.Color.White;
            this.btn_changePSW.Location = new System.Drawing.Point(3, 197);
            this.btn_changePSW.Name = "btn_changePSW";
            this.btn_changePSW.Size = new System.Drawing.Size(348, 40);
            this.btn_changePSW.TabIndex = 27;
            this.btn_changePSW.Text = "确定";
            this.btn_changePSW.UseVisualStyleBackColor = false;
            this.btn_changePSW.Click += new System.EventHandler(this.btn_changePSW_Click);
            // 
            // strtxt_pswtwo
            // 
            this.strtxt_pswtwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strtxt_pswtwo.ForeColor = System.Drawing.Color.DimGray;
            this.strtxt_pswtwo.Location = new System.Drawing.Point(21, 151);
            this.strtxt_pswtwo.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.strtxt_pswtwo.Name = "strtxt_pswtwo";
            this.strtxt_pswtwo.PasswordChar = '*';
            this.strtxt_pswtwo.Size = new System.Drawing.Size(251, 36);
            this.strtxt_pswtwo.TabIndex = 26;
            this.strtxt_pswtwo.Tips = "密码确认：";
            // 
            // strtxt_pswone
            // 
            this.strtxt_pswone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strtxt_pswone.ForeColor = System.Drawing.Color.DimGray;
            this.strtxt_pswone.Location = new System.Drawing.Point(21, 105);
            this.strtxt_pswone.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.strtxt_pswone.Name = "strtxt_pswone";
            this.strtxt_pswone.PasswordChar = '*';
            this.strtxt_pswone.Size = new System.Drawing.Size(251, 36);
            this.strtxt_pswone.TabIndex = 25;
            this.strtxt_pswone.Tips = "新密码：";
            // 
            // strtxt_oldpsw
            // 
            this.strtxt_oldpsw.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strtxt_oldpsw.ForeColor = System.Drawing.Color.DimGray;
            this.strtxt_oldpsw.Location = new System.Drawing.Point(21, 59);
            this.strtxt_oldpsw.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.strtxt_oldpsw.Name = "strtxt_oldpsw";
            this.strtxt_oldpsw.PasswordChar = '*';
            this.strtxt_oldpsw.Size = new System.Drawing.Size(251, 36);
            this.strtxt_oldpsw.TabIndex = 24;
            this.strtxt_oldpsw.Tips = "原密码：";
            // 
            // lbl_logintypes
            // 
            this.lbl_logintypes.AutoSize = true;
            this.lbl_logintypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_logintypes.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_logintypes.Location = new System.Drawing.Point(88, 28);
            this.lbl_logintypes.Name = "lbl_logintypes";
            this.lbl_logintypes.Size = new System.Drawing.Size(78, 17);
            this.lbl_logintypes.TabIndex = 23;
            this.lbl_logintypes.Text = "登陆类型：";
            // 
            // cbo_logintype
            // 
            this.cbo_logintype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_logintype.Font = new System.Drawing.Font("华文中宋", 10F);
            this.cbo_logintype.FormattingEnabled = true;
            this.cbo_logintype.Location = new System.Drawing.Point(171, 25);
            this.cbo_logintype.Name = "cbo_logintype";
            this.cbo_logintype.Size = new System.Drawing.Size(99, 23);
            this.cbo_logintype.TabIndex = 22;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 270);
            this.Controls.Add(this.lbl_errortips);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.tabControlEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CNC.Measurement.Forms.Controls.TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        internal System.Windows.Forms.Label lbl_usertype;
        internal System.Windows.Forms.ComboBox cbo_usertype;
        private CNC.Measurement.Forms.Controls.StringInputBox strtxt_password;
        internal System.Windows.Forms.Button btn_OK;
        internal System.Windows.Forms.Button btn_changePSW;
        private CNC.Measurement.Forms.Controls.StringInputBox strtxt_pswtwo;
        private CNC.Measurement.Forms.Controls.StringInputBox strtxt_pswone;
        private CNC.Measurement.Forms.Controls.StringInputBox strtxt_oldpsw;
        internal System.Windows.Forms.Label lbl_logintypes;
        internal System.Windows.Forms.ComboBox cbo_logintype;
        private System.Windows.Forms.Label lbl_errortips;
        internal System.Windows.Forms.Button btn_close;
    }
}