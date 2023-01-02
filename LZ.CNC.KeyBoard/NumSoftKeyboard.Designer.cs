namespace LZ.CNC.KeyBoard
{
    partial class NumSoftKeyboard
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
            this.panel_titlebar = new System.Windows.Forms.Panel();
            this.lbl_close = new System.Windows.Forms.Label();
            this.lbl_tips = new System.Windows.Forms.Label();
            this.txt_maxvalue = new System.Windows.Forms.TextBox();
            this.txt_minvalue = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btn_backspace = new System.Windows.Forms.Button();
            this.btn_num9 = new System.Windows.Forms.Button();
            this.btn_num8 = new System.Windows.Forms.Button();
            this.txt_inputbox = new System.Windows.Forms.TextBox();
            this.btn_pulsminus = new System.Windows.Forms.Button();
            this.btn_num3 = new System.Windows.Forms.Button();
            this.btn_num6 = new System.Windows.Forms.Button();
            this.btn_num2 = new System.Windows.Forms.Button();
            this.btn_num5 = new System.Windows.Forms.Button();
            this.btn_num0 = new System.Windows.Forms.Button();
            this.btn_num1 = new System.Windows.Forms.Button();
            this.btn_num4 = new System.Windows.Forms.Button();
            this.btn_num7 = new System.Windows.Forms.Button();
            this.btn_dot = new System.Windows.Forms.Button();
            this.panel_titlebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_titlebar
            // 
            this.panel_titlebar.Controls.Add(this.lbl_close);
            this.panel_titlebar.Controls.Add(this.lbl_tips);
            this.panel_titlebar.Location = new System.Drawing.Point(1, 2);
            this.panel_titlebar.Name = "panel_titlebar";
            this.panel_titlebar.Size = new System.Drawing.Size(276, 20);
            this.panel_titlebar.TabIndex = 27;
            this.panel_titlebar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel_titlebar_MouseDown);
            this.panel_titlebar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel_titlebar_MouseMove);
            this.panel_titlebar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Panel_titlebar_MouseUp);
            // 
            // lbl_close
            // 
            this.lbl_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbl_close.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_close.ForeColor = System.Drawing.Color.Silver;
            this.lbl_close.Location = new System.Drawing.Point(237, 1);
            this.lbl_close.Name = "lbl_close";
            this.lbl_close.Size = new System.Drawing.Size(38, 19);
            this.lbl_close.TabIndex = 0;
            this.lbl_close.Text = "X";
            this.lbl_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_close.Click += new System.EventHandler(this.Lbl_close_Click);
            // 
            // lbl_tips
            // 
            this.lbl_tips.AutoSize = true;
            this.lbl_tips.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_tips.Location = new System.Drawing.Point(3, 5);
            this.lbl_tips.Name = "lbl_tips";
            this.lbl_tips.Size = new System.Drawing.Size(41, 12);
            this.lbl_tips.TabIndex = 0;
            this.lbl_tips.Text = "提示：";
            // 
            // txt_maxvalue
            // 
            this.txt_maxvalue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_maxvalue.Enabled = false;
            this.txt_maxvalue.Location = new System.Drawing.Point(226, 7);
            this.txt_maxvalue.Name = "txt_maxvalue";
            this.txt_maxvalue.Size = new System.Drawing.Size(65, 21);
            this.txt_maxvalue.TabIndex = 26;
            this.txt_maxvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_maxvalue.Visible = false;
            // 
            // txt_minvalue
            // 
            this.txt_minvalue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_minvalue.Enabled = false;
            this.txt_minvalue.Location = new System.Drawing.Point(110, 7);
            this.txt_minvalue.Name = "txt_minvalue";
            this.txt_minvalue.Size = new System.Drawing.Size(65, 21);
            this.txt_minvalue.TabIndex = 25;
            this.txt_minvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_minvalue.Visible = false;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(192, 10);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(35, 12);
            this.Label2.TabIndex = 24;
            this.Label2.Text = "Max：";
            this.Label2.Visible = false;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(77, 10);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 12);
            this.Label1.TabIndex = 23;
            this.Label1.Text = "Min：";
            this.Label1.Visible = false;
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.Location = new System.Drawing.Point(207, 196);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(70, 51);
            this.btn_OK.TabIndex = 21;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            this.btn_OK.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_OK.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_cancel
            // 
            this.btn_cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cancel.ForeColor = System.Drawing.Color.White;
            this.btn_cancel.Location = new System.Drawing.Point(207, 146);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(70, 51);
            this.btn_cancel.TabIndex = 20;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = false;
            this.btn_cancel.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_cancel.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_clear
            // 
            this.btn_clear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_clear.ForeColor = System.Drawing.Color.White;
            this.btn_clear.Location = new System.Drawing.Point(207, 98);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(70, 51);
            this.btn_clear.TabIndex = 19;
            this.btn_clear.Text = "清除";
            this.btn_clear.UseVisualStyleBackColor = false;
            this.btn_clear.Click += new System.EventHandler(this.Btn_clear_Click);
            this.btn_clear.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_clear.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_backspace
            // 
            this.btn_backspace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_backspace.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_backspace.ForeColor = System.Drawing.Color.White;
            this.btn_backspace.Location = new System.Drawing.Point(207, 49);
            this.btn_backspace.Name = "btn_backspace";
            this.btn_backspace.Size = new System.Drawing.Size(70, 51);
            this.btn_backspace.TabIndex = 18;
            this.btn_backspace.Text = "回退";
            this.btn_backspace.UseVisualStyleBackColor = false;
            this.btn_backspace.Click += new System.EventHandler(this.Btn_backspace_Click);
            this.btn_backspace.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_backspace.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num9
            // 
            this.btn_num9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num9.ForeColor = System.Drawing.Color.White;
            this.btn_num9.Location = new System.Drawing.Point(140, 49);
            this.btn_num9.Name = "btn_num9";
            this.btn_num9.Size = new System.Drawing.Size(70, 51);
            this.btn_num9.TabIndex = 17;
            this.btn_num9.Text = "9";
            this.btn_num9.UseVisualStyleBackColor = false;
            this.btn_num9.Click += new System.EventHandler(this.Num_Click);
            this.btn_num9.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num9.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num8
            // 
            this.btn_num8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num8.ForeColor = System.Drawing.Color.White;
            this.btn_num8.Location = new System.Drawing.Point(72, 49);
            this.btn_num8.Name = "btn_num8";
            this.btn_num8.Size = new System.Drawing.Size(70, 51);
            this.btn_num8.TabIndex = 7;
            this.btn_num8.Text = "8";
            this.btn_num8.UseVisualStyleBackColor = false;
            this.btn_num8.Click += new System.EventHandler(this.Num_Click);
            this.btn_num8.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num8.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // txt_inputbox
            // 
            this.txt_inputbox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_inputbox.Location = new System.Drawing.Point(4, 25);
            this.txt_inputbox.Name = "txt_inputbox";
            this.txt_inputbox.ReadOnly = true;
            this.txt_inputbox.Size = new System.Drawing.Size(272, 23);
            this.txt_inputbox.TabIndex = 6;
            this.txt_inputbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btn_pulsminus
            // 
            this.btn_pulsminus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_pulsminus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_pulsminus.ForeColor = System.Drawing.Color.White;
            this.btn_pulsminus.Location = new System.Drawing.Point(140, 196);
            this.btn_pulsminus.Name = "btn_pulsminus";
            this.btn_pulsminus.Size = new System.Drawing.Size(70, 51);
            this.btn_pulsminus.TabIndex = 15;
            this.btn_pulsminus.Text = "+/-";
            this.btn_pulsminus.UseVisualStyleBackColor = false;
            this.btn_pulsminus.Click += new System.EventHandler(this.Btn_pulsminus_Click);
            this.btn_pulsminus.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_pulsminus.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num3
            // 
            this.btn_num3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num3.ForeColor = System.Drawing.Color.White;
            this.btn_num3.Location = new System.Drawing.Point(140, 146);
            this.btn_num3.Name = "btn_num3";
            this.btn_num3.Size = new System.Drawing.Size(70, 51);
            this.btn_num3.TabIndex = 14;
            this.btn_num3.Text = "3";
            this.btn_num3.UseVisualStyleBackColor = false;
            this.btn_num3.Click += new System.EventHandler(this.Num_Click);
            this.btn_num3.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num3.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num6
            // 
            this.btn_num6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num6.ForeColor = System.Drawing.Color.White;
            this.btn_num6.Location = new System.Drawing.Point(140, 98);
            this.btn_num6.Name = "btn_num6";
            this.btn_num6.Size = new System.Drawing.Size(70, 51);
            this.btn_num6.TabIndex = 13;
            this.btn_num6.Text = "6";
            this.btn_num6.UseVisualStyleBackColor = false;
            this.btn_num6.Click += new System.EventHandler(this.Num_Click);
            this.btn_num6.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num6.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num2
            // 
            this.btn_num2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num2.ForeColor = System.Drawing.Color.White;
            this.btn_num2.Location = new System.Drawing.Point(72, 146);
            this.btn_num2.Name = "btn_num2";
            this.btn_num2.Size = new System.Drawing.Size(70, 51);
            this.btn_num2.TabIndex = 11;
            this.btn_num2.Text = "2";
            this.btn_num2.UseVisualStyleBackColor = false;
            this.btn_num2.Click += new System.EventHandler(this.Num_Click);
            this.btn_num2.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num2.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num5
            // 
            this.btn_num5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num5.ForeColor = System.Drawing.Color.White;
            this.btn_num5.Location = new System.Drawing.Point(72, 98);
            this.btn_num5.Name = "btn_num5";
            this.btn_num5.Size = new System.Drawing.Size(70, 51);
            this.btn_num5.TabIndex = 10;
            this.btn_num5.Text = "5";
            this.btn_num5.UseVisualStyleBackColor = false;
            this.btn_num5.Click += new System.EventHandler(this.Num_Click);
            this.btn_num5.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num5.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num0
            // 
            this.btn_num0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num0.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num0.ForeColor = System.Drawing.Color.White;
            this.btn_num0.Location = new System.Drawing.Point(3, 196);
            this.btn_num0.Name = "btn_num0";
            this.btn_num0.Size = new System.Drawing.Size(70, 51);
            this.btn_num0.TabIndex = 9;
            this.btn_num0.Text = "0";
            this.btn_num0.UseVisualStyleBackColor = false;
            this.btn_num0.Click += new System.EventHandler(this.Num_Click);
            this.btn_num0.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num0.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num1
            // 
            this.btn_num1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num1.ForeColor = System.Drawing.Color.White;
            this.btn_num1.Location = new System.Drawing.Point(3, 146);
            this.btn_num1.Name = "btn_num1";
            this.btn_num1.Size = new System.Drawing.Size(70, 51);
            this.btn_num1.TabIndex = 8;
            this.btn_num1.Text = "1";
            this.btn_num1.UseVisualStyleBackColor = false;
            this.btn_num1.Click += new System.EventHandler(this.Num_Click);
            this.btn_num1.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num1.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num4
            // 
            this.btn_num4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num4.ForeColor = System.Drawing.Color.White;
            this.btn_num4.Location = new System.Drawing.Point(3, 98);
            this.btn_num4.Name = "btn_num4";
            this.btn_num4.Size = new System.Drawing.Size(70, 51);
            this.btn_num4.TabIndex = 22;
            this.btn_num4.Text = "4";
            this.btn_num4.UseVisualStyleBackColor = false;
            this.btn_num4.Click += new System.EventHandler(this.Num_Click);
            this.btn_num4.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num4.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_num7
            // 
            this.btn_num7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_num7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_num7.ForeColor = System.Drawing.Color.White;
            this.btn_num7.Location = new System.Drawing.Point(3, 49);
            this.btn_num7.Name = "btn_num7";
            this.btn_num7.Size = new System.Drawing.Size(70, 51);
            this.btn_num7.TabIndex = 16;
            this.btn_num7.Text = "7";
            this.btn_num7.UseVisualStyleBackColor = false;
            this.btn_num7.Click += new System.EventHandler(this.Num_Click);
            this.btn_num7.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_num7.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // btn_dot
            // 
            this.btn_dot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_dot.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_dot.ForeColor = System.Drawing.Color.White;
            this.btn_dot.Location = new System.Drawing.Point(72, 196);
            this.btn_dot.Name = "btn_dot";
            this.btn_dot.Size = new System.Drawing.Size(70, 51);
            this.btn_dot.TabIndex = 28;
            this.btn_dot.Text = ".";
            this.btn_dot.UseVisualStyleBackColor = false;
            this.btn_dot.Click += new System.EventHandler(this.Btn_dot_Click);
            this.btn_dot.MouseEnter += new System.EventHandler(this.Mounse_Enter);
            this.btn_dot.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // NumSoftKeyboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(281, 253);
            this.Controls.Add(this.btn_dot);
            this.Controls.Add(this.btn_pulsminus);
            this.Controls.Add(this.panel_titlebar);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_backspace);
            this.Controls.Add(this.btn_num9);
            this.Controls.Add(this.btn_num8);
            this.Controls.Add(this.txt_inputbox);
            this.Controls.Add(this.btn_num3);
            this.Controls.Add(this.btn_num6);
            this.Controls.Add(this.btn_num2);
            this.Controls.Add(this.btn_num5);
            this.Controls.Add(this.btn_num0);
            this.Controls.Add(this.btn_num1);
            this.Controls.Add(this.btn_num4);
            this.Controls.Add(this.btn_num7);
            this.Controls.Add(this.txt_maxvalue);
            this.Controls.Add(this.txt_minvalue);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NumSoftKeyboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NumSoftKeyboard";
            this.Load += new System.EventHandler(this.NumSoftKeyboard_Load);
            this.panel_titlebar.ResumeLayout(false);
            this.panel_titlebar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Panel panel_titlebar;
        internal System.Windows.Forms.Label lbl_close;
        internal System.Windows.Forms.Label lbl_tips;
        internal System.Windows.Forms.TextBox txt_maxvalue;
        internal System.Windows.Forms.TextBox txt_minvalue;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btn_OK;
        internal System.Windows.Forms.Button btn_cancel;
        internal System.Windows.Forms.Button btn_clear;
        internal System.Windows.Forms.Button btn_backspace;
        internal System.Windows.Forms.Button btn_num9;
        internal System.Windows.Forms.Button btn_num8;
        internal System.Windows.Forms.TextBox txt_inputbox;
        internal System.Windows.Forms.Button btn_pulsminus;
        internal System.Windows.Forms.Button btn_num3;
        internal System.Windows.Forms.Button btn_num6;
        internal System.Windows.Forms.Button btn_num2;
        internal System.Windows.Forms.Button btn_num5;
        internal System.Windows.Forms.Button btn_num0;
        internal System.Windows.Forms.Button btn_num1;
        internal System.Windows.Forms.Button btn_num4;
        internal System.Windows.Forms.Button btn_num7;
        internal System.Windows.Forms.Button btn_dot;
    }
}