namespace LZ.CNC.Measurement.Forms
{
    partial class AlarmDisplayForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dgv_Message = new System.Windows.Forms.DataGridView();
            this.btn_disbytime = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_disbydes = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radbtn_AlarmDaySelect = new System.Windows.Forms.RadioButton();
            this.radbtn_AlarmNightSelect = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Message)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 15);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(180, 26);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dgv_Message
            // 
            this.dgv_Message.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Message.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Message.Location = new System.Drawing.Point(7, 59);
            this.dgv_Message.Name = "dgv_Message";
            this.dgv_Message.ReadOnly = true;
            this.dgv_Message.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_Message.RowTemplate.Height = 23;
            this.dgv_Message.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Message.Size = new System.Drawing.Size(792, 690);
            this.dgv_Message.TabIndex = 2;
            // 
            // btn_disbytime
            // 
            this.btn_disbytime.Location = new System.Drawing.Point(223, 12);
            this.btn_disbytime.Name = "btn_disbytime";
            this.btn_disbytime.Size = new System.Drawing.Size(90, 38);
            this.btn_disbytime.TabIndex = 3;
            this.btn_disbytime.Text = "按时间显示";
            this.btn_disbytime.UseVisualStyleBackColor = true;
            this.btn_disbytime.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(671, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 38);
            this.button2.TabIndex = 4;
            this.button2.Text = "打开文件夹";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_disbydes
            // 
            this.btn_disbydes.Location = new System.Drawing.Point(330, 12);
            this.btn_disbydes.Name = "btn_disbydes";
            this.btn_disbydes.Size = new System.Drawing.Size(90, 38);
            this.btn_disbydes.TabIndex = 5;
            this.btn_disbydes.Text = "按次数显示";
            this.btn_disbydes.UseVisualStyleBackColor = true;
            this.btn_disbydes.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LZ.CNC.Properties.Resources._00j58PaICI79_1024;
            this.pictureBox1.Location = new System.Drawing.Point(767, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 37);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // radbtn_AlarmDaySelect
            // 
            this.radbtn_AlarmDaySelect.AutoSize = true;
            this.radbtn_AlarmDaySelect.Checked = true;
            this.radbtn_AlarmDaySelect.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radbtn_AlarmDaySelect.Location = new System.Drawing.Point(480, 4);
            this.radbtn_AlarmDaySelect.Name = "radbtn_AlarmDaySelect";
            this.radbtn_AlarmDaySelect.Size = new System.Drawing.Size(55, 24);
            this.radbtn_AlarmDaySelect.TabIndex = 7;
            this.radbtn_AlarmDaySelect.TabStop = true;
            this.radbtn_AlarmDaySelect.Text = "白班";
            this.radbtn_AlarmDaySelect.UseVisualStyleBackColor = true;
            // 
            // radbtn_AlarmNightSelect
            // 
            this.radbtn_AlarmNightSelect.AutoSize = true;
            this.radbtn_AlarmNightSelect.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radbtn_AlarmNightSelect.Location = new System.Drawing.Point(480, 33);
            this.radbtn_AlarmNightSelect.Name = "radbtn_AlarmNightSelect";
            this.radbtn_AlarmNightSelect.Size = new System.Drawing.Size(55, 24);
            this.radbtn_AlarmNightSelect.TabIndex = 8;
            this.radbtn_AlarmNightSelect.Text = "晚班";
            this.radbtn_AlarmNightSelect.UseVisualStyleBackColor = true;
            // 
            // AlarmDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(811, 755);
            this.Controls.Add(this.radbtn_AlarmNightSelect);
            this.Controls.Add(this.radbtn_AlarmDaySelect);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_disbydes);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_disbytime);
            this.Controls.Add(this.dgv_Message);
            this.Controls.Add(this.dateTimePicker1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AlarmDisplayForm";
            this.ShowIcon = false;
            this.Text = "Alarm";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Message)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dgv_Message;
        private System.Windows.Forms.Button btn_disbytime;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_disbydes;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radbtn_AlarmDaySelect;
        private System.Windows.Forms.RadioButton radbtn_AlarmNightSelect;
    }
}

